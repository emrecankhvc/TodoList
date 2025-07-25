
using TodoProject.Entities;

namespace TodoProject.Business.Interfaces
{
    public interface IUserNoteService
    {
        UserNote? GetById(Guid userId);
        void Save(UserNote note);
    }
}
