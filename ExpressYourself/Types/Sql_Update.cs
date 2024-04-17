namespace ExpressYourself.Types
{
    public class Sql_Update
    {
        public List<Sql_UpdateData> Rows { get; set; } = new List<Sql_UpdateData>();
    }

    public class Sql_UpdateData
    {
        public string IP { get; set; }
        public string Name { get; set; }
        public string TwoLetterCode { get; set; }
        public string ThreeLetterCode { get; set; }
    }
}
