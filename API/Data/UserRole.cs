using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data
{
	public class UserRole
	{
		[Required] [Column("user_id")]
		public int UserId { get; set; }

		[Required] [Column("role_id")]
		public int RoleId { get; set; }
	}
}