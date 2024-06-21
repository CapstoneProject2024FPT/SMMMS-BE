using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SAM.DataTier.Repository.Implement
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, object filter)
        {
            if (filter != null)
            {
                var properties = filter.GetType().GetProperties();

                foreach (var propertyInfo in properties)
                {
                    var propertyValue = propertyInfo.GetValue(filter, null);

                    if (propertyValue == null)
                        continue;

                    if (propertyValue is string stringValue && !string.IsNullOrWhiteSpace(stringValue))
                    {
                        query = query.Where(x => EF.Property<string>(x, propertyInfo.Name).ToLower().Contains(stringValue.ToLower()));
                    }
                    else if (propertyValue is List<string> stringListValue && stringListValue.Count > 0)
                    {
                        query = query.Where(x => stringListValue.Contains(EF.Property<string>(x, propertyInfo.Name).ToLower()));
                    }
                    // Add other types of filters here as needed
                }
            }

            return query;
        }
    }

}
