using EventManagementsystem.Models;

namespace EventManagementSystem.IRepository
{
    public interface ILoginRepository
    {
        UserDetails GetLoginDetailByEmail(string email, string password);
        Boolean GetEmailValidate(string email);
        void SaveUserDetails(UserDetails userDetails);
    }
}
