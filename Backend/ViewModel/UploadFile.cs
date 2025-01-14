namespace Backend.ViewModel
{
    public class UploadFile
    {
        public int id { get; set; }
        public IFormFile File { get; set; } = null!;
        public string command { get; set; } = string.Empty;
    }
}
