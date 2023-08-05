using EventManagementsystem.Models;

namespace EventManagementSystem.IService
{
    public interface ILoginService
    {
        UserDetails GetLoginDetailByEmail(string email, string password);
        Boolean GetEmailValidate(string email);
        void SaveUserDetails(UserDetails userDetails);
        
    }
}
