using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DotsWithFriends.Models
{
	public class Player : DatabaseObject
	{
		public Profile Profile { get; set; }
		public int Color { get; set; }
		public int Score { get; set; }

		public Player()
			: base()
		{
			this.Score = 0;
		}
		public Player(Profile Profile)
			: base()
		{
			this.Color = Profile.DefaultColor;
			this.Score = 0;
		}
	}
}
