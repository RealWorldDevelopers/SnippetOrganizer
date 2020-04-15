
using Octokit;
using RWD.Toolbox.PasswordGenerator;
using SnippetOrganizer.Business.Shared;
using SnippetOrganizer.Business.Snippet.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnippetOrganizer.Business.Snippet.Queries
{   


   /// <inheritdoc cref="IQuery{SnippetDto}"/>
   public class GetSnippets : IQuery<SnippetDto>
   {
      // private readonly AppSettings _appSettings;
      //private readonly string _clientId;
      //private readonly string _clientSecret;
      private readonly GitHubClient _gitClient;
      private IPasswordGenerator _pwdGenerator;

      public GetSnippets(GitHubClient gitClient, IPasswordGenerator passwordGenerator)
      {
         // _clientId = _appSettings.GitHubConfig.ClientId;
         // _clientSecret = _appSettings.GitHubConfig.ClientSecret;
         _gitClient = gitClient;
      }

      /// <inheritdoc cref="IQuery{SnippetDto}.Execute"/>
      public List<SnippetDto> Execute()
      {
         throw new NotImplementedException();
      }

      /// <inheritdoc cref="IQuery{SnippetDto}.ExecuteAsync"/>
      public async Task<List<SnippetDto>> ExecuteAsync()
      {
         var gists = new List<SnippetDto>();






         var user = await _gitClient.User.Current();
         var gist = await _gitClient.Gist.GetAllForUser(user.Login);

         foreach (var g in gist)
         {
            var dto = new SnippetDto
            {
               Id = g.Id,
               Description = g.Description,
               Public = g.Public,
               GitHubLink = new Uri(g.HtmlUrl),
               Owner = new UserDto { Id = g.Owner.Id, Login = g.Owner.Login }
            };
            foreach (var f in g.Files)
            {
               dto.Languages.Add(f.Value.Language);
            }

            gists.Add(dto);
         }

         return gists;

      }

      /// <inheritdoc cref="IQuery{SnippetDto}.Execute(object)"/>
      public SnippetDto Execute(object id)
      {
         throw new NotImplementedException();
      }

      /// <inheritdoc cref="IQuery{SnippetDto}.ExecuteAsync(object)"/>
      public async Task<SnippetDto> ExecuteAsync(object primaryKey)
      {
         var id = primaryKey.ToString();
         if (string.IsNullOrWhiteSpace(id))
            throw new NullReferenceException(nameof(primaryKey));

         var gist = await _gitClient.Gist.Get(id.ToString());

         var dto = new SnippetDto
         {
            Id = gist.Id,
            Description = gist.Description,
            Public = gist.Public,
            GitHubLink = new Uri(gist.HtmlUrl),
            Owner = new UserDto { Id = gist.Owner.Id, Login = gist.Owner.Login }
         };

         foreach (var item in gist.Files)
         {
            var f = item.Value;
            var file = new FileDto
            {
               Content = f.Content,
               FileName = f.Filename,
               Language = f.Language,
               RawUrl = new Uri(f.RawUrl),
               Size = f.Size,
               Type = f.Type
            };
            dto.Files.Add(file);
         }

         return dto;
      }



   }
}
