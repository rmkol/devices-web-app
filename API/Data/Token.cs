using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data
{
	public class Token
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Required] [Key] [Column("id")]
		public int Id { get; set; }

		[Required] [Column("token")]
		public string Value { get; set; }

		[ForeignKey("id")]
		[Required] [Column("user_id")]
		public int UserId { get; set; }

		[Required] [Column("ttl_sec")]
		public long TtlSec { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		[Required] [Column("created_ts")]
		public DateTime CreatedTimestamp { get; set; }

		[Column("updated_ts")]
		public DateTime? UpdatedTimestamp { get; set; }

		public bool IsExpired => DateTime.Now >= CreatedTimestamp.AddSeconds(TtlSec); // todo: move this logic to service?
	}
}