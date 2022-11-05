using EventoWeb.Data;

namespace EventoWeb.Models
{
    public class SqlUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public SqlUserRepository(ApplicationDbContext _db)
        {
            this._db = _db;
        }
        public User Add(User User)
        {
            _db.Users.Add(User);
            _db.SaveChanges();
            return User;
        }

        public User Delete(int id)
        {
            User User = _db.Users.Find(id);
            if (User != null)
            {
                _db.Users.Remove(User);
                _db.SaveChanges();
            }
            return User;
        }

        public User GetExistingUser(string email, string phone)
        {
            return _db.Users.FirstOrDefault(m => m.Email == email || m.PhoneNo == phone);
        }

        public User GetUser(string email, string password)
        {
            return _db.Users.FirstOrDefault(m => m.Email == email && m.Password == password);
        }

        public User GetUserById(int id)
        {
            return _db.Users.FirstOrDefault(m => m.UserId == id);
        }

        public User Update(User UserChanges)
        {
            var User = _db.Users.Attach(UserChanges);
            User.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _db.SaveChanges();
            return UserChanges;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _db.Users;
        }
    }
}
