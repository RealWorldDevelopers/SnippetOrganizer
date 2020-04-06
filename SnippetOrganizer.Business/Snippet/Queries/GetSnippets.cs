using Octokit;
using SnippetOrganizer.Business.Shared;
using SnippetOrganizer.Business.Snippet.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnippetOrganizer.Business.Snippet.Queries
{
   public class GetSnippets : IQuery<SnippetDto>
   {
      public GetSnippets()
      {

      }

      public List<SnippetDto> Execute()
      {       

         //var recipes = _dbContext.Recipes.Include("PicturesXref").Include("Ratings").ToList();
         //var list = _mapper.Map<List<RecipeDto>>(recipes);
         //var categories = _dbContext.Categories.ToList();
         //var varieties = _dbContext.Varieties.ToList();

         //foreach (var item in list)
         //{
         //   if (item.Variety != null)
         //   {
         //      var code = varieties.SingleOrDefault(a => a.Id == item.Variety.Id);
         //      item.Variety.Literal = code.Variety;
         //      var cat = _dbContext.Categories.SingleOrDefault(a => a.Id == code.CategoryId);
         //      item.Category = _mapper.Map<ICode>(cat);
         //   }
         //}

         throw new NotImplementedException();
      }

      public async Task<List<SnippetDto>> ExecuteAsync()
      {
         var client = new GitHubClient(new ProductHeaderValue("SnippetOrganizer"));

         // fails due to no dual factor setup us oauth
         //var basicAuth = new Credentials("realworlddevelopers", "P!n0t_Gr1$"); // NOTE: real credentials do not use
         //client.Credentials = basicAuth;

         // https://github.com/octokit/octokit.net/blob/master/docs/oauth-flow.md
         //https://haacked.com/archive/2014/04/24/octokit-oauth/

         //var tokenAuth = new Credentials("token"); // NOTE: not real token
         //client.Credentials = tokenAuth;

         //var user = await client.User.Get("RealWorldDevelopers");
         var gists = await client.Gist.GetAll();




         var miscellaneousRateLimit = await client.Miscellaneous.GetRateLimits();

         //  The "core" object provides your rate limit status except for the Search API.
         var coreRateLimit = miscellaneousRateLimit.Resources.Core;

         var howManyCoreRequestsCanIMakePerHour = coreRateLimit.Limit;
         var howManyCoreRequestsDoIHaveLeft = coreRateLimit.Remaining;
         var whenDoesTheCoreLimitReset = coreRateLimit.Reset; // UTC time

         // the "search" object provides your rate limit status for the Search API.
         var searchRateLimit = miscellaneousRateLimit.Resources.Search;

         var howManySearchRequestsCanIMakePerMinute = searchRateLimit.Limit;
         var howManySearchRequestsDoIHaveLeft = searchRateLimit.Remaining;
         var whenDoesTheSearchLimitReset = searchRateLimit.Reset; // UTC time


         return new List<SnippetDto>();

      }

      public SnippetDto Execute(int id)
      {
         throw new NotImplementedException();
      }     

      public Task<SnippetDto> ExecuteAsync(int id)
      {
         throw new NotImplementedException();
      }
   }
}
