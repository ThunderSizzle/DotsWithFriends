using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotsWithFriends.Models
{
	public class Turn : DatabaseObject
	{
		public Line Line { get; set; }
		public Player Player { get; set; }


		public Turn()
			: base()
		{

		}

		public Turn(Line Line, Player Player)
			: base()
		{
			this.Line = Line;
			this.Player = Player;
		}

		public override bool Equals( object obj )
		{
			Turn Turn = obj as Turn;
			if(Turn != null)
			{
				if(this.Line.Equals(Turn.Line) && (this.Player.Equals(Turn.Player)))
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
