
using Dapper;
using User.DTOs.Input;
using User.Services;

namespace User.Data.Repositories.UserRepositories;

public class DapperUserRepository : IUserRepository
{
    private readonly SqlConnectionFactory _sqlConnectionFactory;
    public DapperUserRepository(SqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    
    public async Task<int?> CreateUser(UserInputDto user)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"INSERT INTO Users (Name, Email, Role, SchoolID) 
                            VALUES (@Name, @Email, @Role, @SchoolID) 
                            RETURNING ID";

        var id = await connection.ExecuteScalarAsync<int>(sql, user);

        return id;
    }

    public async Task<bool> DeleteUser(int userId)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = "DELETE FROM Users WHERE ID = @UserId";

        var linesAffected = await connection.ExecuteAsync(sql, new { UserId = userId });

        return linesAffected > 0;
    }

    public async Task<IEnumerable<Models.User>?> GetParentsBySchool(int schoolId)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = "SELECT * FROM Users WHERE SchoolID = @SchoolId AND Role = 'Parent'";

        var parents = await connection.QueryAsync<Models.User>(sql, new { SchoolId = schoolId });

        return parents;
    }

    public async Task<IEnumerable<Models.User>?> GetStudentsBySchool(int schoolId)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = "SELECT * FROM Users WHERE SchoolID = @SchoolId AND Role = 'Student'";

        var students = await connection.QueryAsync<Models.User>(sql, new { SchoolId = schoolId });

        return students;
    }

    public async Task<IEnumerable<Models.User>?> GetTeachersBySchool(int schoolId)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = "SELECT * FROM Users WHERE SchoolID = @SchoolId AND Role = 'Teacher'";

        var teachers = await connection.QueryAsync<Models.User>(sql, new { SchoolId = schoolId });

        return teachers;
    }

    public async Task<Models.User?> GetUserById(int userId)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = "SELECT * FROM Users WHERE ID = @UserId";

        var user = await connection.QueryFirstOrDefaultAsync<Models.User>(sql, new { UserId = userId });

        return user;
    }

    public async Task<IEnumerable<Models.User>?> GetUsers()
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = "SELECT * FROM Users";

        var users = await connection.QueryAsync<Models.User>(sql);

        return users;
    }

    public async Task<IEnumerable<Models.User>?> GetUsersBySchool(int schoolId)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = "SELECT * FROM Users WHERE SchoolID = @SchoolId";

        var users = await connection.QueryAsync<Models.User>(sql, new { SchoolId = schoolId });

        return users;
    }

    public async Task<bool> UpdateUser(int id, UserInputDto user)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = "UPDATE Users SET Name = @Name, Email = @Email, Role = @Role, SchoolID = @SchoolId WHERE ID = @Id";

        var linesAffected = await connection.ExecuteAsync(sql, 
                    new { Name = user.Name, Email = user.Email, Role = user.Role, SchoolId = user.SchoolID, 
                          Id = id });

        return linesAffected > 0;
    }
}
