namespace PortalJob.Payload.Request
{
    public class RegisterRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Avatar { get; set; }
        public string Occupation { get; set; }
        public string Experience { get; set; }
    }
}
