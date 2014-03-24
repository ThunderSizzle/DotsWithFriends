using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotsWithFriends.Models
{
	public class DatabaseObject
	{
		public Guid Id { get; set; }

		public DatabaseObject()
			: base()
		{
			this.Id = Guid.NewGuid();
		}
		public DatabaseObject(Guid Id)
			: base()
		{
			this.Id = Id;
		}
	}
}
