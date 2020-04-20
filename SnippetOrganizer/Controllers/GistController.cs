using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Octokit;
using RWD.Toolbox.PasswordGenerator;
using SnippetOrganizer.Ui.Models;
using System;
using System.Threading.Tasks;

namespace SnippetOrganizer.Ui.Controllers
{
   public class GistController : GitHubController
   {
      public GistController(GitHubClient gitClient, IOptions<AppSettings> appSettings, IPasswordGenerator passwordGenerator) :
         base(gitClient, appSettings, passwordGenerator)
      {
         // local constructor stuff
      }

      public async Task<ActionResult> Index()
      {
         // TODO Add some filtering category type property

         // TODO get link, email code,  do more than just display list


         UserInfoViewModel userModel = null;
         try
         {
            ViewData["Title"] = "My Gist";
            HttpContext.Session.SetString("ReturnUrl", "Index|Gist");

            if (!SetClientCredentials())
               return Redirect(GetOauthLoginUrl());

            userModel = await BuildUserInfoModel(_gitClient);

            var user = await _gitClient.User.Current();
            var gists = await _gitClient.Gist.GetAllForUser(user.Login);

            var model = new GistsViewModel();

            foreach (var g in gists)
            {
               var gist = new GistViewModel
               {
                  Id = g.Id,
                  Description = g.Description,
                  Public = g.Public,
                  GitHubLink = new Uri(g.HtmlUrl)
               };

               foreach (var f in g.Files)
               {
                  // no dupes for display only
                  if (!gist.Languages.Contains(f.Value.Language))
                     gist.Languages.Add(f.Value.Language);

                  // content not null only on singe gist retrieval
                  var file = new FileViewModel
                  {
                     RawUrl = new Uri(f.Value.RawUrl),
                     FileName = f.Value.Filename,
                     Language = f.Value.Language,
                     Type = f.Value.Type
                  };
                  gist.Files.Add(file);
               }

               gist.Languages.Sort();
               model.Gists.Add(gist);
            }

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


   }
}