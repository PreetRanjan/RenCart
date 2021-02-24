namespace RenCart.API.Models
{
    public class AppUserRole
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}