using TodoProject.Entities;


namespace TodoProject.Data.Interfaces
{
    public interface IUserRepository
    {
        User? GetById(Guid id);
        User? GetByUsername(string username);

        void Add(User user);
        void Update(User user);
        void Save();

        string? GetFullName(Guid id);

    }
}
