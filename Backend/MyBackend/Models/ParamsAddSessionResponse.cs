namespace MyBackend.Models
{
    public class ParamsAddSessionResponse
    {
        public bool IsTitleUnique { get; set; } = true;

        public bool IsSystemNew { get; set; } = false;

        //description czy nie za długie
    }
}
