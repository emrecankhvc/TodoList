using TodoProject.Entities;
namespace TodoProject.Data.Interfaces
{
    public interface IUserNoteRepository
    {
        UserNote? GetById(Guid UserId);
        void Save(UserNote note);

    }
}