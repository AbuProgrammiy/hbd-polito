using Microsoft.AspNetCore.Http;

namespace CQRSTemplate.Domain.Entities.Models.PrimaryModels
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GroupNumber { get; set; }
        public DateOnly BirthDate { get; set; }
        public string? InstagramUsername { get; set; }
        public string Wishes { get; set; }
        public string PhotoUrl { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
