using System.Threading.Tasks;

namespace SnippetOrganizer.Business.Shared
{
   /// <summary>
   /// Generic Command for Adding or Updating Entities
   /// </summary>
   public interface ICommand<T>
   {
      /// <summary>
      /// Add Data
      /// </summary>
      T Add(T dto);

      /// <summary>
      /// Asynchronously add Data
      /// </summary>
      Task<T> AddAsync(T dto);

      /// <summary>
      /// Update Data
      /// </summary>
      T Update(T dto);

      /// <summary>
      /// Asynchronously update Data
      /// </summary>
      Task<T> UpdateAsync(T dto);

      /// <summary>
      /// Delete Data
      /// </summary>
      void Delete(T dto);

      /// <summary>
      /// Asynchronously delete Data
      /// </summary>
      Task DeleteAsync(T dto);

   }
}
