using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Octokit;
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

      // TODO move GitHub calls to a communications or data or business layer        
      private readonly string clientId;      
      private readonly string clientSecret;    
      readonly GitHubClient gitClient;

      public HomeController(IOptions<AppSettings> appSettings)
      {
         _appSettings = appSettings.Value;
         clientId = _appSettings.GitHubConfig.ClientId;
         clientSecret = _appSettings.GitHubConfig.ClientSecret;

         // TODO make this a DI
         gitClient = new GitHubClient(new ProductHeaderValue(_appSettings.GitHubConfig.ProductHeader));
      }


      public async Task<IActionResult> Index()
      {
         UserInfoViewModel userModel = null;
         try
         {
            _appSettings.GitHubConfig.ReturnUrl = "Index";
            if (!SetClientCredentials())
               return Redirect(GetOauthLoginUrl());

            userModel = await BuildUserInfoModel(gitClient);

            // The following requests retrieves all of the user's repositories and requires that the user be logged in to work.
            var repositories = await gitClient.Repository.GetAllForCurrent();

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
            _appSettings.GitHubConfig.ReturnUrl = "Gist";
            if (!SetClientCredentials())
               return Redirect(GetOauthLoginUrl());

            userModel = await BuildUserInfoModel(gitClient);            

            var user = await gitClient.User.Current();
            var gist = await gitClient.Gist.GetAllForUser(user.Login);

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
         var miscellaneousRateLimit = await gitClient.Miscellaneous.GetRateLimits();
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
            _appSettings.GitHubConfig.ReturnUrl = "UserInfo";
            if (!SetClientCredentials())
               return Redirect(GetOauthLoginUrl());

            model = await BuildUserInfoModel(gitClient);
         }

         return View(model);
      }

      public IActionResult Privacy()
      {
         return View();
      }



      private bool SetClientCredentials()
      {
         var accessToken = _appSettings.GitHubConfig.OAuthToken; //System.Web. Session["OAuthToken"] as string;
         if (accessToken != null)
         {
            // This allows the client to make requests to the GitHub API on the user's behalf without ever having the user's OAuth credentials.
            gitClient.Credentials = new Credentials(accessToken);
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
            var expectedState = _appSettings.GitHubConfig.State; //Session["CSRF:State"] as string;
            if (state != expectedState)
               throw new InvalidOperationException("SECURITY FAIL!");
            _appSettings.GitHubConfig.State = null; //Session["CSRF:State"] = null;

            var token = await gitClient.Oauth.CreateAccessToken(
                new OauthTokenRequest(clientId, clientSecret, code));
            _appSettings.GitHubConfig.OAuthToken = token.AccessToken; //Session["OAuthToken"] = token.AccessToken;
         }

         return RedirectToAction(_appSettings.GitHubConfig.ReturnUrl);
      }
      
      private string GetOauthLoginUrl()
      {
         // TODO use RWD NuGet Package
         var pwg = new PasswordGenerator();

         string csrf = pwg.Generate(24);
         _appSettings.GitHubConfig.State = csrf; //Session["CSRF:State"] = csrf;

         // Redirect users to request GitHub access
         var request = new OauthLoginRequest(clientId)
         {
            Scopes = { "user", "notifications" },
            State = csrf
         };
         var oauthLoginUrl = gitClient.Oauth.GetGitHubLoginUrl(request);
         return oauthLoginUrl.ToString();
      }


      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
      public IActionResult Error()
      {
         return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      }

   }
}
