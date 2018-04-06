using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
namespace Digishui.Extensions
{
  //===========================================================================================================================
  /// <summary>
  ///   System.Collections.Generic.IEnumerable Extensions
  /// </summary>
  /// <remarks>
  ///   https://stackoverflow.com/questions/3185067/singleordefault-throws-an-exception-on-more-than-one-element
  /// </remarks>
  public static partial class Extensions
  {
    //-------------------------------------------------------------------------------------------------------------------------
    public static TSource ExclusiveOrDefault<TSource>(this IEnumerable<TSource> source)
    {
      if (source == null)
        throw new ArgumentNullException("source");

      var results = source.Take(2).ToArray();

      return results.Length == 1 ? results[0] : default(TSource);
    }

    //-------------------------------------------------------------------------------------------------------------------------
    public static TSource ExclusiveOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      if (source == null)
        throw new ArgumentNullException("source");
      if (predicate == null)
        throw new ArgumentNullException("predicate");

      var results = source.Where(predicate).Take(2).ToArray();

      return results.Length == 1 ? results[0] : default(TSource);
    }
  }
}