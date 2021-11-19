using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data
{
	public class DeviceType
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Required] [Key] [Column("id")]
		public int Id { get; set; }

		[Required] [Column("name")]
		public string Name { get; set; }

		[Column("description")]
		public string Description { get; set; }
	}
}