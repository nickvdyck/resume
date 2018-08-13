using System;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Resume.Validator
{
    public class ResumeValidator
    {
        public string Schema { get; set; }

        public string ResumeJSON { get; set; }


        public ResumeValidator(string schema, string resume)
        {
            Schema = schema;
            ResumeJSON = resume;
        }

        public async Task<Tuple<bool, IList<string>>> Validate() {

            string fullPath = Path.GetFullPath(ResumeJSON);
            bool isValid = false;
            IList<string> messages;

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(Schema);
                var resumeSchema = await response.Content.ReadAsStringAsync();

                var resumeJson = File.ReadAllText(fullPath);

                JSchema schema = JSchema.Parse(resumeSchema);
                JObject resume = JObject.Parse(resumeJson);

                isValid = resume.IsValid(schema, out messages);
            }

            return new Tuple<bool, IList<string>>(isValid, messages);
        }
    }
}

