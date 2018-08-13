using System.Collections.Generic;
using Resume.Models;

namespace Resume.Views.ViewModels
{
    public class ResumeViewModel
    {
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string Picture { get; set; }
        public List<string> AboutMe { get; set; }

        public List<Work> WorkPlaces { get; set; }

        public List<Education> Schools { get; set; }

        public List<Language> Languages { get; set; }

        public List<ContactRecord> ContactInfo { get; set; }

    }

    public class ContactRecord
    {
        public string Type { get; set; }
        public string Data { get; set; }
    }
}
