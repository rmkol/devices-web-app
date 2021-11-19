using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Data
{
	public class User
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Required] [Key] [Column("id")]
		public int Id { get; set; }

		[Required] [Column("email_address")]
		public string EmailAddress { get; set; }
		
		[Required] [Column("password")]
		public string Password { get; set; }

		[Required] [Column("first_name")]
		public string FirstName { get; set; }

		[Required] [Column("last_name")]
		public string LastName { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		[Required] [Column("created_ts")]
		public DateTime CreatedTimestamp { get; set; }

		[Column("updated_ts")]
		public DateTime? UpdatedTimestamp { get; set; }
	}
}