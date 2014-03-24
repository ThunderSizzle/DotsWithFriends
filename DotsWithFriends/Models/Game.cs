using DotsWithFriends.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace DotsWithFriends.Models
{
	public class Game : DatabaseObject
	{
		/// <summary>
		/// The Players involved in the game.
		/// </summary>
		public IDictionary<int, Player> Players { get; private set; }
		/// <summary>
		/// The width of the game board, in boxes (Add 1 for width in dots).
		/// </summary>
		public int width { get; private set; }
		/// <summary>
		/// The height of the game board, in boxes (Add 1 for height in dots).
		/// </summary>
		public int height { get; private set; }
		public int CurrentPlayer { get; private set; }
		public ICollection<Turn> Turns { get; private set; }
		public ICollection<Box> Boxes { get; private set; }

		public int TotalTurns
		{
			get
			{
				return (( 2 * this.width * this.height ) + ( this.height ) + ( this.width ));
			}
		}
		public Boolean GameFinished
		{
			get
			{
				if ( this.TotalTurns == this.Turns.Count )
					return true;
				else
					return false;
			} 
		}
		public Boolean GameStarted
		{
			get
			{
				if ( this.Turns.Count > 0 )
					return true;
				else
					return false;
			}
		}
		public Boolean GameStartable
		{
			get
			{
				if ( this.Players.Count <= this.MaximumPlayers && this.Players.Count >= this.MinimumPlayers )
					return true;
				else
					return false;
			}
		}
		public Boolean CanAddPlayer
		{
			get
			{
				if ( this.Players.Count < this.MaximumPlayers && this.GameStarted == false )
					return true;
				else
					return false;
			}
		}

		//Properties for Match-Making
		public int MinimumPlayers { get; set; }
		public int MaximumPlayers { get; set; }
		/// <summary>
		/// If true, Match-Making will avoid this game.
		/// </summary>
		public Boolean PrivateMatch { get; set; }


		public Game()
			: this(5, 5)
		{

		}
		public Game(int width, int height)
			: base()
		{
			this.width = width;
			this.height = height;
			this.Boxes = new Collection<Box>();
			this.Turns = new Collection<Turn>();
			this.Players = new Dictionary<int, Player>();
			this.MinimumPlayers = 2;
			this.MaximumPlayers = 2;
			this.GenerateBoxList();
		}
		public Game( GameViewModel Game )
			: base()
		{
			//Use width and height from GameViewModel
			this.width = Game.width;
			this.height = Game.height;
			//Boxes and Turns should be empty in a new game. Ignore them.
			this.Boxes = new Collection<Box>();
			this.Turns = new Collection<Turn>();
			this.Players = new Dictionary<int, Player>();
			this.GenerateBoxList();
		}
		/// <summary>
		/// Helper Method to populate Boxes
		/// </summary>
		private void GenerateBoxList()
		{
			//For every row
			for ( int x = 0; x < width; x++)
			{
				//And for every column in that row
				for (int y = 0; y < height; y++)
				{
					//Add the box
					this.Boxes.Add(new Box(new Coordinate(x, y)));
					//Therefore, the grid will look like:
					//	20	21	22	23	24
					//	15	16	17	18	19
					//	10	11	12	13	14
					//	5	6	7	8	9
					//	0	1	2	3	4
					// For a total of 25 boxes (0 -> 24) if the grid was 5x5 (boxes) or 6x6 (points).
				}
			}
		}

		public void AddPlayer(Player Player)
		{
			if(!GameStarted && this.Players.Count < this.MaximumPlayers)
			{
				foreach ( var player in this.Players )
				{
					if ( player.Value.Profile.Id.Equals( Player.Profile.Id ) )
					{
						throw new InvalidAddPlayerException( "Player is already in game!" );
					}
					if ( player.Value.Color.Equals( Player.Color ) )
					{
						throw new InvalidAddPlayerException( "Please select another color!" );
					}
				}
				this.Players.Add( this.Players.Count, Player );
			}
			else
			{
				throw new InvalidAddPlayerException("Game has already started, or is full.");
			}
		}

		/// <summary>
		/// Method to Add a Turn to the Game.
		/// </summary>
		/// <param name="Turn">The Turn to Play</param>
		public void AddTurn(Turn Turn)
		{
			//Grab who the current player ought to be from the Dictionary.
			var currentPlayer = Players.FirstOrDefault( c => c.Key == this.CurrentPlayer );
			//Verify the game is started/startable, and the gmae is not finished. Also, verify the player submitting the turn is correct.
			if(!GameFinished && (GameStarted || GameStartable) && currentPlayer.Value.Id.Equals(Turn.Player.Id)  )
			{
				//Add the Turn to the list.
				this.Turns.Add( Turn );
				//Draw the lines and score the boxes.
				var score = this.ScoreBoxes( Turn );
				//If the player did not score, then the turn moves to the next player.
				if ( score == 0 )
				{
					//If the current player is the last player in the list, return to the first player
					if(this.CurrentPlayer == this.Players.Count - 1 )
					{
						this.CurrentPlayer = this.Players.First().Key;
					}
					//Else, proceed to the next Player in the list.
					else
					{
						this.CurrentPlayer++;
					}
				}
				//Otherwise, the player goes again, and we save his score to his player account.
				else
				{
					currentPlayer.Value.Score += score;
				}
			}
			else
				throw new InvalidTurnException("It is not your turn, or the game has not yet started, or the game is finished!");			
		}
		/// <summary>
		/// Method to score Boxes that a Turn may have created.
		/// </summary>
		/// <param name="Turn">The Turn completed</param>
		/// <returns>Returns the number of boxes created.</returns>
		private int ScoreBoxes(Turn Turn)
		{
			int score = 0;
			var x = 0;
			var y = 0;
			var b1 = 0;
			var b2 = 0;
			//Check if Line is Verticle or Horizontal
			if(Turn.Line.Direction == "Verticle")
			{
				//If Line is Verticle, the X value is static.
				x = Turn.Line.To.X;
				//Find the lowest Y value
				if(Turn.Line.To.Y > Turn.Line.From.Y)
				{
					y = Turn.Line.From.Y;
				}
				else
				{
					y = Turn.Line.To.Y;
				}
				//Calcaluate the Position for the boxes on either side of the line.
				b1 = ( y * height ) + x;
				b2 = ( y * height ) + (x - 1);

				//Retreive box1 from list.
				var box1 = this.Boxes.ElementAtOrDefault( b1 );
				//If box is out of bounds, box1 will be null
				if ( box1 != null )
				{
					//A verticle line will be the west line of the first box.
					if(box1.West.Created)
					{
						//Line was already created. Throw an exception.
						throw new InvalidTurnException( "Line Has already been created." );
					}
					else
					{
						//Assign the Line to the Player
						box1.West.Created = true;
						box1.West.Player = Turn.Player;
						//Check to see if the new line creates a box
						if ( box1.Completed() )
						{
							//If so, set the owner to the current Player and increment score.
							box1.Owner = Turn.Player;
							score++;
						}
						//Otherwise, do nothing.
						//Move on to box 2.
					}
				}
				//Retreive box2 from list.
				var box2 = this.Boxes.ElementAtOrDefault( b2 );
				//If box is out of bounds, box2 will be null
				if ( box2 != null )
				{
					//A verticle line will be the east line of the first box.
					if ( box2.East.Created )
					{
						//Line was already created. Throw an exception.
						throw new InvalidTurnException( "Line Has already been created." );
					}
					else
					{
						//Assign the Line to the Player
						box2.East.Created = true;
						box2.East.Player = Turn.Player;
						//Check to see if the new line creates a box
						if ( box2.Completed() )
						{
							//If so, set the owner to the current Player and increment score.
							box2.Owner = Turn.Player;
							score++;
						}
						//Otherwise, do nothing.
						//Both boxes have been examined. The turn has been successfully added.
					}
				}	

			}
			else
			{
				//If Line is Horizontal, the Y value is static.
				y = Turn.Line.To.Y;
				//Find the lowest X value.
				if(Turn.Line.To.X > Turn.Line.From.X)
				{
					x = Turn.Line.From.X;
				}
				else
				{
					x = Turn.Line.To.X;
				}
				//Calcaluate the Position for the boxes on either side of the line.
				b1 = ( y * height ) + x;
				b2 = ( (y - 1) * height ) + x;

				//Retreive box1 from list.
				var box1 = this.Boxes.ElementAtOrDefault( b1 );
				//If box is out of bounds, box1 will be null
				if ( box1 != null )
				{
					//A horizontal line will be the south line of the first box.
					if ( box1.South.Created )
					{
						//Line was already created. Throw an exception.
						throw new InvalidTurnException( "Line Has already been created." );
					}
					else
					{
						//Assign the Line to the Player
						box1.South.Created = true;
						box1.South.Player = Turn.Player;
						//Check to see if the new line creates a box
						if ( box1.Completed() )
						{
							//If so, set the owner to the current Player and increment score.
							box1.Owner = Turn.Player;
							score++;
						}
						//Otherwise, do nothing.
						//Move on to box 2.
					}
				}
				//Retreive box2 from list.
				var box2 = this.Boxes.ElementAtOrDefault( b2 );
				//If box is out of bounds, box2 will be null
				if ( box2 != null )
				{
					//A horizontal line will be the north line of the first box.
					if ( box2.North.Created )
					{
						//Line was already created. Throw an exception.
						throw new InvalidTurnException( "Line Has already been created." );
					}
					else
					{
						//Assign the Line to the Player
						box2.North.Created = true;
						box2.North.Player = Turn.Player;
						//Check to see if the new line creates a box
						if ( box2.Completed() )
						{
							//If so, set the owner to the current Player and increment score.
							box2.Owner = Turn.Player;
							score++;
						}
						//Otherwise, do nothing.
						//Both boxes have been examined. The turn has been successfully added.
					}
				}	
			}
			return score;
		/*
			//Go through each box on the grid.
			foreach(var box in Boxes)
			{
				//If Current Box is not completed already, check to see if this turn completed it.
				if(!box.Completed())
				{
					//Check if the line from the Turn matches the Northern Line for the box.
					if(box.North.Equals(Turn.Line))
					{
						//Check if Line has Already Been Created (A line touches up to two boxes directly)
						if(!box.North.Created)
						{
							//Line has not been created. Create line, attach it to the player, and check if the line completes the box.
							box.North.Created = true;
							box.North.Player = Turn.Player;
							if(box.Completed())
							{
								//Line completes 
								box.Owner = Turn.Player;
								score++;
							}
						}
						else
						{
							throw new InvalidTurnException("Line Has already been created.");
						}
					}
					else if(box.South.Equals(Turn.Line))
					{
						if ( !box.South.Created )
						{
							box.South.Created = true;
							box.South.Player = Turn.Player;
							if ( box.Completed() )
							{
								box.Owner = Turn.Player;
								score++;
							}
						}
						else
						{
							throw new InvalidTurnException( "Line Has already been created." );
						}
					}
					else if(box.West.Equals(Turn.Line))
					{
						if ( !box.West.Created )
						{
							box.West.Created = true;
							box.West.Player = Turn.Player;
							if ( box.Completed() )
							{
								box.Owner = Turn.Player;
								score++;
							}
						}
						else
						{
							throw new InvalidTurnException( "Line Has already been created." );
						}
					}
					else if(box.East.Equals(Turn.Line))
					{
						if ( !box.East.Created )
						{
							box.East.Created = true;
							box.East.Player = Turn.Player;
							if ( box.Completed() )
							{
								box.Owner = Turn.Player;
								score++;
							}
						}
						else
						{
							throw new InvalidTurnException( "Line Has already been created." );
						}
					}
				}
				if ( score == 2 )
				{
					break;
				}
			}
			return score;
		 */
		}
	}
}