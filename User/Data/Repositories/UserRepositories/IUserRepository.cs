using User.DTOs.Input;

namespace User.Data.Repositories.UserRepositories;

public interface IUserRepository
{
    Task<Models.User?> GetUserById(int userId);
    Task<IEnumerable<Models.User>?> GetUsers();
    Task<IEnumerable<Models.User>?> GetUsersBySchool(int schoolId);
    Task<IEnumerable<Models.User>?> GetTeachersBySchool(int schoolId);
    Task<IEnumerable<Models.User>?> GetStudentsBySchool(int schoolId);
    Task<IEnumerable<Models.User>?> GetParentsBySchool(int schoolId);
    Task<int?> CreateUser(UserInputDto user);
    Task<bool> UpdateUser(int id, UserInputDto user);
    Task<bool> DeleteUser(int userId);

}