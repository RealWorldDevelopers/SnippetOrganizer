using Octokit;
using System.Collections.Generic;

namespace SnippetOrganizer.Ui.Models
{
   public class GistViewModel
   {
      public GistViewModel(IEnumerable<Gist> gist)
      {
         Gist = gist;
      }


      public IEnumerable<Gist> Gist { get; private set; }

   }
}
