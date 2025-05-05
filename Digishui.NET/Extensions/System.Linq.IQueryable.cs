using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui.Extensions
{
  //===========================================================================================================================
  /// <summary>
  ///   System.Linq.IQueryable Extensions
  /// </summary>
  /// <remarks>
  ///   https://stackoverflow.com/questions/3185067/singleordefault-throws-an-exception-on-more-than-one-element
  /// </remarks>
  public static partial class Extensions
  {
    public static async Task<TSource> ExclusiveOrDefaultAsync<TSource>(this IQueryable<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");

      var results = await source.Take(2).ToArrayAsync();

      return results.Length == 1 ? results[0] : default;
    }

    public static async Task<TSource> ExclusiveOrDefaultAsync<TSource>(this IQueryable<TSource> source, Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");

      var results = await source.Where(predicate).AsQueryable().Take(2).ToArrayAsync();

      return results.Length == 1 ? results[0] : default;
    }
  }
}