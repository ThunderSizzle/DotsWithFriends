using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;

namespace DotsWithFriends.Tests.Controllers
{
	[TestClass]
	public class AccountControllerTest
	{
	/*
		public HttpClient client { get; set; }

		public AccountControllerTest()
		{
			this.client =  new HttpClient();
			this.client.BaseAddress = new Uri("http://localhost:49702/");
			this.client.DefaultRequestHeaders.Accept.Clear();
			this.client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
		}

		[TestMethod]
		public async Task SignIn()
		{
			HttpResponseMessage response = await client.PostAsJsonAsync( "api/Account/LogInExternal", new { ExternalAccessToken = "SDALHFIOL213JKLLHFA", Email = "james0308@outlook.com", Id = "1000", FirstName = "James", LastName = "Haug" } );
			Assert.IsTrue(response.IsSuccessStatusCode);
			var token = await response.Content.ReadAsAsync<String>();

			this.client.DefaultRequestHeaders.Add( "Authorization-Token", token );
			response = await client.GetAsync("api/Account/UserInfo");
			var userInfo = await response.Content.ReadAsAsync<String>();
			Assert.AreEqual("1000", userInfo);
		}

		[TestMethod]
		public void SignOut()
		{
		}
	 * */
	}
}
