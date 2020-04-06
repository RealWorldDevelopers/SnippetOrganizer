namespace SnippetOrganizer.Business.Snippet.Commands
{
   public interface IFactory
   {
      ModifySnippet CreateSnippetCommand();
   }
}