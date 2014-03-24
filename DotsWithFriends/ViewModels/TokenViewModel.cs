using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DotsWithFriends.ViewModels
{
	public class TokenViewModel
	{
		[Required]
		public String Email { get; set; }
		[Required]
		public String Token { get; set; }
	}
}