using TodoProject.Data.Interfaces;
using TodoProject.Business.Interfaces;
using TodoProject.Entities;

namespace TodoProject.Business.Services
{
    public class UserNoteService : IUserNoteService
    {
        private readonly IUserNoteRepository _repository;

        public UserNoteService(IUserNoteRepository repository)
        {
            _repository = repository;
        }

        public UserNote GetById(Guid userId)
        {
            return _repository.GetById(userId);
        }

        public void Save(UserNote note)
        {
            _repository.Save(note);

        }
    }
}
