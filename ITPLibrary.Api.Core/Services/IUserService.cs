using ITPLibrary.Api.Core.Dtos;
using System.Threading.Tasks;

namespace ITPLibrary.Api.Core.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterUserDto userDto);
        Task<string> LoginUserAsync(LoginUserDto userDto);
        Task<bool> RecoverPasswordAsync(PasswordRecoveryDto recoveryDto);
    }
}