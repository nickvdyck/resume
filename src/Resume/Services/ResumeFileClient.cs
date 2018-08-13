using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Resume.Configuration;
using Resume.Models;

namespace Resume.Services
{
    public class ResumeFileClient : IResumeClient
    {
        private IFileProvider FileProvider { get; set; }

        private ResumeClientOptions Config { get; set; }

        private JsonResume Cache { get; set; }

        public ResumeFileClient(IFileProvider fileProvider, IOptions<ResumeClientOptions> options)
        {
            FileProvider = fileProvider;
            Config = options.Value;
        }

        public JsonResume GetResume()
        {
            var resumeFileInfo = FileProvider.GetFileInfo(Config.Location);
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
