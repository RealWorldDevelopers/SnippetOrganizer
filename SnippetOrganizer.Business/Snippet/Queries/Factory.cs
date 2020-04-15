
using System;
using System.Collections.Generic;
using System.Text;

namespace SnippetOrganizer.Business.Snippet.Queries
{
   public class Factory : IFactory
   {
      public GetSnippets CreateSnippetsQuery()
      {
         return null;
         //return new GetSnippets();
      }
   }
}
