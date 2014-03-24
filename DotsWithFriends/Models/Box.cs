using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotsWithFriends.Models
{
	public class Box : DatabaseObject
	{
		public Line North { get; private set; }
		public Line South { get; private set; }
		public Line East { get; private set; }
		public Line West { get; private set; }

		public Coordinate NorthWest { get; private set; }
		public Coordinate SouthWest { get; private set; }
		public Coordinate NorthEast { get; private set; }
		public Coordinate SouthEast { get; private set; }

		public Player Owner { get; set; }

		public Box()
			: base()
		{

		}
		public Box( Coordinate SouthWest, Player Owner = null )
		{
			this.SouthWest = SouthWest;
			this.NorthWest = new Coordinate( SouthWest.X, SouthWest.Y + 1 );
			this.SouthEast = new Coordinate(SouthWest.X + 1, SouthWest.Y);
			this.NorthEast = new Coordinate( SouthWest.X + 1, SouthWest.Y + 1 );

			this.North = new Line( NorthWest, NorthEast );
			this.South = new Line( SouthWest, SouthEast );
			this.East = new Line( SouthEast, NorthEast );
			this.West = new Line( SouthWest, NorthWest );

			this.Owner = Owner;
		}

		public override string ToString()
		{
			String output;

			output = "[ " + Owner + " ; North : " + North + " ; South: " + South + " ; East: " + East + " ; West: " + West + " ] ";

			return output;
		}

		public Boolean Completed()
		{
			if(this.North.Created && this.South.Created && this.East.Created && this.West.Created)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Checks whether the two boxes represent the same box.
		/// </summary>
		/// <param name="Coordinate">The Box to compare.</param>
		/// <returns></returns>
		public override bool Equals( object obj )
		{
			Box Box = obj as Box;
			if ( Box != null )
			{
				if ( this.North.Equals(Box.North) && this.South.Equals(Box.South) && this.East.Equals(Box.East) && Box.West.Equals(this.West))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return base.Equals( obj );
			}
		}

	}
}