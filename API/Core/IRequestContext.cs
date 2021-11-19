using API.Data;

namespace API.Core
{
	public interface IRequestContext
	{
		User User { get; set; }
	}
}