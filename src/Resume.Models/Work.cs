using System;
using System.Collections.Generic;

namespace Resume.Models
{
    public class Work
    {
        public string Company { get; set; }

        public string Position { get; set; }

        public string Website { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Summary { get; set; }

        public List<string> Highlights { get; set; }
    }
}
