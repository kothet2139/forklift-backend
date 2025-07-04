using ForkliftControlSystem.Application.DTOs;

namespace ForkliftControlSystem.Application.Interfaces.Services;

public interface IForkliftService
{
    Task<IEnumerable<ForkliftDto>> GetAllForklifts();
    Task<PagedResult<ForkliftDto>> GetPaginatedForklifts(int page, int pageSize);
    Task<PagedResult<ForkliftDto>> GetPaginatedForkliftsAsync(int page, int pageSize);
    Task SeedForkliftDataAsync();
}
