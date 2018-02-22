namespace Sunflower.Entities
{
    /// <summary>
    /// Represents a stock that is available for trading.
    /// </summary>
    public sealed class Stock : EntityBase
    {
        /// <summary>
        /// International securities identification number
        /// of the stock; serves as the unique identifier
        /// when synchronizing with other systems.
        /// </summary>
        public string Isin { get; set; }

        /// <summary>
        /// Name of the stock.
        /// </summary>
        public string Name { get; set; }
    }
}
