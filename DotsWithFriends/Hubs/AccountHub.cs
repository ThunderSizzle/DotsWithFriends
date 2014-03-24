using DotsWithFriends.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.ModelBinding;
using System.Data.Entity;
using DotsWithFriends.ViewModels;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace DotsWithFriends.Hubs
{
	public class AccountHub : Hub
	{
		public static Context db = new Context();
		public static UserManager<MyUser> UserManager = new UserManager<MyUser>( new UserStore<MyUser>( db ) );
		public ModelStateDictionary ModelState = new ModelStateDictionary();


		public async Task SignIn(RegisterExternalBindingModel model)
		{
			//Check Model State
			if(!ModelState.IsValid)
			{
				//If Model State is not Valid, call error on Caller.
				Clients.Caller.error("Invalid Model: " + ModelState);
			}
			else
			{
				//Else, attempt logging the user
				try
				{
					//Find the User using the Log In Information
					var user = await UserManager.FindAsync( new UserLoginInfo( "microsoft", model.ExternalAccessToken ) );
					//If User and Login exists, return the Token.
					if(user != null)
					{
						Clients.Caller.token(user.Id);
					}
					else
					{
						//Else, the User will be created.
						user = new MyUser
						{
							//UserName just needs to be unique. We really want an email address.
							UserName = model.Id,
							Email = model.Email,
							FirstName = model.FirstName,
							LastName = model.LastName,
							Profile = new Profile()
						};
						//The External Login is Added to the user
						user.Logins.Add(new IdentityUserLogin
						{
							LoginProvider = "microsoft",
							ProviderKey = model.ExternalAccessToken
						});
						//Save the new User to the Database.
						IdentityResult result = await UserManager.CreateAsync( user );
						//Check to see if User was successfully saved
						if(result.Succeeded)
						{
							//If User was saved, return the Token.
							Clients.Caller.token( user.Id );
						}
						else
						{
							//Else, something went wrong with registering user. Abort operation and call error method on Caller.
							Clients.Caller.error("Registration Failed");
						}
					}
				}
				//For Entity Validation Exception Errors, output the validation errors.
				catch ( DbEntityValidationException error )
				{
					string output = "Exception Occurred: " + error.Message + ":";
					//Return all error messages to error method on Caller.
					foreach ( var validationErrors in error.EntityValidationErrors )
					{
						foreach ( var validationError in validationErrors.ValidationErrors )
						{
							output += "Property: " + validationError.PropertyName + ",  Error: " + validationError.ErrorMessage + " ------";
						}
					}
					Clients.Caller.error( output );
				}
				//Catch all other Exceptions to avoid silent failure
				catch ( Exception error )
				{
					string output = "Exception Occurred: " + error.Message + " : ";
					while(error.InnerException != null)
					{
						output += "Inner Exception: " + error.InnerException.Message;
						error = error.InnerException;
					}
					//Return all error messages to error method on Caller.
					Clients.Caller.error( output );
				}
			}
		}
		public async Task<Boolean> VerifyToken(TokenViewModel Token)
		{
			//Check Model State
			if(!ModelState.IsValid)
			{
				//If Model State is not Valid, call error on Caller.
				Clients.Caller.error("Invalid Model: " + ModelState);
				return false;
			}
			else
			{
				//Else, attempt to match the Email with the Token.
				try
				{
					//Try to find user with specified Email Address
					var user = await db.Users.FirstOrDefaultAsync( c => c.Email == Token.Email );
					//Check to see if query was succesful
					if(user != null)
					{
						//User properly found. Attempt to verify the token.
						if ( user.Id != Token.Token )
						{
							//Token does not match our system
							Clients.Caller.error( "Incorrect Token." );
							return false;
						}
						else
						{
							//Token is authenitcated
							Clients.Caller.tokenVerified("Token Verified");
							return true;
						}
					}
					else
					{
						//Else, the Email does not exist in our system.
						Clients.Caller.error("Email Not Found");
						return false;
					}
				}
				//Catch all Exceptions to avoid silent failure
				catch ( Exception error )
				{
					//Return all error messages to error method on Caller.
					Clients.Caller.error( "Exception Occurred: " + error.Message );
					return false;
				}
			}			
		}
	}
}