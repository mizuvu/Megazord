using System.Threading.Tasks;
using Zord.DomainActiveDirectory.Dtos;
using Zord.Result;

namespace Zord.DomainActiveDirectory.Interfaces
{
    public interface IActiveDirectoryService
    {
        /// <summary>
        /// Check userName & password from Active Directory
        /// </summary>
        Task<IResult> CheckPasswordSignInAsync(string userName, string password);

        /// <summary>
        /// Get User Infomation from Active Directory
        /// </summary>
        Task<IResult<DomainUserDto>> GetByUserNameAsync(string userName);
    }
}