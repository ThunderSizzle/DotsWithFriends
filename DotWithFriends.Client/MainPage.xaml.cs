using Microsoft.AspNet.SignalR.Client;
using Microsoft.Live;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace DotWithFriends.Client
{
	public partial class MainPage : PhoneApplicationPage
	{
		public IHubProxy AccountHub { get; set; }
		public Boolean IsSignedIn { get; set; }
		// Constructor
		public MainPage()
		{
			InitializeComponent();
			// Sample code to localize the ApplicationBar
			//BuildLocalizedApplicationBar();
			
		}

		protected override void OnNavigatedTo( NavigationEventArgs e )
		{
			this.IsSignedIn = false;
			this.AccountHub  = App.hubConnection.CreateHubProxy( "AccountHub" );
			base.OnNavigatedTo( e );
		}
		private void Button_Click( object sender, RoutedEventArgs e )
		{
			NavigationService.Navigate( new Uri( "/NewGame.xaml", UriKind.Relative ) );
		}

		// This section manages the requirement for users to be logged in. Upon logging in with Microsoft:
		// The data will be sent to our servers to register an account using OAuth / Rest.
		// This will allow for an eventual website to connect the log in of mobile devices.
		private async void SignIn_Click( object sender, RoutedEventArgs e )
		{
			this.AccountHub.On<string>( "error", text =>
			{
				if ( "Incorrect Token." == text || "Email Not Found"  == text)
				{
					this.IsSignedIn = false;
					App.Email = null;
					App.Token = null;
				}
				Debug.WriteLine(text);
			} );
			this.AccountHub.On<string>( "token", text =>
			{
				App.Token = text;
				Debug.WriteLine( "You are signed in! We'll just verify to be doubly correct." );
			} );
			this.AccountHub.On<string>( "tokenVerified", text =>
			{
				this.IsSignedIn = true;
				Debug.WriteLine( "You are verified!" );
			} );
			await App.hubConnection.Start();

			try
		    {
				if ( App.Email != null && App.Token != null )
				{
					await this.AccountHub.Invoke( "VerifyToken", new
					{
						Email = App.Email,
						Token = App.Token
					} );
				}
				else
				{
					LiveLoginResult initializeResult = await App.authClient.InitializeAsync();
					try
					{
						LiveLoginResult authResult = await App.authClient.LoginAsync( new List<string>() { "wl.signin", "wl.basic", "wl.emails" } );
						if ( authResult.Status == LiveConnectSessionStatus.Connected )
						{
							App.Session = authResult.Session;
							LiveConnectClient connect = new LiveConnectClient(App.Session);
							LiveOperationResult operationResult = await connect.GetAsync("me");
							dynamic result = operationResult.Result;
							if (result != null)
							{
								Debug.WriteLine("Result successful");
							}
							else
							{
								Debug.WriteLine("Error getting information.");
							}
							var shorterToken = ((authResult.Session.AccessToken as String).Substring(0, 128));
							await this.AccountHub.Invoke( "SignIn", new 
							{
								Email = result.emails.account,
								ExternalAccessToken = shorterToken,
								FirstName = result.first_name,
								Id= result.id,
								LastName= result.last_name
							} );
							App.Email = result.emails.account;
							await this.AccountHub.Invoke( "VerifyToken", new
							{
								Email = App.Email,
								Token = App.Token
							} );

							/*using (var client = new HttpClient())
							{
								client.BaseAddress = new Uri( "http://r4clucky14-001-site7.smarterasp.net/" );
								client.DefaultRequestHeaders.Accept.Clear();
								client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

								var httpClient = new HttpClient();

								var response = await client.PostAsJsonAsync( "api/Account/RegisterExternal",  );
								//var response = await client.PostAsJsonAsync( "api/Account/AddExternalLogin", login ).ContinueWith( c => c.Result.EnsureSuccessStatusCode() );
								if(response.IsSuccessStatusCode)
								{
									Debug.WriteLine("You are signed in!");
								}
							}*/
							App.Session = authResult.Session;
						}
						else
						{
							this.IsSignedIn = false;
						}
					}
					catch(LiveAuthException exception)
					{
						Debug.WriteLine("Error signing in: " + exception.Message);
					}
					catch (LiveConnectException exception)
					{
						Debug.WriteLine("Error calling API:  " + exception.Message);
					}
					catch (InvalidOperationException exception)
					{
						//Something went wrong with the SignalR pipeline. Restart the connection.
						App.hubConnection.Stop();
						this.SignIn_Click(sender, e);
					}
				}
			}
			catch (LiveAuthException exception)
			{
				Debug.WriteLine("Error initializing: " + exception.Message);
			}
		}

		// Sample code for building a localized ApplicationBar
		//private void BuildLocalizedApplicationBar()
		//{
		//    // Set the page's ApplicationBar to a new instance of ApplicationBar.
		//    ApplicationBar = new ApplicationBar();

		//    // Create a new button and set the text value to the localized string from AppResources.
		//    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
		//    appBarButton.Text = AppResources.AppBarButtonText;
		//    ApplicationBar.Buttons.Add(appBarButton);

		//    // Create a new menu item with the localized string from AppResources.
		//    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
		//    ApplicationBar.MenuItems.Add(appBarMenuItem);
		//}

	}
}