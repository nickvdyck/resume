using Newtonsoft.Json;

namespace Resume.Models
{
    public class Language
    {
        [JsonProperty("language")]
        public string Name { get; set; }
        public string Fluency { get; set; }
    }
}
