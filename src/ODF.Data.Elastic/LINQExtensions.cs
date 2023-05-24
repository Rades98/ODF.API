using System;
using System.Linq.Expressions;

namespace ODF.Data.Elastic
{
	internal static class LINQExtensions
	{
		internal static Expression<Func<T, bool>> AndAlsoNext<T>(this Expression<Func<T, bool>> left,Expression<Func<T, bool>> right)
		{
			var param = Expression.Parameter(typeof(T), "x");
			var body = Expression.AndAlso(
					Expression.Invoke(left, param),
					Expression.Invoke(right, param)
				);
			var lambda = Expression.Lambda<Func<T, bool>>(body, param);
			return lambda;
		}
	}
}
