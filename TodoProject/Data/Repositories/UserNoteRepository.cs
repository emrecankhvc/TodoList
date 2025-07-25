using TodoProject.Entities;
using TodoProject.Data.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace TodoProject.Data.Repositories
{
    public class UserNoteRepository : IUserNoteRepository
    {
        private readonly DatabaseContext _context;

        public UserNoteRepository(DatabaseContext context)
        {
            _context = context;
        }

        public UserNote? GetById(Guid UserId)
        {
            return _context.UserNotes.FirstOrDefault(x => x.UserId == UserId);
        }
        public void Save(UserNote note)
        {
            var existingNote = _context.UserNotes.FirstOrDefault(x => x.UserId == note.UserId);

            if (existingNote == null)
            {

                _context.UserNotes.Add(note);
            }
            else
            {
                existingNote.Note = note.Note;
                
                
            }
            _context.SaveChanges();




        }
    }
}
