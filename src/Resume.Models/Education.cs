using System;
using System.Collections.Generic;

namespace Resume.Models
{
    public class Education
    {
        public string Institution { get; set; }

        public string Area { get; set; }

        public string StudyType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<string> Courses { get; set; }

    }
}
