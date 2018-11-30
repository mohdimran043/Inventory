// ====================================================

// Email: support@ebenmonney.com
// ====================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNet.Security.OpenIdConnect.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using OpenIddict.Core;
using AspNet.Security.OpenIdConnect.Primitives;
using System.Security.Claims;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using MOI.Patrol;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MOI.Patrol.DataAccessLayer;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860


namespace AssetManagement.Controllers
{
    public class AuthorizationController : Controller
    {


        //private readonly IOptions<IdentityOptions> _identityOptions;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly UserManager<ApplicationUser> _userManager;

        //public AuthorizationController(
        //    IOptions<IdentityOptions> identityOptions
        //    )
        //{
        //    _identityOptions = identityOptions;
        //}
        private readonly patrolsContext _context;
        public AuthorizationController(patrolsContext context,
            IHttpContextAccessor httpAccessor)
        {
            if (httpAccessor.HttpContext?.User.FindFirst(OpenIdConnectConstants.Claims.Subject) != null)
            {
                //   _context.CurrentUserId = httpAccessor.HttpContext?.User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value?.Trim();
            }
            //  _context.CurrentUserId = httpAccessor.HttpContext?.User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value?.Trim();
        }


        [HttpPost("~/connect/token")]
        [Produces("application/json")]
        public async Task<IActionResult> Exchange(OpenIdConnectRequest request)
        {
            if (request.IsPasswordGrantType())
            {
                string userName = HttpContext.User.Identity.Name;
                IdentityUser user = new IdentityUser();
                user.Email = "admin";
                user.UserName = "admin";

                //  var user = await _userManager.FindByEmailAsync(request.Username) ?? await _userManager.FindByNameAsync(request.Username);
                //if (user == null)
                //{
                //    return BadRequest(new OpenIdConnectResponse
                //    {
                //        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                //        ErrorDescription = "Please check that your email and password is correct"
                //    });
                //}

                //// Ensure the user is enabled.
                //if (!user.IsEnabled)
                //{
                //    return BadRequest(new OpenIdConnectResponse
                //    {
                //        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                //        ErrorDescription = "The specified user account is disabled"
                //    });
                //}


                //// Validate the username/password parameters and ensure the account is not locked out.
                //var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);

                //// Ensure the user is not already locked out.
                //if (result.IsLockedOut)
                //{
                //    return BadRequest(new OpenIdConnectResponse
                //    {
                //        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                //        ErrorDescription = "The specified user account has been suspended"
                //    });
                //}

                //// Reject the token request if two-factor authentication has been enabled by the user.
                //if (result.RequiresTwoFactor)
                //{
                //    return BadRequest(new OpenIdConnectResponse
                //    {
                //        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                //        ErrorDescription = "Invalid login procedure"
                //    });
                //}

                //// Ensure the user is allowed to sign in.
                //if (result.IsNotAllowed)
                //{
                //    return BadRequest(new OpenIdConnectResponse
                //    {
                //        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                //        ErrorDescription = "The specified user is not allowed to sign in"
                //    });
                //}

                //if (!result.Succeeded)
                //{
                //    return BadRequest(new OpenIdConnectResponse
                //    {
                //        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                //        ErrorDescription = "Please check that your email and password is correct"
                //    });
                //}



                // Create a new authentication ticket.
                //  var ticket = await CreateTicketAsync(request, user);
                var ticket = await CreateTicketAsync(request, user);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);

            }
            else if (request.IsRefreshTokenGrantType())
            {
                string userName = HttpContext.User.Identity.Name;
                IdentityUser user = new IdentityUser();
                user.Email = "admin";
                user.UserName = "admin";

                // Create a new authentication ticket, but reuse the properties stored
                // in the refresh token, including the scopes originally granted.
                var ticket = await CreateTicketAsync(request, user);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }
            return BadRequest(new OpenIdConnectResponse
            {
                Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported"
            });
        }

        private async Task<AuthenticationTicket> CreateTicketAsync(OpenIdConnectRequest request, IdentityUser user)
        {
            // Create a new ClaimsPrincipal containing the claims that
            // will be used to create an id_token, a token or a code.
            // var principal = await _signInManager.CreateUserPrincipalAsync(user);

            var identity = new ClaimsIdentity(
                OpenIddictServerDefaults.AuthenticationScheme,
                OpenIdConnectConstants.Claims.Name,
                OpenIdConnectConstants.Claims.Role);
            // Add a "sub" claim containing the user identifier, and attach
            // the "access_token" destination to allow OpenIddict to store it
            // in the access token, so it can be retrieved from your controllers.
            Users _user = PatrolUserManager.GetUserByUserName(user.UserName);
            identity.AddClaim(OpenIdConnectConstants.Claims.Subject,
               _user.Userid.ToString(),
                OpenIdConnectConstants.Destinations.AccessToken);
            identity.AddClaim(OpenIdConnectConstants.Claims.Name, _user.Username,
                OpenIdConnectConstants.Destinations.AccessToken);
            identity.AddClaim("userName", _user.Username,
       OpenIdConnectConstants.Destinations.AccessToken);
            identity.AddClaim("empDisplayName", _user.Name,
             OpenIdConnectConstants.Destinations.AccessToken);
         //   identity.AddClaim("mNO", "",
         // OpenIdConnectConstants.Destinations.AccessToken);
         //   identity.AddClaim("empDeptCode", "",
         //OpenIdConnectConstants.Destinations.AccessToken);
         //   identity.AddClaim("empDeptName", "",
         //OpenIdConnectConstants.Destinations.AccessToken);

            var userRoles = PatrolUserManager.GetRolesByUserId(_user.Userid);
            foreach (var role in userRoles)
            {
                identity.AddClaim(ClaimTypes.Role, role,
                   OpenIdConnectConstants.Destinations.AccessToken);
            }
            var userLeftNavigation=
            identity.AddClaim("userLeftNavigation", PatrolUserManager.FetchLeftNavigationByUserId(_user.Userid),
        OpenIdConnectConstants.Destinations.AccessToken);
            //identity.AddClaim(ClaimTypes.Role, "ViewPatrolCarsRole",
            //   OpenIdConnectConstants.Destinations.AccessToken);
            //identity.AddClaim(ClaimTypes.Role, "ViewPatrolCarsRole",
            //  OpenIdConnectConstants.Destinations.AccessToken);

            // ... add other claims, if necessary.
            var principal = new ClaimsPrincipal(identity);

            // Create a new authentication ticket holding the user identity.
            var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), OpenIddictServerDefaults.AuthenticationScheme);


            //if (!request.IsRefreshTokenGrantType())
            //{
            // Set the list of scopes granted to the client application.
            // Note: the offline_access scope must be granted
            // to allow OpenIddict to return a refresh token.
            ticket.SetScopes(new[]
            {
                    OpenIdConnectConstants.Scopes.OpenId,
                    OpenIdConnectConstants.Scopes.Email,
                    OpenIdConnectConstants.Scopes.OfflineAccess,
                    OpenIddictConstants.Scopes.Roles

            }.Intersect(request.GetScopes()));

            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.
            foreach (var claim in ticket.Principal.Claims)
            {
                //// Never include the security stamp in the access and identity tokens, as it's a secret value.
                //if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType)
                //    continue;


                var destinations = new List<string> { OpenIdConnectConstants.Destinations.AccessToken };
                destinations.Add(OpenIdConnectConstants.Destinations.IdentityToken);

                claim.SetDestinations(destinations);
            }



            return ticket;
        }
    }
}
