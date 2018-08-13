using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Resume.Validator
{
    [VersionOptionFromMember(MemberName = nameof(GetVersion))]
    public class Program
    {
        [Required]
        [Option(Description = "Path to resume.json file", ShortName = "r")]
        public string Resume { get; set; }

        [Url]
        [Option(Description = "Url to resume schema.", ShortName = "s")]
        public string SchemaUrl { get; set; } = "https://raw.githubusercontent.com/jsonresume/resume-schema/v1.0.0/schema.json";

        public async Task<int> OnExecuteAsync()
        {
            try
            {
                var validator = new ResumeValidator(SchemaUrl, Resume);
                Tuple<bool, IList<string>> result = await validator.Validate();
                var isValid = result.Item1;
                var messages = result.Item2;

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

        public static Task<int> Main(string[] args) => CommandLineApplication.ExecuteAsync<Program>(args);

        private static string GetVersion()
            => typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}
