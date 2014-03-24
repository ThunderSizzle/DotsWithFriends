using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotsWithFriends.Models;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace DotsWithFriends.Tests.Models
{
	[TestClass]
	public class GameTest
	{

		[TestMethod]
		public void ConstructorTest()
		{
			Game Game = new Game();
			var width = 5;
			var height = 5;
			var expectedBoxes = ( width )*( height );


			Assert.IsNotNull(Game);

			Assert.AreEqual( width, Game.width );
			Assert.AreEqual( height, Game.height );
			Assert.AreEqual( expectedBoxes, Game.Boxes.Count );

		}
		[TestMethod]
		public void CustomConstructorTest()
		{
			var width = 10;
			var height = 5;
			var expectedBoxes = (width)*(height);
			Game Game = new Game( width, height );

			Assert.IsNotNull( Game );

			Assert.AreEqual( width, Game.width );
			Assert.AreEqual( height, Game.height );
			Assert.AreEqual( expectedBoxes, Game.Boxes.Count );

			var box = new Box( new Coordinate( 0, 0 ) );
			Assert.AreEqual(box, Game.Boxes.ElementAt( 0 ) );
		}
		[TestMethod]
		public void PlayGame()
		{
			var width = 2;
			var height = 2;
			var expectedBoxes = ( width )*( height );

			Game Game = new Game( width, height );
			var player1 = new Player();
			var player2 = new Player();

			player1.Profile = new Profile();
			player1.Color = Color.Blue.ToArgb();
			player2.Profile = new Profile();
			player2.Color = Color.Red.ToArgb();

			Game.AddPlayer( player1 );
			Game.AddPlayer( player2 );

			Assert.AreEqual(2, Game.Players.Count);

			Assert.AreEqual( 12, Game.TotalTurns );
			Assert.AreEqual( true, Game.GameStartable );
			Assert.AreEqual( false, Game.GameStarted );
			Assert.AreEqual( false, Game.GameFinished );

			Game.AddTurn( new Turn( new Line( new Coordinate( 0, 0 ), new Coordinate( 1, 0 ) ), player1 ) );
			Game.AddTurn( new Turn( new Line( new Coordinate( 0, 0 ), new Coordinate( 0, 1 ) ), player2 ) );
			Game.AddTurn( new Turn( new Line( new Coordinate( 1, 0 ), new Coordinate( 1, 1 ) ), player1 ) );
			Game.AddTurn( new Turn( new Line( new Coordinate( 1, 1 ), new Coordinate( 0, 1 ) ), player2 ) );
			

			int boxCount = 0;
			foreach(var box in Game.Boxes)
			{
				if(box.Completed())
				{
					boxCount++;
				}
			}


			Assert.AreEqual( 4, Game.Turns.Count );
			Assert.AreEqual( 1, boxCount );
			Assert.AreEqual( 1, Game.CurrentPlayer );
			Game.AddTurn( new Turn( new Line( new Coordinate( 1, 1 ), new Coordinate( 2, 1 ) ), player2 ) );
			Assert.AreEqual( 0, Game.CurrentPlayer );
					
			
		}
	}
}
