using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data
{
	public abstract class DeviceBase
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Required] [Key] [Column("id")]
		public int Id { get; set; }

		[Required] [Column("name")]
		public string Name { get; set; }

		[ForeignKey("id")]
		[Required] [Column("type_id")]
		public int TypeId { get; set; }

		[Required] [Column("status")]
		public DeviceStatus Status { get; set; }
	}
}