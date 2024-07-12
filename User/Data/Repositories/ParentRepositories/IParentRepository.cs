namespace User.Data.Repositories.ParentRepositories;

public interface IParentRepository
{
    Task<int?> GetStudentId(int parentId);
}