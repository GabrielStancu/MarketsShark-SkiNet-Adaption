namespace API.Dtos
{
    //the user data to be sent during database querrying 
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}