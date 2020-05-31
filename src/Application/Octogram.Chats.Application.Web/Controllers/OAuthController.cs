using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Octogram.Chats.Application.Web.Controllers
{
	[Route("/oauth")]
	public class OAuthController : ControllerBase
	{
		[HttpGet("github")]
		public IActionResult GithubLogin()
		{
			return Challenge(new AuthenticationProperties{ RedirectUri = "/Account" }, "Github");
		}
	}
}
