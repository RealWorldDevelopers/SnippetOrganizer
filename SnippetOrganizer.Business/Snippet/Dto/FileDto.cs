using System;

namespace SnippetOrganizer.Business.Snippet.Dto
{
   /// <summary>
   /// Abbreviated File Class from GitHub
   /// </summary>
   public class FileDto
   {
      /// <summary>
      /// Name of File as it was saved in GitHub
      /// </summary>
      public string FileName { get; set; }

      /// <summary>
      /// HTML Application Type
      /// </summary>
      public string Type { get; set; }

      /// <summary>
      /// Language of File via file extension
      /// </summary>
      public string Language { get; set; }

      /// <summary>
      /// URL to Get API call for this file
      /// </summary>
      public Uri RawUrl { get; set; }

      /// <summary>
      /// File Size
      /// </summary>
      public int Size { get; set; } 

      /// <summary>
      /// Contents of File
      /// </summary>
      public string Content { get; set; }
   }

}
