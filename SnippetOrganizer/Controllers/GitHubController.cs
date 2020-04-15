using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Octokit;
using RWD.Toolbox.PasswordGenerator;
using SnippetOrganizer.Ui.Models;
using System.Threading.Tasks;

namespace SnippetOrganizer.Ui.Controllers
{
   public class GitHubController : Controller
   {
      protected AppSettings _appSettings;
      protected string _clientId;
      protected string _clientSecret;
      protected GitHubClient _gitClient;
      protected IPasswordGenerator _pwdGenerator;

      public GitHubController(GitHubClient gitClient, IOptions<AppSettings> appSettings, IPasswordGenerator passwordGenerator)
      {
         _appSettings = appSettings.Value;

         _pwdGenerator = passwordGenerator;

         _clientId = _appSettings.GitHubConfig.ClientId;
         _clientSecret = _appSettings.GitHubConfig.ClientSecret;
         _gitClient = gitClient;
      }

      protected async Task<UserInfoViewModel> BuildUserInfoModel(GitHubClient client)
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


      protected bool SetClientCredentials()
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


      protected string GetOauthLoginUrl()
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


   }

}
