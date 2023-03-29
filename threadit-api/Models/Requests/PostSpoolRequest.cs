namespace ThreaditAPI.Models.Requests
{
	public class PostSpoolRequest
	{
		public string Name { get; set; } = "";
		public string OwnerId { get; set; } = "";
		public List<string> Interests { get; set; } = new List<string> { };
		public List<string> Moderators { get; set; } = new List<string> { };
	}
}
