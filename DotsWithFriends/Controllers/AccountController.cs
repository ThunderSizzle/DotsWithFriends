using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using DotsWithFriends.Models;
using DotsWithFriends.Providers;
using DotsWithFriends.Results;
using WebAPI;

namespace DotsWithFriends.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
		private const string LocalLoginProvider = "Local";
		private const string XsrfKey = "XsrfId";
		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.Current.GetOwinContext().Authentication;
			}
		}
		public UserManager<IdentityUser> UserManager { get; private set; }
		public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }
		public Context db = new Context();		

        public AccountController()
			: base()
        {
			this.UserManager = new UserManager<IdentityUser>( new UserStore<IdentityUser>( this.db ) );
			this.AccessTokenFormat = Startup.OAuthOptions.AccessTokenFormat;
        }

        public AccountController(UserManager<IdentityUser> userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
			//UserManager.UserValidator = new UserValidator<IdentityUser>( UserManager ) { AllowOnlyAlphanumericUserNames = false };
            AccessTokenFormat = accessTokenFormat;
        }
 
        // GET api/Account/UserInfo
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
		[Route( "UserInfo" )]
		[TokenValidationAttribute]
        public IHttpActionResult GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
		
			var token = ControllerContext.Request.Headers.GetValues("Authorization-Token").First();
			var user = db.Users.First( c => c.Id == token);

            return Ok(user.UserName);
            /*{
                UserName = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };*/
        }

		// POST api/Account/LogInExternal
		[AllowAnonymous]
		[Route( "LogInExternal" )]
		public async Task<IHttpActionResult> LogInExternal(RegisterExternalBindingModel model)
		{

			if(!ModelState.IsValid)
			{
				return BadRequest( ModelState );
			}
			
			if(User.Identity.IsAuthenticated == true)
			{
				if(User.Identity.Name == model.Email)
				{
					return Ok("Already Authenticated");
				}
				else
				{
					return Unauthorized();
				}
			}
			else
			{
				try
				{
					var user = await UserManager.FindAsync(new UserLoginInfo("microsoft", model.ExternalAccessToken));
					if( user != null)
					{
						//Return Token (GUID of User)
						return Ok(user.Id);
					}
					else
					{
						user = new MyUser
						{
							UserName = model.Id,
							Email = model.Email,
							FirstName = model.FirstName,
							LastName = model.LastName
						};
						user.Logins.Add( new IdentityUserLogin
						{
							LoginProvider = "microsoft",
							ProviderKey = model.ExternalAccessToken
						} );
						IdentityResult result = await UserManager.CreateAsync( user );
						IHttpActionResult errorResult = GetErrorResult( result );
						if ( errorResult != null )
						{
							return errorResult;
						}
						//Return Token (GUID of User)
						return Ok( user.Id );
					}
				}
				catch(Exception error)
				{
					return BadRequest(error.Message);
				}
			}
		}



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserManager.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Helpers


		private class ChallengeResult
		{
			public ChallengeResult( string provider, string redirectUri )
				: this( provider, redirectUri, null )
			{
			}
			public ChallengeResult( string provider, string redirectUri, string userId )
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}
			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }
			public void ExecuteResult( ApiController context )
			{
				var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
				if ( UserId != null )
				{
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.ActionContext.ControllerContext.Request.GetOwinContext().Authentication.Challenge( properties, LoginProvider );
			}
		}

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
