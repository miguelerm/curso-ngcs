using System;

namespace WebCachedApplication.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string SSN { get; set; }
        public Gender Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string SomethingUnique { get; set; }
        public string CartId { get; set; }
        public string FullName { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
