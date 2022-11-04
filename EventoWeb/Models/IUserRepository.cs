namespace EventoWeb.Models
{
    public interface IUserRepository
    {
        User GetUserById(int id);
        User GetUser(string email, string password);
        User GetExistingUser(string email);
        User Add(User User);
        User Update(User UserChanges);
        User Delete(int id);
        IEnumerable<User> GetAllUsers();
    }
}
