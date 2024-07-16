using Grade.DTOs.Input;

namespace Grade.Services;

public interface ITermService
{
    public Task<TermDto?> GetTermAsync(int termId);
}