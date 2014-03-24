using DotsWithFriends.Models;
using DotsWithFriends.ViewModels;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DotsWithFriends.Hubs
{
	public class GameHub : BaseHub
	{
		public async Task GetAllGames( TokenViewModel Token )
		{
			try
			{
				var User = await this.VerifyToken( Token );
				if ( User != null )
				{
					//todo Return All Games to Client.
				}
				else
				{
					Clients.Caller.error( "Unauthorized Access." );
				}
			}
			catch(Exception error)
			{
				Clients.Caller.error("Exception Occurred: " + error.Message);
			}
		}
		public async Task MatchMakeGame( TokenViewModel Token )
		{
			try
			{
				var User = await this.VerifyToken( Token );
				if ( User != null )
				{
					//See if Any Games Are Available before making a new one
					var games = db.Games.Where( c => c.PrivateMatch == false && c.CanAddPlayer == true );
					Game game;
					if(games.Count() == 0)
					{
						//There are no available games.
						//Create New Game
						game = new Game();
						//Add Current User to Game
						game.AddPlayer( new Player( User.Profile ) );
						//Save Game
						db.Games.Add( game );
						await db.SaveChangesAsync();
						Clients.Caller.newGame(game);
					}
					else
					{
						//Add Current Player to oldest game.
						game = games.First();
						game.AddPlayer(new Player(User.Profile));
						//Save Changes
						await db.SaveChangesAsync();

						//Get a list of all connections for the Game
						ICollection<String> connections = new Collection<String>();
						//For Every Player:
						foreach( var player in game.Players)
						{
							//For Every Connection:
							foreach( var connection in player.Value.Profile.User.Connections)
							{
								//Add to ID Connection List
								connections.Add(connection.Id.ToString());
							}
						}
						//Notify All clients about new Player Joining.
						Clients.Clients(connections as IList<String>).playerJoined(game);
					}
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
		public async Task CreateNewCustomGame( TokenViewModel Token, GameViewModel Game )
		{
			try
			{
				var User = await this.VerifyToken( Token );
				if ( User != null )
				{
					//Create New Game
					Game game = new Game(Game);
					//Add Current User to Game
					game.AddPlayer(new Player(User.Profile));
					//Save Game
					db.Games.Add(game);
					await db.SaveChangesAsync();
					//Send Game back to client.
					Clients.Caller.newGame(game);
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
		public async Task AddPlayerToGame( TokenViewModel Token, GameViewModel Game, PlayerViewModel Player )
		{
			try
			{
				var User = await this.VerifyToken( Token );
				if ( User != null )
				{
					//todo Create a New Game
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
		public async Task RemovePlayerFromGame( TokenViewModel Token, GameViewModel Game, PlayerViewModel Player )
		{
			try
			{
				var User = await this.VerifyToken( Token );
				if ( User != null )
				{
					//todo Create a New Game
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
		public async Task PlayTurn( TokenViewModel Token, GameViewModel Game, TurnViewModel Turn )
		{
			try
			{
				var User = await this.VerifyToken( Token );
				if ( User != null )
				{
					//todo Create a New Game
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