using System.Collections.Generic;

namespace Resume.Models
{
    public class JsonResume
    {
        public PersonalInfo Basics { get; set; }

        public List<Work> Work { get; set; }

        public List<Education> Education { get; set; }

        public List<Skill> Skills { get; set; }

        public List<Language> Languages { get; set; }

        public List<Interest> Interests { get; set; }
    }
}
