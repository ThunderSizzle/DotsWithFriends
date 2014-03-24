using System;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Diagnostics;
using DotsWithFriends.Models;

namespace DotsWithFriends.Tests.Hubs
{
	[TestClass]
	public class HubTest
	{
		public static HubConnection hubConnection = new HubConnection( "http://localhost:49702" );
		public IHubProxy AccountHub  = hubConnection.CreateHubProxy( "AccountHub" );

		[TestMethod]
		public async Task ConnectionTest()
		{
			this.AccountHub.On<string>( "weAreHere", text => Assert.AreEqual( "Coonnected", text ) );  
			await hubConnection.Start();
			hubConnection.Stop();
		}
		[TestMethod]
		public async Task SignInandVerifyTest()
		{
			String Token = "";
			this.AccountHub.On<string>( "error", text =>
			{
				Assert.AreEqual( "", text );
			} );
			this.AccountHub.On<string>( "token", text =>
			{
				Assert.AreEqual( "7463e65c-aacf-49ad-95d8-d0895e6f6376", text );
				Token = text;
			} ); 
			this.AccountHub.On<string>( "tokenVerified", text =>
			{
				Assert.AreEqual( "Token Verified", text );
			});
			await hubConnection.Start();



			await this.AccountHub.Invoke( "SignIn", new
			{
				Email = "james0308@outlook.com",
				ExternalAccessToken ="SDALHFIOL213JKLLHFA",
				FirstName = "James",
				Id="1000",
				LastName="Haug"
			} );
			await this.AccountHub.Invoke( "VerifyToken", new { Email= "james0308@outlook.com", Token= Token } );
			hubConnection.Stop();

		}
	}
}
