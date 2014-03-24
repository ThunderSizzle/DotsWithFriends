using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
namespace DotWithFriends.Client
{
	public interface IDispatcher
	{
		void Dispatch( Action action );
	}

	public interface IConnectedDataProvider
	{
		Task StartAsync();
		void Stop();
	}

	public class SignalRDataProvider : IConnectedDataProvider
	{
		private HubConnection _connection;
		private IHubProxy _hubProxy;
		
		private const string EndpointAddress = "http://r4clucky14-001-site7.smarterasp.net/";
		private const string AccountHub = "Account";
		private const string GameHub = "Game";
		private const string ProfileHub = "Profile";



		private readonly IMessenger _messenger;
		private readonly IDispatcher _dispatcher;

		public SignalRDataProvider( IMessenger messenger, IDispatcher dispatcher )
		{
			_messenger = messenger;
			_dispatcher = dispatcher;
			_connection = new HubConnection( EndpointAddress );
			_hubProxy = _connection.CreateHubProxy( StockHubName );
			_hubProxy.On<Quote>( QuoteUpdateName, p => _dispatcher
					.Dispatch( () => UpdateQuote( p ) ) );

			_connection.StateChanged += _connection_StateChanged;
		}

		void _connection_StateChanged( StateChange stateChange )
		{
			ConnectionState oldState = ConnectionStateConverter
				.ToConnectionState( stateChange.OldState );
			ConnectionState newState = ConnectionStateConverter
				.ToConnectionState( stateChange.NewState );

			var msg = new ConnectionStateChangedMessage()
			{
				NewState = newState,
				OldState = oldState,
			};

			_dispatcher.Dispatch( () => _messenger
				.Send<ConnectionStateChangedMessage>( msg ) );
		}

		public Task StartAsync()
		{
			return _connection.Start();
		}

		private void UpdateQuote( Quote quote )
		{
			var msg = new QuoteUpdatedMessage()
			{
				Quote = quote
			};
			_messenger.Send<QuoteUpdatedMessage>( msg );
		}

		public void Stop()
		{
			_connection.Stop();
		}
	}

}
*/