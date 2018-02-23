using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sunflower.Finance
{
    /// <summary>
    /// Parses Entries from a Quandl Code file that holds
    /// a list of available databases with their codes.
    /// </summary>
    internal sealed class QuandlCodeFileReader
    {
        /// <summary>
        /// Characters used to seperate values.
        /// </summary>
        private const char SeparatorCharacter = ',';

        /// <summary>
        /// Character used for escaping values in the csv format.
        /// Allows linebreaks to be part of the value.
        /// </summary>
        private const char EscapeCharacter = '\"';

        /// <summary>
        /// Token used to find ISIN values in the database names.
        /// </summary>
        private const string IsinKeyphrase = "| ISIN ";

        /// <summary>
        /// Length of an ISIN.
        /// </summary>
        private const int IsinLength = 12;

        /// <summary>
        /// Parses entries read from a Quandl Code File via the specified StreamReader.
        /// </summary>
        /// <param name="reader">StreamReader used to read the Quandl Code File.</param>
        /// <returns>A task that will complete with the list of parsed entries.</returns>
        public IEnumerable<QuandlCodeItem> Read(StreamReader reader)
        {
            var result = new List<QuandlCodeItem>();
            string line = null;

            // Continue while lines are left to read.
            while ((line = ReadLine(reader)) != null)
            {
                // Parse values and ISIN from line.
                var (code, name) = ReadValuesFromLine(line);
                var isin = ReadIsinFromName(name);

                // Skip is ISIN is not specified for the line.
                if (isin == null)
                {
                    continue;
                }

                // Convert to result item.
                result.Add(new QuandlCodeItem
                {
                    DatabaseCode = code,
                    Name = name,
                    Isin = isin
                });
            }

            return result
                // Eliminate duplicate ISIN entries.
                .GroupBy(x => x.Isin)
                .Select(g => g.First())
                .ToList();
        }

        /// <summary>
        /// Reads the values from an entry line in the Quandl Code File.
        /// </summary>
        /// <param name="line">Line with the values.</param>
        /// <returns>The code and name of the database represented by the line.</returns>
        private (string Code, string Name) ReadValuesFromLine(string line)
        {
            var index = line.IndexOf(SeparatorCharacter);
            var code = line.Substring(0, index);
            var name = line.Substring(index + 1);

            return (code, name);
        }

        /// <summary>
        /// Extracts the ISIN from the database name, if specified.
        /// </summary>
        /// <param name="name">Name of the database.</param>
        /// <returns>ISIN specified in the database name; or null if omitted.</returns>
        private string ReadIsinFromName(string name)
        {
            // Find ISIN position by token.
            var isinIndex = name.IndexOf(IsinKeyphrase);
            if (isinIndex == -1)
            {
                return null;
            }

            return name.Substring(isinIndex + IsinKeyphrase.Length, IsinLength);
        }

        /// <summary>
        /// Reads the next line that represents an entry in the Quandl Code File.
        /// Might contain linebreaks if escaped correctly.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private string ReadLine(StreamReader reader)
        {
            var totalLine = string.Empty;
            string line = null;

            // Concatenate multiple lines as long as unclosed
            // escape characters are present (escapes the linebreak).
            while ((line = reader.ReadLine()) != null)
            {
                totalLine += line;
                if (totalLine.Count(c => c == EscapeCharacter) % 2 == 0)
                {
                    return totalLine;
                }
            }

            // If no lines in file are left, there should be no trailing
            // content (unclosed escape character).
            if (!string.IsNullOrEmpty(totalLine))
            {
                throw new InvalidOperationException("Unclosed escape character.");
            }

            return null;
        }
    }
}