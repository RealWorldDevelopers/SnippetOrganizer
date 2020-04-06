using System;
using System.Collections.Generic;
using System.Text;

namespace SnippetOrganizer.Business.Snippet.Dto
{
   public class Factory : IFactory
   {
      public SnippetDto CreateNewSnippet()
      {
         return new SnippetDto();
      }
   }
}
