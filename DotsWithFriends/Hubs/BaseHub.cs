using DotsWithFriends.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.Web.Http.ModelBinding;
using DotsWithFriends.ViewModels;

namespace DotsWithFriends.Hubs
{
	public abstract class BaseHub : Hub
	{
		public static Context db = new Context();
		public ModelStateDictionary ModelState = new ModelStateDictionary();

		public override Task OnConnected()
		{
			return base.OnConnected();
		}
		/// <summary>
		/// Verifies the Token matches the Email Address given. All Connections must 
		/// </summary>
		/// <param name="Token">User's Email and Token</param>
		/// <returns>The User that the User and Token apply to. If a User can not be identified properly, null is returned.</returns>
		public async Task<MyUser> VerifyToken( TokenViewModel Token )
		{
			//Check Model State
			if ( !ModelState.IsValid )
			{
				//If Model State is not Valid, call error on Caller.
				Clients.Caller.error( "Invalid Model: " + ModelState );
				return null;
			}
			else
			{
				//Else, attempt to match the Email with the Token.
				try
				{
					//Try to find user with specified Email Address
					var user = await db.Users.FirstOrDefaultAsync( c => c.Email == Token.Email );
					//Check to see if query was succesful
					if ( user != null )
					{
						//User properly found. Attempt to verify the token.
						if ( user.Id != Token.Token )
						{
							//Token does not match our system
							Clients.Caller.error( "Incorrect Token." );
							return null;
						}
						else
						{
							//Token is authenitcated
							Clients.Caller.tokenVerified( "Token Verified" );
							return user;
						}
					}
					else
					{
						//Else, the Email does not exist in our system.
						Clients.Caller.error( "Email Not Found" );
						return null;
					}
				}
				//Catch all Exceptions to avoid silent failure
				catch ( Exception error )
				{
					//Return all error messages to error method on Caller.
					Clients.Caller.error( "Exception Occurred: " + error.Message );
					return null;
				}
			}
		}

	}
}