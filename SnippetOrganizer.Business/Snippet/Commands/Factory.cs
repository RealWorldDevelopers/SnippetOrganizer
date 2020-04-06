using System;
using System.Collections.Generic;
using System.Text;

namespace SnippetOrganizer.Business.Snippet.Commands
{
   public class Factory : IFactory
   {
      public ModifySnippet CreateSnippetCommand()
      {
         return new ModifySnippet();
      }
   }
}
