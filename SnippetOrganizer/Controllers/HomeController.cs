using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Octokit;
using RWD.Toolbox.PasswordGenerator;
using SnippetOrganizer.Models;
using SnippetOrganizer.Ui;
using SnippetOrganizer.Ui.Models;

namespace SnippetOrganizer.Controllers
{
   // https://github.com/octokit/octokit.net (NuGet) 
   // https://github.com/octokit/octokit.net/blob/master/docs/oauth-flow.md
   // https://haacked.com/archive/2014/04/24/octokit-oauth/

   /// <summary>
   /// This API uses the GitHub API to get a list of the current user's repositories which include public and private repositories.
   /// </summary>
   public class HomeController : Controller
   {
      private readonly AppSettings _appSettings;
      private readonly string _clientId;
      private readonly string _clientSecret;
      private readonly GitHubClient _gitClient;
      private IPasswordGenerator _pwdGenerator;

      public HomeController(GitHubClient gitClient, IOptions<AppSettings> appSettings, IPasswordGenerator passwordGenerator)
      {         
         _appSettings = appSettings.Value;

         // TODO move get snippet logic to business         

         _pwdGenerator = passwordGenerator;

         _clientId = _appSettings.GitHubConfig.ClientId;
         _clientSecret = _appSettings.GitHubConfig.ClientSecret;
         _gitClient = gitClient;

      }


      public async Task<IActionResult> Index()
      {
         UserInfoViewModel userModel = null;
         try
         {
            HttpContext.Session.SetString("ReturnUrl", "Index");
            if (!SetClientCredentials())
               return Redirect(GetOauthLoginUrl());

            userModel = await BuildUserInfoModel(_gitClient);

            // The following requests retrieves all of the user's repositories and requires that the user be logged in to work.
            var repositories = await _gitClient.Repository.GetAllForCurrent();

            var model = new IndexViewModel(repositories);

            return View(model);
         }
         catch (RateLimitExceededException)
         {
            // User Has Exceeded Rate Limits
            // Show Rates and When Rates will Reset
            return RedirectToAction("UserInfo", userModel);
         }
         catch (AuthorizationException)
         {
            // Either the accessToken is null or it's invalid. 
            // This redirects to the GitHub OAuth login page. 
            // That page will redirect back to the Authorize Action.
            return Redirect(GetOauthLoginUrl());
         }
      }

      public async Task<ActionResult> Gist()
      {
         UserInfoViewModel userModel = null;
         try
         {
            HttpContext.Session.SetString("ReturnUrl", "Gist");
            if (!SetClientCredentials())
               return Redirect(GetOauthLoginUrl());

            userModel = await BuildUserInfoModel(_gitClient);

            var user = await _gitClient.User.Current();
            var gist = await _gitClient.Gist.GetAllForUser(user.Login);

            var model = new GistViewModel(gist);

            return View(model);

         }
         catch (RateLimitExceededException)
         {
            // User Has Exceeded Rate Limits
            // Show Rates and When Rates will Reset
            return RedirectToAction("UseInfo", userModel);
         }
         catch (AuthorizationException)
         {
            // Either the accessToken is null or it's invalid. 
            // This redirects to the GitHub OAuth login page. 
            // That page will redirect back to the Authorize Action.
            return Redirect(GetOauthLoginUrl());
         }


      }

      private async Task<UserInfoViewModel> BuildUserInfoModel(GitHubClient client)
      {
         var user = await client.User.Current();
         var miscellaneousRateLimit = await _gitClient.Miscellaneous.GetRateLimits();
         var coreRateLimit = miscellaneousRateLimit.Resources.Core;
         var searchRateLimit = miscellaneousRateLimit.Resources.Search;
         var model = new UserInfoViewModel
         {
            AuthenticationType = client.Credentials.AuthenticationType,
            Login = user.Login,
            CoreRatePerHour = coreRateLimit.Limit,
            CoreRequestsRemaining = coreRateLimit.Remaining,
            CoreLimitReset = coreRateLimit.Reset.LocalDateTime,
            SearchRatePerMinute = searchRateLimit.Limit,
            SearchRequestsRemaining = searchRateLimit.Remaining,
            SearchLimitReset = searchRateLimit.Reset.LocalDateTime,
            WasBuiltPreviously = true
         };

         return model;
      }

      public async Task<IActionResult> UserInfo(UserInfoViewModel model)
      {
         if (!model.WasBuiltPreviously)
         {
            HttpContext.Session.SetString("ReturnUrl", "UserInfo");
            if (!SetClientCredentials())
               return Redirect(GetOauthLoginUrl());

            model = await BuildUserInfoModel(_gitClient);
         }

         return View(model);
      }

      public IActionResult Privacy()
      {
         return View();
      }



      private bool SetClientCredentials()
      {
         var accessToken = HttpContext.Session.GetString("OAuthToken");
         if (accessToken != null)
         {
            // This allows the client to make requests to the GitHub API on the user's behalf without ever having the user's OAuth credentials.
            _gitClient.Credentials = new Credentials(accessToken);

            return true;
         }
         return false;
      }


      /// <summary>
      /// This is the Callback URL that the GitHub OAuth Login page will redirect back to.
      /// </summary>
      public async Task<ActionResult> Authorized(string code, string state)
      {
         if (!string.IsNullOrEmpty(code))
         {
            var expectedState = HttpContext.Session.GetString("CSRF:State");
            if (state != expectedState)
               throw new InvalidOperationException("SECURITY FAIL!");
            HttpContext.Session.SetString("CSRF:State", string.Empty);

            var token = await _gitClient.Oauth.CreateAccessToken(
                new OauthTokenRequest(_clientId, _clientSecret, code));
            HttpContext.Session.SetString("OAuthToken", token.AccessToken);
         }

         return RedirectToAction(HttpContext.Session.GetString("ReturnUrl"));
      }

      private string GetOauthLoginUrl()
      {
         string csrf = _pwdGenerator.Generate(24);
         HttpContext.Session.SetString("CSRF:State", csrf);

         // Redirect users to request GitHub access
         var request = new OauthLoginRequest(_clientId)
         {
            Scopes = { "user", "notifications" },
            State = csrf
         };
         var oauthLoginUrl = _gitClient.Oauth.GetGitHubLoginUrl(request);
         return oauthLoginUrl.ToString();
      }


      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
      public IActionResult Error()
      {
         return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      }

   }
}
