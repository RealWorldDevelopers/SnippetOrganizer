using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Octokit;
using RWD.Toolbox.PasswordGenerator;
using SnippetOrganizer.Models;
using SnippetOrganizer.Ui;
using SnippetOrganizer.Ui.Controllers;
using SnippetOrganizer.Ui.Models;

namespace SnippetOrganizer.Controllers
{
   // https://github.com/octokit/octokit.net (NuGet) 
   // https://github.com/octokit/octokit.net/blob/master/docs/oauth-flow.md
   // https://haacked.com/archive/2014/04/24/octokit-oauth/

   /// <summary>
   /// This API uses the GitHub API to get a list of the current user's repositories which include public and private repositories.
   /// </summary>
   public class HomeController : GitHubController
   {      
      public HomeController(GitHubClient gitClient, IOptions<AppSettings> appSettings, IPasswordGenerator passwordGenerator) : 
         base(gitClient, appSettings, passwordGenerator)
      {

      }


      public IActionResult Index()
      {
         // TODO build some sort of Home Page
         return RedirectToAction("Index","Gist");
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

         var actionController = HttpContext.Session.GetString("ReturnUrl").Split('|', StringSplitOptions.RemoveEmptyEntries);
         return RedirectToAction(actionController[0], actionController[1]);
      }
          

      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
      public IActionResult Error()
      {
         return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      }

   }
}
