namespace ThreaditAPI.Models
{
	public class Interest
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public string Name { get; set; } = "";
		public int SpoolCount { get; set; } = 0;

		public Interest()
		{

		}
	}
}
