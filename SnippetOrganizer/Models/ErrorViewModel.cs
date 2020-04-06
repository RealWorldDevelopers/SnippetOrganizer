using System;

namespace SnippetOrganizer.Models
{
   public class ErrorViewModel
   {
      public string RequestId { get; set; }

      public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
   }
}
