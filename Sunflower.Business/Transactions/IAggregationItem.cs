namespace Sunflower.Business.Transactions
{
    /// <summary>
    /// Interface for items that can be part of an aggregation (e.g. Transactions).
    /// </summary>
    public interface IAggregationItem
    {
        /// <summary>
        /// Applies the changes caused by this item to the accumulator.
        /// </summary>
        /// <param name="accumulator">Accumulator on which changes should be applied.</param>
        void Apply(AggregationAccumulator accumulator);
    }
}
