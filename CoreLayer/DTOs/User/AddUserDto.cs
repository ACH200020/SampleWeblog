namespace CoreLayer.DTOs.User
{
    public class AddUserDto
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public UserRole UserRole { get; set; }
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public string Password { get; set; }
    }
}
