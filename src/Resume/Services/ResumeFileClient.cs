using System.IO;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Resume.Models;

namespace Resume.Services
{
    public class ResumeFileClient : IResumeClient
    {
        private IFileProvider FileProvider { get; set; }

        private JsonResume Cache { get; set; }

        public ResumeFileClient(IFileProvider fileProvider)
        {
            FileProvider = fileProvider;
        }

        public JsonResume GetResume(string location)
        {
            var resumeFileInfo = FileProvider.GetFileInfo(location);
            var stream = resumeFileInfo.CreateReadStream();
            var serializer = new JsonSerializer();
            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<JsonResume>(jsonTextReader);
            }
        }
    }
}
