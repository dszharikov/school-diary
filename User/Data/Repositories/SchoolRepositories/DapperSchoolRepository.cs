using Dapper;
using User.DTOs.Input;
using User.Models;
using User.Services;

namespace User.Data.Repositories.SchoolRepositories;

public class DapperSchoolRepository : ISchoolRepository
{
    private SqlConnectionFactory _sqlConnectionFactory;

    public DapperSchoolRepository(SqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }
    public async Task<int> CreateSchool(SchoolInputDto school)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = "INSERT INTO Schools (Name, Address) VALUES (@Name, @Address) RETURNING id;";

        var id = await connection.ExecuteScalarAsync<int>(sql, school);

        return id;
    }

    public async Task<School?> GetSchool(int id)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = "SELECT * FROM Schools WHERE Id = @Id";

        var school = await connection.QueryFirstOrDefaultAsync<School>(sql, new { Id = id });

        return school;
    }

    public async Task<IEnumerable<School>?> GetSchools()
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = "SELECT * FROM Schools";

        var schools = await connection.QueryAsync<School>(sql);

        return schools;
    }

    public async Task<bool> UpdateSchool(int id, School school)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = "UPDATE Schools SET Name = @Name, Address = @Address WHERE Id = @Id";

        var linesAffected = await connection.ExecuteAsync(sql, new { Name = school.Name, Address = school.Address, Id = id });

        return linesAffected > 0;
    }
}