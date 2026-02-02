using Secu.DataTransferObjects;

namespace Secu.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileDto> CreateAsync(CreateProfileDto dto, Guid userId);
        Task<ProfileDto> CreateAsync(CreateProfileDto dto);
        Task<ProfileDto?> GetByIdAsync(Guid Id);
        Task<SecurityOfficerDto?> GetByUserIdAsync(Guid userId);
        Task<LoginResponseDto?> LoginAsync(LoginDto dto);
        Task<LoginResponseDto?> OfficerLoginAsync(OfficerLoginDto dto);
    }
}
