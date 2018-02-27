using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Text;

namespace Sunflower.Finance.Tests
{
    /// <summary>
    /// Tests for the QuandlCodeFileReader.
    /// </summary>
    [TestClass]
    public class QuandlCodeFileReaderTests
    {
        /// <summary>
        /// Tests the correct parsing of a single entry.
        /// </summary>
        [TestMethod]
        public void QuandlCodeFileReader_ReadsSimpleEntry()
        {
            var codeFile = @"SSE/BEI,BEIERSDORF STOCK WKN 520000 | ISIN DE0005200000 (BEI) - Stuttgart";

            var sut = new QuandlCodeFileReader();
            var result = sut.Read(CreateStreamReaderFromString(codeFile)).ToArray();

            Assert.AreEqual(1, result.Length, "result.Length");
            var entry = result.Single();
            Assert.AreEqual("SSE/BEI", entry.DatabaseCode, "DatabaseCode");
            Assert.AreEqual("BEIERSDORF STOCK WKN 520000 | ISIN DE0005200000 (BEI) - Stuttgart", entry.Name, "Name");
            Assert.AreEqual("DE0005200000", entry.Isin, "Isin");
        }

        /// <summary>
        /// Tests skipping of entries with no detectable ISIN in the name.
        /// </summary>
        [TestMethod]
        public void QuandlCodeFileReader_SkipsEntryWithNoIsin()
        {
            var codeFile = @"SSE/BEI,BEIERSDORF STOCK WKN 520000 | ISxxxxIN DE0005200000 (BEI) - Stuttgart";

            var sut = new QuandlCodeFileReader();
            var result = sut.Read(CreateStreamReaderFromString(codeFile)).ToArray();

            Assert.AreEqual(0, result.Length, "result.Length");
        }

        /// <summary>
        /// Tests correct parsing of entries that span multiple
        /// lines (by escaping the linebreak with quotes).
        /// The linebreaks should be replaced by simple whitespaces.
        /// </summary>
        [TestMethod]
        public void QuandlCodeFileReader_ReadsMultiLineEntry()
        {
            var codeFile = @"SSE/BGO,""BG GROUP STOCK
WKN 931283 | ISIN GB0008762899(BGO) - Stuttgart""";

            var sut = new QuandlCodeFileReader();
            var result = sut.Read(CreateStreamReaderFromString(codeFile)).ToArray();

            Assert.AreEqual(1, result.Length, "result.Length");
            var entry = result.Single();
            Assert.AreEqual("SSE/BGO", entry.DatabaseCode, "DatabaseCode");
            Assert.AreEqual(@"BG GROUP STOCK WKN 931283 | ISIN GB0008762899(BGO) - Stuttgart", entry.Name, "Name");
            Assert.AreEqual("GB0008762899", entry.Isin, "Isin");
        }

        /// <summary>
        /// Tests the elimination of duplicate ISINs.
        /// </summary>
        [TestMethod]
        public void QuandlCodeFileReader_EliminatesDuplicateIsins()
        {
            var codeFile = @"SSE/BEI,BEIERSDORF STOCK WKN 520000 | ISIN DE0005200000 (BEI) - Stuttgart
SSE/NFC,NETFLIX STOCK WKN 552484 | ISIN US64110L1061 (NFC) - Stuttgart
SSE/BEI,DUPLICATE BEIERSDORF STOCK WKN 520000 | ISIN DE0005200000 (BEI) - Stuttgart";

            var sut = new QuandlCodeFileReader();
            var result = sut.Read(CreateStreamReaderFromString(codeFile)).ToArray();

            Assert.AreEqual(2, result.Length, "result.Length");

            var first = result[0];
            Assert.AreEqual("SSE/BEI", first.DatabaseCode, "first.DatabaseCode");
            Assert.AreEqual("BEIERSDORF STOCK WKN 520000 | ISIN DE0005200000 (BEI) - Stuttgart", first.Name, "first.Name");
            Assert.AreEqual("DE0005200000", first.Isin, "first.Isin");
            
            var second = result[1];
            Assert.AreEqual("SSE/NFC", second.DatabaseCode, "second.DatabaseCode");
            Assert.AreEqual("NETFLIX STOCK WKN 552484 | ISIN US64110L1061 (NFC) - Stuttgart", second.Name, "second.Name");
            Assert.AreEqual("US64110L1061", second.Isin, "second.Isin");
        }

        private StreamReader CreateStreamReaderFromString(string input)
        {
            var encoding = Encoding.UTF8;
            var memoryStream = new MemoryStream(encoding.GetBytes(input));
            return new StreamReader(memoryStream, encoding);
        }
    }
}