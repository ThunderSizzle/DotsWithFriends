using DotsWithFriends.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotsWithFriends.ViewModels
{
	public class PlayerViewModel
	{
		public ProfileViewModel Profile { get; set; }
		public int Color { get; set; }
		public int Score { get; set; }

		public PlayerViewModel()
			: base()
		{

		}
		public PlayerViewModel(Player Player)
			: base()
		{
			this.Profile = new ProfileViewModel(Player.Profile);
			this.Color = Player.Color;
			this.Score = Player.Score;
		}
	}
}