using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Resume.Services;

namespace Resume.Commands
{
    [Command(Description = "Validate a json resume file")]
    public class ValidateCommand
    {
        [LegalFilePath]
        [Option(Description = "Path to resume.json file", ShortName = "l")]
        public string Location { get; set; } = "resume.json";

        private readonly IResumeValidator _resumeValidator;

        public ValidateCommand(IResumeValidator resumeValidator)
        {
            _resumeValidator = resumeValidator;
        }

        public async Task<int> OnExecuteAsync()
        {
            try
            {
                (bool isValid, IList<string> messages) = await _resumeValidator.Validate(Location);

                Console.WriteLine("Your json resume is {0}", (isValid ? "valid" : "invalid:"));

                Console.WriteLine("---");

                foreach (var message in messages)
                {
                    Console.WriteLine($"- {message}");
                }

                Console.WriteLine("Done!");

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return 1;
            }
        }
    }
}
