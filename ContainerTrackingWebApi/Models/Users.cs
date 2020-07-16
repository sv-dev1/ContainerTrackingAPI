using System;
using System.Collections.Generic;

namespace ContainerTrackingWebApi.Models
{
    public partial class Users
    {
        public Users()
        {
            ColumnsShow = new HashSet<ColumnsShow>();
            CompanyAlias = new HashSet<CompanyAlias>();
            ContainerApidata = new HashSet<ContainerApidata>();
            EmailLogs = new HashSet<EmailLogs>();
            NotificationsEmails = new HashSet<NotificationsEmails>();
            Payment = new HashSet<Payment>();
            ShipsgoContainer = new HashSet<ShipsgoContainer>();
            UserNotifications = new HashSet<UserNotifications>();
        }

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

        public virtual ICollection<ColumnsShow> ColumnsShow { get; set; }
        public virtual ICollection<CompanyAlias> CompanyAlias { get; set; }
        public virtual ICollection<ContainerApidata> ContainerApidata { get; set; }
        public virtual ICollection<EmailLogs> EmailLogs { get; set; }
        public virtual ICollection<NotificationsEmails> NotificationsEmails { get; set; }
        public virtual ICollection<Payment> Payment { get; set; }
        public virtual ICollection<ShipsgoContainer> ShipsgoContainer { get; set; }
        public virtual ICollection<UserNotifications> UserNotifications { get; set; }
    }
}
