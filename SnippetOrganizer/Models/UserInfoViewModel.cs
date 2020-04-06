using Octokit;
using System;
using System.ComponentModel;

namespace SnippetOrganizer.Ui.Models
{
   public class UserInfoViewModel
   {
      public bool WasBuiltPreviously { get; set; }
      
      public AuthenticationType AuthenticationType { get; set; }

      [DisplayName("Login Name")]
      public string Login { get; set; }

      [DisplayName("Core Rate Per Hour")]
      public int? CoreRatePerHour { get; set; }

      [DisplayName("Core Requests Remaining")]
      public int? CoreRequestsRemaining { get; set; }

      [DisplayName("Core Limit Resets on")]
      public DateTimeOffset? CoreLimitReset { get; set; }

      [DisplayName("Search Rate Per Minute")]
      public int? SearchRatePerMinute { get; set; }

      [DisplayName("Search Requests Remaining")]
      public int? SearchRequestsRemaining { get; set; }

      [DisplayName("Search Limit Resets on")]
      public DateTimeOffset? SearchLimitReset { get; set; }

   }
}
