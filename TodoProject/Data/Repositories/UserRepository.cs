using TodoProject.Data.Interfaces; 
using TodoProject.Entities;


namespace TodoProject.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public User? GetById(Guid id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public User? GetByUsername(string username)
        {
            return _context.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());
        }
        public User? GetUserByCredentials(string username, string hashedPassword)
        {
            var user = _context.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());

            if (user == null || user.Password != hashedPassword || user.Locked)
            {
                return null;
            }

            return user;
        }

        public string? GetFullName(Guid id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id)?.FullName;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public void Save()
        {
            _context.SaveChanges();
        }



    }
}
