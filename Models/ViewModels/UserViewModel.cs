namespace RentACar_ip.Models
{
    public class UserViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public User NewUser { get; set; } = new User();
        public IEnumerable<Role> Roles { get; set; }
    }
}
