using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace YourMotivation.Web.Extensions
{
  public static class EntityFrameworkExtensions
  {
    public static Task<List<TSource>> ToListAsyncSafe<TSource>(
      this IQueryable<TSource> source)
    {
      return source is IAsyncEnumerable<TSource> ?
        source.ToListAsync() :
        Task.FromResult(source.ToList());
    }

    public static Task<int> CountAsyncSafe<TSource>(
      this IQueryable<TSource> source)
    {
      return source is IAsyncEnumerable<TSource> ?
        source.CountAsync() :
        Task.FromResult(source.Count());
    }
  }
}
