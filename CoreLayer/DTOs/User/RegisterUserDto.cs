namespace CoreLayer.DTOs.User
{
    public class RegisterUserDto
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public int UserRoleId { get; set; }
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public string Password { get; set; }
    }
}
