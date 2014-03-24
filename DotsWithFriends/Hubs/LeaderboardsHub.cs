using DotsWithFriends.ViewModels;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DotsWithFriends.Hubs
{
	public class LeaderboardsHub : BaseHub
	{
		public async Task GetLeaderboards( TokenViewModel Token )
		{
			try
			{
				var User = await this.VerifyToken( Token );
				if ( User != null )
				{
					//todo Return Leaderboard Stats or something. This can be added later.
				}
				else
				{
					Clients.Caller.error( "Unauthorized Access." );
				}
			}
			catch ( Exception error )
			{
				Clients.Caller.error( "Exception Occurred: " + error.Message );
			}
		}
	}
}