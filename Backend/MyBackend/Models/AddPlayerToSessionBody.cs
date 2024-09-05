namespace MyBackend.Models
{
    public class AddPlayerToSessionBody
    {
        public string SessionTitle { get; set; } = "";
        public string PlayerName { get; set; } = "";
    }
}
