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
using AspNet.Security.OpenIdConnect.Server;
using OpenIddict.Core;
using AspNet.Security.OpenIdConnect.Primitives;
using DAL.Models;
using DAL.Core;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using MOI.Patrol;
using Microsoft.AspNetCore.Http;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860


namespace AssetManagement.Controllers
{
    public class AuthorizationController : Controller
    {


        //private readonly IOptions<IdentityOptions> _identityOptions;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly UserManager<ApplicationUser> _userManager;

        //public AuthorizationController(
        //    IOptions<IdentityOptions> identityOptions,
        //    SignInManager<ApplicationUser> signInManager,
        //    UserManager<ApplicationUser> userManager)
        //{
        //    _identityOptions = identityOptions;
        //    _signInManager = signInManager;
        //    _userManager = userManager;
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
                user.Email = "mimran";
                user.UserName = "mimran";
                user.PasswordHash = "AQAAAAEAACcQAAAAEBlQUseSFv1ThSefLX/RLDe/zGhA7aEVrJPotIcfWGyf+FaqtqCIHLg1Vd+9xjqskg==";

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

                // var principal = await _signInManager.CreateUserPrincipalAsync(user);
                //var identity = new ClaimsIdentity(
                //    OpenIddictServerDefaults.AuthenticationScheme,
                //    OpenIdConnectConstants.Claims.Name,
                //    OpenIdConnectConstants.Claims.Role);
                //// Add a "sub" claim containing the user identifier, and attach
                //// the "access_token" destination to allow OpenIddict to store it
                //// in the access token, so it can be retrieved from your controllers.
                //identity.AddClaim(OpenIdConnectConstants.Claims.Subject,
                //    "71346D62-9BA5-4B6D-9ECA-755574D628D8",
                //    OpenIdConnectConstants.Destinations.AccessToken);
                //identity.AddClaim(OpenIdConnectConstants.Claims.Name, "Alice",
                //    OpenIdConnectConstants.Destinations.AccessToken);
                //// ... add other claims, if necessary.
                //var principal = new ClaimsPrincipal(identity);

                //return SignIn(principal,  OpenIddictServerDefaults.AuthenticationScheme);
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
            identity.AddClaim(OpenIdConnectConstants.Claims.Subject,
                "71346D62-9BA5-4B6D-9ECA-755574D628D8",
                OpenIdConnectConstants.Destinations.AccessToken);
            identity.AddClaim(OpenIdConnectConstants.Claims.Name, "Immuuu11",
                OpenIdConnectConstants.Destinations.AccessToken);
            identity.AddClaim("Configuration", "Configuration112",
                OpenIdConnectConstants.Destinations.AccessToken);
            identity.AddClaim("Configuration2", "Immuuu22",
               OpenIdConnectConstants.Destinations.IdentityToken);
            identity.AddClaim("ViewPatrolCars", "AllCars",
               OpenIdConnectConstants.Destinations.AccessToken);

            identity.AddClaim(ClaimTypes.Role, "ViewPatrolCarsRole",
               OpenIdConnectConstants.Destinations.AccessToken);
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
                    OpenIdConnectConstants.Scopes.Phone,
                    OpenIdConnectConstants.Scopes.Profile,
                    OpenIdConnectConstants.Scopes.OfflineAccess,
                    OpenIddictConstants.Scopes.Roles,
                    "ViewPatrolCars",
                    "ViewPatrolCarsRole"

            }.Intersect(request.GetScopes()));
            //}

            //ticket.SetResources("quickapp-api");

            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            foreach (var claim in ticket.Principal.Claims)
            {
                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                //if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType)
                //    continue;


                var destinations = new List<string> { OpenIdConnectConstants.Destinations.AccessToken };

                // Only add the iterated claim to the id_token if the corresponding scope was granted to the client application.
                // The other claims will only be added to the access_token, which is encrypted when using the default format.
                //if ((claim.Type == OpenIdConnectConstants.Claims.Subject && ticket.HasScope(OpenIdConnectConstants.Scopes.OpenId)) ||
                //    (claim.Type == OpenIdConnectConstants.Claims.Name && ticket.HasScope(OpenIdConnectConstants.Scopes.Profile)) ||
                //    (claim.Type == OpenIdConnectConstants.Claims.Role && ticket.HasScope(OpenIddictConstants.Claims.Roles)) ||
                //    (claim.Type == CustomClaimTypes.Permission && ticket.HasScope(OpenIddictConstants.Claims.Roles)))
                //{
                //    destinations.Add(OpenIdConnectConstants.Destinations.IdentityToken);
                //}
                destinations.Add(OpenIdConnectConstants.Destinations.IdentityToken);

                claim.SetDestinations(destinations);
            }


            // var identity = principal.Identity as ClaimsIdentity;


            //if (ticket.HasScope(OpenIdConnectConstants.Scopes.Profile))
            //{
            //    if (!string.IsNullOrWhiteSpace(user.JobTitle))
            //        identity.AddClaim(CustomClaimTypes.JobTitle, user.JobTitle, OpenIdConnectConstants.Destinations.IdentityToken);

            //    if (!string.IsNullOrWhiteSpace(user.FullName))
            //        identity.AddClaim(CustomClaimTypes.FullName, user.FullName, OpenIdConnectConstants.Destinations.IdentityToken);

            //    if (!string.IsNullOrWhiteSpace(user.Configuration))
            //        identity.AddClaim(CustomClaimTypes.Configuration, user.Configuration, OpenIdConnectConstants.Destinations.IdentityToken);
            //}

            //if (ticket.HasScope(OpenIdConnectConstants.Scopes.Email))
            //{
            //    if (!string.IsNullOrWhiteSpace(user.Email))
            //        identity.AddClaim(CustomClaimTypes.Email, user.Email, OpenIdConnectConstants.Destinations.IdentityToken);
            //}

            //if (ticket.HasScope(OpenIdConnectConstants.Scopes.Phone))
            //{
            //    if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            //        identity.AddClaim(CustomClaimTypes.Phone, user.PhoneNumber, OpenIdConnectConstants.Destinations.IdentityToken);
            //}


            return ticket;
        }
    }
}
