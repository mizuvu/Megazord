using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Zord.Extensions;

namespace Zord.Core.Documents
{
    public class ColumnOptions<T>
    {
        public ColumnOptions(Action<ColumnOptions<T>> action)
        {
            action.Invoke(this);
        }

        public Dictionary<string, string> ColumnNames { get; private set; } = new Dictionary<string, string>();

        public void SetColumn(Expression<Func<T, string>> expr, string columnHeader)
        {
            var propName = ObjectHelper.GetPropertyName(expr);

            if (ColumnNames.ContainsKey(propName))
                ColumnNames[propName] = columnHeader;
            else
                ColumnNames.Add(propName, columnHeader);
        }
    }
}