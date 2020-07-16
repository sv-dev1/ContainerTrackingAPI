using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContainerTrackingWebApi.ViewModel
{
    public class Users
    {
        public int Id { get; set; }
        public int InvitedId { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string ZipCode { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string CvrNo { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string ProfilePic { get; set; }
        public int UserRole { get; set; }
        public short Status { get; set; }
        public short SignupStatus { get; set; }
        public int TotalContainer { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string PasswordNew { get; set; }
    }
}
