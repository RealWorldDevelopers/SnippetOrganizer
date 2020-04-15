
using System;
using System.Collections.Generic;

namespace SnippetOrganizer.Ui.Models
{
   public class ReposViewModel
   {
      public ReposViewModel()
      {
         Repos = new List<RepoViewModel>();
      }
      public List<RepoViewModel> Repos { get; }
   }

   public class RepoViewModel
   {
      public RepoViewModel()
      {

      }

      /// <summary>
      /// URL to GitHub Repo Page
      /// </summary>
      public Uri GitHubLink { get; set; }

      /// <summary>
      /// Is Repo Public Viewable
      /// </summary>
      public bool Public { get; set; }

      /// <summary>
      /// Primary Key of Repo
      /// </summary>
      public long Id { get; set; }

      /// <summary>
      /// Description of Repo
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Name of Repo assigned by GitHub
      /// </summary>
      public string DisplayName { get; set; }

   }

}
