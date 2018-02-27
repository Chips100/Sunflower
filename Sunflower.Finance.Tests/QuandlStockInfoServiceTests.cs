using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sunflower.Finance.Tests
{
    /// <summary>
    /// Tests for the QuandlStockInfoService.
    /// </summary>
    [TestClass]
    public class QuandlStockInfoServiceTests
    {
        /// <summary>
        /// Tests the extraction of the relevant value
        /// from the XML response from Quandl.
        /// </summary>
        [TestMethod]
        public void QuandlStockInfoService_ParsesValueFromXml()
        {
            var input = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<quandl-response>
  <dataset-data>
    <limit type=""integer"">1</limit>
    <transform nil=""true""/>
    <column-index nil=""true""/>
    <column-names type=""array"">
      <column-name>Date</column-name>
      <column-name>High</column-name>
      <column-name>Low</column-name>
      <column-name>Last</column-name>
      <column-name>Previous Day Price</column-name>
      <column-name>Volume</column-name>
    </column-names>
    <start-date type=""date"">2014-03-20</start-date>
    <end-date type=""date"">2018-02-26</end-date>
    <frequency>daily</frequency>
    <data type=""array"">
      <datum type=""array"">
        <datum type=""date"">2018-02-26</datum>
        <datum type=""float"">93.34</datum>
        <datum type=""float"">92.04</datum>
        <datum type=""float"">92.82</datum>
        <datum type=""float"">91.9</datum>
        <datum type=""float"">831.0</datum>
      </datum>
    </data>
    <collapse nil=""true""/>
    <order>asc</order>
  </dataset-data>
</quandl-response>";

            var result = QuandlStockInfoService.GetValueFromXmlResponse(input);
            Assert.AreEqual(92.82M, result);
        }
    }
}