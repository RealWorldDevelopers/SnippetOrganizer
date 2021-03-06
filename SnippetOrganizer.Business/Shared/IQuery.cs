﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace SnippetOrganizer.Business.Shared
{
   /// <summary>
   /// Generic Query for All Entities
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public interface IQuery<T>
   {
      /// <summary>
      /// Query all Data
      /// </summary>
      List<T> Execute();

      /// <summary>
      /// Query a specific record by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="object"/></param>
      T Execute(object id);

      /// <summary>
      /// Asynchronously query all Data
      /// </summary>
      Task<List<T>> ExecuteAsync();

      /// <summary>
      /// Asynchronously query a specific record by primary key
      /// </summary>
      /// <param name="id">Primary Key as <see cref="object"/></param>
      Task<T> ExecuteAsync(object id);

   }
}
