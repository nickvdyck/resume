using System.Collections.Generic;

namespace Resume.Models
{
    public class PersonalInfo
    {
        public string Name { get; set; }

        public string Label { get; set; }

        public string Picture { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Website { get; set; }

        public string Summary { get; set; }

        public Location Location { get; set; }

        public List<OnlineProfile> Profiles { get; set; }
    }
}
