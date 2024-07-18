using Homework.DTOs;

namespace Homework.Services;

public interface ITermService
{
    public Task<TermDto?> GetTermAsync(int termId);
}