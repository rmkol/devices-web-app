using API.Data;

namespace API.Core
{
	public class RequestContext : IRequestContext
	{
		public User User { get; set; }
	}
}