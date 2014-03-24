using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotsWithFriends.Models
{
	public class Coordinate : DatabaseObject
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Coordinate()
			: base()
		{

		}
		public Coordinate(int X, int Y)
			: base()
		{
			this.X = X;
			this.Y = Y;
		}

		public override string ToString()
		{
			String output;

			output = "( " + X + " , " + Y + " )";

			return output;
		}
		/// <summary>
		/// Checks whether the two coordinates represent the same point.
		/// </summary>
		/// <param name="Coordinate">The Coordinate to compare.</param>
		/// <returns></returns>
		public override bool Equals( object obj )
		{
			Coordinate Coordinate = obj as Coordinate;
			if(Coordinate != null)
			{
				if ( this.X == Coordinate.X && this.Y == Coordinate.Y )
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
				return base.Equals(obj);
			}
		}
	}
}
