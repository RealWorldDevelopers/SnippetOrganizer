using System;
using System.Collections.Generic;

namespace SnippetOrganizer.Ui.Models
{
   public class GistsViewModel
   {
      public GistsViewModel()
      {
         Gists = new List<GistViewModel>();
      }
      public List<GistViewModel> Gists { get; }
   }

   public class GistViewModel
   {
      public GistViewModel()
      {
         Files = new List<FileViewModel>();
         Languages = new List<string>();
      }

      /// <summary>
      /// URL to GitHub Gist Page
      /// </summary>
      public Uri GitHubLink { get; set; }

      /// <summary>
      /// Is Gist Public Viewable
      /// </summary>
      public bool Public { get; set; }

      /// <summary>
      /// Primary Key of Gist
      /// </summary>
      public string Id { get; set; }

      /// <summary>
      /// Description of Gist
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Programming Languages Collected from Files 
      /// </summary>
      public List<string> Languages { get; }

      /// <summary>
      /// List of Files that Make Up Gist
      /// </summary>
      public List<FileViewModel> Files { get; }


   }
}
