using System;
using System.Collections.Generic;

namespace SnippetOrganizer.Business.Snippet.Dto
{
   /// <summary>
   /// Class Representing a Single GitHub Gist
   /// </summary>
   public class SnippetDto
   {
      public SnippetDto()
      {
         Files = new List<FileDto>();
         Languages = new List<string>();
      }

      /// <summary>
      /// URL to GitHub Gist Page
      /// </summary>
      public Uri GitHubLink { get; set; }

      /// <summary>
      /// GitHub User for Gist
      /// </summary>
      public UserDto Owner { get; set; }

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
      public List<string> Languages { get; set; }
           
      /// <summary>
      /// List of Files that Make Up Gist
      /// </summary>
      public List<FileDto> Files { get;  }     
             
   }

}
