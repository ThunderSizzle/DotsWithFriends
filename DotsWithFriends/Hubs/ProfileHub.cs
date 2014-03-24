using DotsWithFriends.ViewModels;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DotsWithFriends.Hubs
{
	public class ProfileHub : BaseHub
	{
		public async Task GetProfile( TokenViewModel Token )
		{
			try
			{
				var User = await this.VerifyToken( Token );
				if ( User != null )
				{
					//Return Profile that belongs to User
					var profile = new ProfileViewModel(User.Profile);
					Clients.Caller.Profile(profile);
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
		public async Task UpdateProfile( TokenViewModel Token, ProfileViewModel Profile )
		{
			try
			{
				var User = await this.VerifyToken( Token );
				if ( User != null )
				{
					//Update Profile
					User.Profile.UpdateProfile(Profile);
					await db.SaveChangesAsync();
					//Return Profile that belongs to User
					var profile = new ProfileViewModel(User.Profile);
					Clients.Caller.Profile(profile);
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