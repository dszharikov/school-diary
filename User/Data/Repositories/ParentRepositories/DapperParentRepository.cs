
using Dapper;
using User.Services;

namespace User.Data.Repositories.ParentRepositories;

public class DapperParentRepository : IParentRepository
{
    private readonly SqlConnectionFactory _sqlConnectionFactory;

    public DapperParentRepository(SqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;    
    }
    
    public async Task<int?> GetStudentId(int parentId)
    {
        const string sql = "SELECT StudentID FROM ParentStudents WHERE ParentID = @ParentId";
        
        using var connection = _sqlConnectionFactory.CreateConnection();
        
        return await connection.QueryFirstOrDefaultAsync<int?>(sql, new { ParentId = parentId });
    }
}