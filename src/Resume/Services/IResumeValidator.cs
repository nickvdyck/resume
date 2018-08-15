using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resume.Services
{
    public interface IResumeValidator
    {
        Task<(bool IsValid, IList<string> Messages)> Validate(string location);
        Task<(bool IsValid, IList<string> Messages)> Validate(string location, string schema);
    }
}
