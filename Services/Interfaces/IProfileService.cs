using Secu.DataTransferObjects;

namespace Secu.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ProfileDto> CreateAsync(CreateProfileDto dto, Guid userId);
        Task<ProfileDto?> GetByIdAsync(Guid id);
    }
}
