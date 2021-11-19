using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data
{
	public class DeviceDetails : DeviceBase
	{
		[Column("description")]
		public string Description { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		[Required] [Column("created_ts")]
		public DateTime CreatedTimestamp { get; set; }

		[Column("updated_ts")]
		public DateTime? UpdatedTimestamp { get; set; }
	}
}