using SnippetOrganizer.Business.Shared;
using SnippetOrganizer.Business.Snippet.Dto;
using System;
using System.Threading.Tasks;

namespace SnippetOrganizer.Business.Snippet.Commands
{
   public class ModifySnippet : ICommand<SnippetDto>
   {
      //private readonly IMapper _mapper;
      //private readonly WMSContext _dbContext;

      ///// <summary>
      ///// Category Command Constructor
      ///// </summary>
      ///// <param name="dbContext">Entity Framework Context Instance as <see cref="WMSContext"/></param>
      ///// <param name="mapper">AutoMapper Instance as <see cref="IMapper"/></param>
      //public ModifyCategory(WMSContext dbContext, IMapper mapper)
      //{
      //   _dbContext = dbContext;
      //   _mapper = mapper;
      //}

      /// <summary>
      /// Add an <see cref="SnippetDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="SnippetDto"/></param>
      /// <returns><see cref="SnippetDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.Add(SnippetDto)"/>
      public SnippetDto Add(SnippetDto dto)
      {
         //   if (dto == null)
         //      throw new ArgumentNullException(nameof(dto));

         //   var entity = _mapper.Map<Categories>(dto);

         //   // add new recipe
         //   _dbContext.Categories.Add(entity);

         //   // Save changes in database
         //   _dbContext.SaveChanges();

         //   //dto.Id = entity.Id;
         //   return dto;
         return null;
      }

      /// <summary>
      /// Add an <see cref="SnippetDto"/> to Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="SnippetDto"/></param>
      /// <returns><see cref="Task{SnippetDto}"/></returns>
      /// <inheritdoc cref="ICommand{T}.AddAsync(SnippetDto)"/>
      public async Task<SnippetDto> AddAsync(SnippetDto dto)
      {
         //   if (dto == null)
         //      throw new ArgumentNullException(nameof(dto));

         //   var entity = _mapper.Map<Categories>(dto);

         //   // add new recipe
         //   await _dbContext.Categories.AddAsync(entity).ConfigureAwait(false);

         //   // Save changes in database
         //   await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         //   //dto.Id = entity.Id;
         //   return dto;
         throw new NotImplementedException();
      }

      /// <summary>
      /// Update a <see cref="SnippetDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="SnippetDto"/></param>
      /// <returns><see cref="SnippetDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.Update(T)"/>
      public SnippetDto Update(SnippetDto dto)
      {
         //   if (dto == null)
         //      throw new ArgumentNullException(nameof(dto));

         //   var entity = _dbContext.Categories.First(r => r.Id == dto.Id);
         //   entity.Description = dto.Description;
         //   entity.Enabled = dto.Enabled;
         //   entity.Category = dto.Literal;

         //   // Update entity in DbSet
         //   _dbContext.Categories.Update(entity);

         //   // Save changes in database
         //   _dbContext.SaveChanges();

         //   return dto;
         throw new NotImplementedException();
      }

      /// <summary>
      /// Update a <see cref="SnippetDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="SnippetDto"/></param>
      /// <returns><see cref="SnippetDto"/></returns>
      /// <inheritdoc cref="ICommand{T}.UpdateAsync(T)"/>
      public async Task<SnippetDto> UpdateAsync(SnippetDto dto)
      {
         //   if (dto == null)
         //      throw new ArgumentNullException(nameof(dto));

         //   var entity = await _dbContext.Categories.FirstAsync(r => r.Id == dto.Id).ConfigureAwait(false);
         //   entity.Description = dto.Description;
         //   entity.Enabled = dto.Enabled;
         //   entity.Category = dto.Literal;

         //   // Update entity in DbSet
         //   _dbContext.Categories.Update(entity);

         //   // Save changes in database
         //   await _dbContext.SaveChangesAsync().ConfigureAwait(false);

         //   return dto;
         throw new NotImplementedException();
      }

      /// <summary>
      /// Delete a <see cref="SnippetDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="SnippetDto"/></param>
      /// <inheritdoc cref="ICommand{T}.Delete(T)"/>
      public void Delete(SnippetDto dto)
      {
         //   var entity = _dbContext.Categories
         //   .FirstOrDefault(c => c.Id == dto.Id);

         //   if (entity != null)
         //   {
         //      // delete category 
         //      _dbContext.Categories.Remove(entity);

         //      // Save changes in database
         //      _dbContext.SaveChanges();
         //   }
         throw new NotImplementedException();

      }

      /// <summary>
      /// Delete a <see cref="SnippetDto"/> in the Database
      /// </summary>
      /// <param name="dto">Data Transfer Object as <see cref="SnippetDto"/></param>
      /// <inheritdoc cref="ICommand{T}.DeleteAsyn(T)"/>
      public async Task DeleteAsync(SnippetDto dto)
      {
         //   var entity = await _dbContext.Categories
         //   .FirstOrDefaultAsync(c => c.Id == dto.Id)
         //   .ConfigureAwait(false);

         //   if (entity != null)
         //   {
         //      // delete category 
         //      _dbContext.Categories.Remove(entity);

         //      // Save changes in database
         //      await _dbContext.SaveChangesAsync().ConfigureAwait(false);
         //   }
         throw new NotImplementedException();
      }

   }
}
