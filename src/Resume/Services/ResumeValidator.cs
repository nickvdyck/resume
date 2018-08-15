using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace Resume.Services
{
    public class ResumeValidator : IResumeValidator
    {
        private const string SCHEMA_URL = "https://raw.githubusercontent.com/jsonresume/resume-schema/v1.0.0/schema.json";
        private readonly HttpClient _httpClient;
        private readonly IFileProvider _fileProvider;

        public ResumeValidator(HttpClient httpClient, IFileProvider fileProvider)
        {
            _httpClient = httpClient;
            _fileProvider = fileProvider;
        }

        public Task<(bool IsValid, IList<string> Messages)> Validate(string location)
        {
            return Validate(location, SCHEMA_URL);
        }

        public async Task<(bool IsValid, IList<string> Messages)> Validate(string location, string schemaUrl)
        {
            bool isValid = false;
            IList<string> messages;

            var fileInfo = _fileProvider.GetFileInfo(location);
            var response = await _httpClient.GetAsync(schemaUrl);
            var resumeSchema = await response.Content.ReadAsStringAsync();

            using (var stream = fileInfo.CreateReadStream())
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var resume = await reader.ReadToEndAsync();

                var schema = JSchema.Parse(resumeSchema);
                var resumeObject = JObject.Parse(resume);

                isValid = resumeObject.IsValid(schema, out messages);
            }

            return (isValid, messages);
        }

    }
}
