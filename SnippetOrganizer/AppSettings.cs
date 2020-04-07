using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnippetOrganizer.Ui
{
   public class AppSettings
   {      
         public GitHubConfig GitHubConfig { get; set; }
         
   }

   public class GitHubConfig
   {     
      public string ClientId { get; set; }
      public string ClientSecret { get; set; }
      public string ProductHeader { get; set; }
   }
}
