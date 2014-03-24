using DotsWithFriends.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Web;

namespace DotsWithFriends.Models
{
	public class Profile : DatabaseObject
	{
		public int DefaultColor { get; set; }
		public ICollection<Player> PlayerAccounts { get; set; }
		public int TotalScore
		{
			get
			{
				var totalscore = 0;
				foreach(var player in PlayerAccounts)
				{
					totalscore += player.Score;
				}
				return totalscore;
			}
		}
		public MyUser User { get; set; }

		public Profile()
			: base()
		{
			this.PlayerAccounts = new Collection<Player>();
			this.DefaultColor = Color.Aqua.ToArgb();
		}
		/// <summary>
		/// Allows changes to be made to a profile using a ViewModel. This is a form of white-listing and seperatation.
		/// </summary>
		/// <param name="Profile">The View Model to use to change the loaded Profile.</param>
		public void UpdateProfile(ProfileViewModel Profile)
		{
			this.DefaultColor = Profile.DefaultColor;
		}
	}
}