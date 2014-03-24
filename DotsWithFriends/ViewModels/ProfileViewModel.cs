using DotsWithFriends.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Web;

namespace DotsWithFriends.ViewModels
{
	public class ProfileViewModel : BaseViewModel
	{
		public int DefaultColor { get; set; }
		public ICollection<PlayerViewModel> PlayersViewModel { get; set; }
		public int TotalScore
		{
			get
			{
				var totalscore = 0;
				foreach ( var player in PlayersViewModel )
				{
					totalscore += player.Score;
				}
				return totalscore;
			}
		}
		public ProfileViewModel()
			: base()
		{
			this.DefaultColor = 0;
		}
		public ProfileViewModel(Profile Profile)
			: base(Profile.Id)
		{
			this.DefaultColor = Profile.DefaultColor;
			this.PlayersViewModel = new Collection<PlayerViewModel>();
			foreach(var player in Profile.PlayerAccounts)
			{
				this.PlayersViewModel.Add(new PlayerViewModel(player));
			}
		}
	}
}