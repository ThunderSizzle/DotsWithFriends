using DotsWithFriends.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace WebAPI
{
	public class TokenValidationAttribute: ActionFilterAttribute
	{
		public override void OnActionExecuting( System.Web.Http.Controllers.HttpActionContext actionContext )
		{
			string token;

			try
			{
				token = actionContext.Request.Headers.GetValues("Authorization-Token").First();
			}
			catch(Exception error)
			{
				actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
				{
					Content = new StringContent("Missing Authorization-Token")
				};
				return;
			}
			try
			{
				Context db = new Context();
				var User = db.Users.First( c => c.Id == token );
				base.OnActionExecuting( actionContext );
			}
			catch(Exception)
			{
				actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
				{
					Content = new StringContent("Unauthorized User")
				};
				return;
			}
		}
	}
}