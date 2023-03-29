namespace ThreaditAPI.Models.Requests
{
    public class PostThreadRequest
    {
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string Topic { get; set; } = "";
        public string OwnerId { get; set; } = "";
        public string SpoolId { get; set; } = "";
        public string ThreadType { get; set; } = "";
    }
}
