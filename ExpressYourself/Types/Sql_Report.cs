namespace ExpressYourself.Types
{
    public class Sql_Report
    {
        public List<Sql_ReportData> Rows { get; set; } = new List<Sql_ReportData>();
    }

    public class Sql_ReportData
    {
        public string CountryName { get; set; }
        public int AddressesCount { get; set; }
        public DateTime LastAddressUpdated { get; set; }
    }
}
