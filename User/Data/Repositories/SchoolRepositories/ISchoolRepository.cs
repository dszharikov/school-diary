using User.DTOs.Input;
using User.Models;

namespace User.Data.Repositories.SchoolRepositories;

public interface ISchoolRepository
{
    Task<IEnumerable<School>?> GetSchools();
    Task<School?> GetSchool(int id);

    Task<int> CreateSchool(SchoolInputDto school);

    Task<bool> UpdateSchool(int id, School school);
}