using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DotsWithFriends.Startup))]

namespace DotsWithFriends
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			ConfigureAuth( app );
			app.MapSignalR();
        }
    }
}
