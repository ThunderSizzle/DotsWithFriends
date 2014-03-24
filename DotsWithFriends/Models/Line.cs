using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotsWithFriends.Models
{
	public class Line : DatabaseObject
	{
		public Coordinate From { get; set; }
		public Coordinate To { get; set; }
		public Boolean Created { get; set; }
		public Player Player { get; set; }
		public String Direction
		{
			get
			{
				if(From.X == To.X)
				{
					return "Verticle";
				}
				else
				{
					return "Horizontal";
				}
			}
		}
		public Line()
			: base()
		{

		}
		public Line(Coordinate From, Coordinate To)
			: base()
		{
			var difference = Math.Abs( From.X - To.X ) + Math.Abs( From.Y - To.Y );
			if ( difference == 1 )
			{
				this.From = From;
				this.To = To;
			}
			else
				throw new InvalidLineException();
		}
		public Line( int fromX, int fromY, int toX, int toY)
			: base()
		{
			if( Math.Abs(fromX - toX) + Math.Abs(fromY - toY) < 1)
			{
				this.From = new Coordinate( fromX, fromY );
				this.To = new Coordinate( toX, toY );
			}
			else
				throw new InvalidLineException();
		}

		public override string ToString()
		{
			String output;

			output = "{ " + From + " --- " + To + " } ";

			return output;
		}
		/// <summary>
		/// Checks whether the line is equal to the current line. This only checks to see if the coordinates are the same (or reversed).
		/// A (2,1) -> (2,2) line will equal a (2,2) -> (2,1) line.
		/// </summary>
		/// <param name="Line"></param>
		/// <returns></returns>
		public override bool Equals( object obj )
		{
			Line Line = obj as Line;
			if(Line != null)
			{
				if ( Line.From.Equals(this.From) && Line.To.Equals(this.To) )
				{
					return true;
				}
				else if ( Line.From.Equals(this.To) && Line.To.Equals(this.From) )
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
