namespace Sunflower.Finance
{
    /// <summary>
    /// Item parsed from the Quandl Code File.
    /// </summary>
    public class QuandlCodeItem
    {
        /// <summary>
        /// Database code of the item.
        /// </summary>
        public string DatabaseCode { get; set; }

        /// <summary>
        /// ISIN of the item.
        /// </summary>
        public string Isin { get; set; }

        /// <summary>
        /// Name of the database.
        /// </summary>
        public string Name { get; set; }
    }
}