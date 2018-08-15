using Resume.Models;

namespace Resume.Services
{
    public interface IResumeClient
    {
         JsonResume GetResume(string path);
    }

}
