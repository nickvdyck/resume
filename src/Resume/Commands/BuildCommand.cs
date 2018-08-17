using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Resume.Services;
using Resume.Views.ViewModels;
using SelectPdf;

namespace Resume.Commands
{
    [Command(Description = "Build resume")]
    public class BuildCommand
    {
        [LegalFilePath]
        [Option(Description = "Path to resume.json file", ShortName = "l")]
        public string Location { get; set; } = "resume.json";

        [LegalFilePath]
        [Option(Description = "Path to resume.json file", ShortName = "o")]
        public string Output { get; set; } = "artifacts";

        private readonly RazorViewRenderer _razorViewRenderer;
        private readonly IResumeClient _resumeClient;

        public BuildCommand(RazorViewRenderer razorViewRenderer, IResumeClient resumeClient)
        {
            _razorViewRenderer = razorViewRenderer;
            _resumeClient = resumeClient;
        }

        public ResumeViewModel GetResumeViewModel()
        {

            var resume = _resumeClient.GetResume(Location);

            var model = new ResumeViewModel()
            {
                Name = resume.Basics.Name,
                JobTitle = resume.Basics.Label,
                Picture = resume.Basics.Picture,
                AboutMe = resume.Basics.Summary.Split("\n\n").ToList(),
                WorkPlaces = resume.Work,
                ContactInfo = new List<ContactRecord>(),
                Schools = resume.Education,
                Languages = resume.Languages,
            };

            model.ContactInfo.Add(new ContactRecord()
            {
                Type = "Email",
                Data = resume.Basics.Email,
            });

            foreach (var profile in resume.Basics.Profiles)
            {
                model.ContactInfo.Add(new ContactRecord()
                {
                    Type = profile.Network,
                    Data = profile.Url,
                });
            }

            return model;

        }

        public async Task OnExecuteAsync()
        {
            Console.WriteLine("Starting ...");
            var model = GetResumeViewModel();
            var page = await _razorViewRenderer.RenderViewToStringAsync("Views/Resume_Default.cshtml", model);

            Directory.CreateDirectory(Output);

            var resumeHtmlPath = $"{Output}/resume.html";
            var resumePdfPath = $"{Output}/resume.pdf";

            using (var stream = new FileStream(resumeHtmlPath, FileMode.Create))
            {
                var bytes = Encoding.UTF8.GetBytes(page);
                await stream.WriteAsync(bytes);
            }

            var converter = new HtmlToPdf();
            var document = converter.ConvertHtmlString(page);

            using (var stream = new FileStream(resumePdfPath, FileMode.Create))
            {
                document.Save(stream);
                document.Close();
            }

            Console.WriteLine("Done...");
        }
    }
}
