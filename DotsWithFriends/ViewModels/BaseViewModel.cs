using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotsWithFriends.ViewModels
{
	public class BaseViewModel
	{
		public Guid Id { get; set; }
		public BaseViewModel( )
			: base()
		{
			this.Id = Guid.Empty;
		}
		public BaseViewModel(Guid Id)
			: base()
		{
			this.Id = Id;
		}
	}

}