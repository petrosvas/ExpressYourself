﻿namespace ExpressYourself.Entity_Framework.Types
{
    public class Countries
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TwoLetterCode { get; set; }
        public string ThreeLetterCode { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
