using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotsWithFriends.ViewModels
{
	public class GameViewModel
	{
		public ICollection<TurnViewModel> TurnsViewModel { get; set; }
		public ICollection<BoxViewModel> BoxesViewModel { get; set; }
		public int width { get; set; }
		public int height { get; set; }
		public int CurrentPlayer { get; set; }
		public int TotalTurns
		{
			get
			{
				return ( ( 2 * this.width * this.height ) + ( this.height ) + ( this.width ) );
			}
		}
		public IDictionary<int, PlayerViewModel> PlayersViewModel { get; set; }
		public Boolean GameFinished
		{
			get
			{
				if ( this.TotalTurns == this.TurnsViewModel.Count )
					return true;
				else
					return false;
			}
		}
		public Boolean GameStarted
		{
			get
			{
				if ( this.TurnsViewModel.Count > 0 )
					return true;
				else
					return false;
			}
		}
		public Boolean GameStartable
		{
			get
			{
				if ( this.PlayersViewModel.Count > 1 )
					return true;
				else
					return false;
			}
		}
	}
}