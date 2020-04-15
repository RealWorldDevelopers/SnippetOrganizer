using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Octokit;
using RWD.Toolbox.PasswordGenerator;
using SnippetOrganizer.Ui.Models;

namespace SnippetOrganizer.Ui.Controllers
{
   public class RepoController : GitHubController
   {
      public RepoController(GitHubClient gitClient, IOptions<AppSettings> appSettings, IPasswordGenerator passwordGenerator) :
         base(gitClient, appSettings, passwordGenerator)
      {
         // local constructor stuff
      }

      public async Task<IActionResult> IndexAsync()
      {
         UserInfoViewModel userModel = null;
         try
         {
            ViewData["Title"] = "My Repos";
            HttpContext.Session.SetString("ReturnUrl", "Index|Repo");
            if (!SetClientCredentials())
               return Redirect(GetOauthLoginUrl());

            userModel = await BuildUserInfoModel(_gitClient);

            // The following requests retrieves all of the user's repositories and requires that the user be logged in to work.
            var repositories = await _gitClient.Repository.GetAllForCurrent();

            var model = new ReposViewModel();

            foreach (var r in repositories)
            {
               var repo = new RepoViewModel
               {
                  Id = r.Id,
                  DisplayName = r.Name,
                  Description = r.Description,
                  Public = !r.Private,
                  GitHubLink = new Uri(r.HtmlUrl)
               };
               model.Repos.Add(repo);
            }

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




   }
}