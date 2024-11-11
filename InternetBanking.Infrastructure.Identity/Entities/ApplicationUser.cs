using Microsoft.AspNetCore.Identity;


namespace InternetBanking.Infrastructure.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; } = true;
        public string UserType { get; set; }
        public string Identification { get; set; } //Cedula

    }
}
