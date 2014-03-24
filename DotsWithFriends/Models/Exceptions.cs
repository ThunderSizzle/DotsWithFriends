using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotsWithFriends.Models
{
	class InvalidTurnException : Exception
	{
		public InvalidTurnException( )
			: base( )
		{

		}
		public InvalidTurnException( String message )
			: base(message)
		{

		}

		public override string Message
		{
			get
			{
				return "The turn is invalid. Please check for more details: " + base.Message;
			}
		}
	}
	class InvalidAddPlayerException : Exception
	{
		public InvalidAddPlayerException( )
			: base( )
		{

		}
		public InvalidAddPlayerException(String message)
			: base(message)
		{

		}
		public override string Message
		{
			get
			{
				return "The player is invalid. Please check for more details: " + base.Message;
			}
		}
	}
	class InvalidLineException : Exception
	{
		public InvalidLineException( )
			: base( )
		{

		}
		public InvalidLineException( String message )
			: base(message)
		{

		}
		public override string Message
		{
			get
			{
				return "The player is invalid. Please check for more details: " + base.Message;
			}
		}
	}
}
