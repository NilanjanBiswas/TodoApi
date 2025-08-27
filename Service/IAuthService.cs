using TodoApi.Dtos;

namespace TodoApi.Service
{
    public interface IAuthService
    {
        Task<AuthResponseDto> SignupAsync(SignupDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}
