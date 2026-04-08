using SqlSugar;
using System.Linq.Expressions;
using System.Reflection;

namespace FastStart.Common.Utils
{
    /// <summary>
    /// 通用查询表达式构建器 - 智能推断版（不依赖 Expressionable）
    /// </summary>
    public static class QueryExpressionBuilder
    {
        /// <summary>
        /// 根据 DTO 构建查询表达式（智能推断操作符）
        /// </summary>
        public static Expression<Func<T, bool>> BuildExpression<T>(object queryDto) where T : class
        {
            if (queryDto == null)
                return x => true;

            var parameter = Expression.Parameter(typeof(T), "x");
            var conditions = new List<Expression>();

            var dtoProperties = queryDto.GetType().GetProperties();

            foreach (var prop in dtoProperties)
            {
                // 跳过分页、排序等非查询字段
                if (ShouldSkipProperty(prop))
                    continue;

                var attr = prop.GetCustomAttribute<QueryableAttribute>();

                // 如果显式标记 Ignore，跳过
                if (attr?.Ignore == true)
                    continue;

                var value = prop.GetValue(queryDto);
                if (IsNullOrEmpty(value))
                    continue;

                try
                {
                    // 获取实际要查询的属性名
                    var targetPropertyName = attr?.PropertyName ?? prop.Name;

                    // 自动推断操作符
                    var op = attr?.Operator ?? InferOperator(prop.PropertyType);

                    var condition = BuildCondition<T>(parameter, targetPropertyName, value, op);
                    if (condition != null)
                    {
                        conditions.Add(condition);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[QueryExpressionBuilder] 构建条件失败: {prop.Name}, Error: {ex.Message}");
                }
            }

            // 如果没有条件，返回全部
            if (conditions.Count == 0)
                return x => true;

            // 合并所有条件（AND）
            Expression finalExpression = conditions[0];
            for (int i = 1; i < conditions.Count; i++)
            {
                finalExpression = Expression.AndAlso(finalExpression, conditions[i]);
            }

            return Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
        }

        /// <summary>
        /// 为单个 DTO 属性构建查询条件
        /// </summary>
        private static Expression BuildCondition<T>(ParameterExpression parameter,
            string propertyName, object value, QueryOperator op) where T : class
        {
            var property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new ArgumentException($"属性 '{propertyName}' 在类型 '{typeof(T).Name}' 中不存在");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            return BuildConditionExpression(propertyAccess, value, op);
        }

        /// <summary>
        /// 构建条件表达式
        /// </summary>
        private static Expression BuildConditionExpression(Expression propertyAccess, object value, QueryOperator op)
        {
            return op switch
            {
                QueryOperator.Equal => Expression.Equal(propertyAccess, Expression.Constant(value)),

                QueryOperator.NotEqual => Expression.NotEqual(propertyAccess, Expression.Constant(value)),

                QueryOperator.Contains => BuildStringMethod(propertyAccess, value, "Contains"),

                QueryOperator.StartsWith => BuildStringMethod(propertyAccess, value, "StartsWith"),

                QueryOperator.EndsWith => BuildStringMethod(propertyAccess, value, "EndsWith"),

                QueryOperator.GreaterThan => Expression.GreaterThan(propertyAccess, Expression.Constant(value)),

                QueryOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(propertyAccess, Expression.Constant(value)),

                QueryOperator.LessThan => Expression.LessThan(propertyAccess, Expression.Constant(value)),

                QueryOperator.LessThanOrEqual => Expression.LessThanOrEqual(propertyAccess, Expression.Constant(value)),

                QueryOperator.In => BuildInCondition(propertyAccess, value),

                QueryOperator.Between => BuildBetweenCondition(propertyAccess, value),

                _ => throw new NotSupportedException($"不支持的查询操作符: {op}")
            };
        }

        /// <summary>
        /// 构建字符串方法调用（Contains, StartsWith, EndsWith）
        /// </summary>
        private static Expression BuildStringMethod(Expression propertyAccess, object value, string methodName)
        {
            var valueStr = value.ToString();
            var nullCheck = Expression.NotEqual(propertyAccess, Expression.Constant(null));

            var method = typeof(string).GetMethod(methodName, new[] { typeof(string) });
            var methodCall = Expression.Call(propertyAccess, method, Expression.Constant(valueStr));

            return Expression.AndAlso(nullCheck, methodCall);
        }

        /// <summary>
        /// 构建 IN 条件
        /// </summary>
        private static Expression BuildInCondition(Expression propertyAccess, object value)
        {
            if (value is System.Collections.IEnumerable enumerable && !(value is string))
            {
                var list = enumerable.Cast<object>().ToList();
                if (list.Count == 0)
                {
                    return null;
                }

                var containsMethod = typeof(List<object>).GetMethod("Contains");
                var listConstant = Expression.Constant(list);
                return Expression.Call(listConstant, containsMethod,
                    Expression.Convert(propertyAccess, typeof(object)));
            }

            throw new ArgumentException("IN 操作符的值必须是集合类型");
        }

        /// <summary>
        /// 构建 Between 条件（用于范围查询）
        /// </summary>
        private static Expression BuildBetweenCondition(Expression propertyAccess, object value)
        {
            if (value is Tuple<object, object> tuple)
            {
                var start = Expression.Constant(tuple.Item1);
                var end = Expression.Constant(tuple.Item2);
                var greaterOrEqual = Expression.GreaterThanOrEqual(propertyAccess, start);
                var lessOrEqual = Expression.LessThanOrEqual(propertyAccess, end);
                return Expression.AndAlso(greaterOrEqual, lessOrEqual);
            }

            throw new ArgumentException("Between 操作符的值必须是 Tuple<object, object> 类型");
        }

        /// <summary>
        /// 自动推断操作符
        /// </summary>
        private static QueryOperator InferOperator(Type propertyType)
        {
            var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            // 字符串类型 → 默认 LIKE
            if (underlyingType == typeof(string))
                return QueryOperator.Contains;

            // 其他类型 → 精确匹配
            return QueryOperator.Equal;
        }

        /// <summary>
        /// 判断是否应该跳过属性
        /// </summary>
        private static bool ShouldSkipProperty(PropertyInfo prop)
        {
            var skipProperties = new[]
            {
                "PageIndex", "pageIndex",
                "PageSize", "pageSize",
                "SortBy", "sortBy",
                "OrderBy", "orderBy",
                "Skip", "skip",
                "Take", "take"
            };

            return skipProperties.Contains(prop.Name);
        }

        /// <summary>
        /// 判断值是否为空
        /// </summary>
        private static bool IsNullOrEmpty(object value)
        {
            if (value == null) return true;
            if (value is string str && string.IsNullOrWhiteSpace(str)) return true;
            if (value is System.Collections.IEnumerable enumerable && !(value is string))
                return !enumerable.Cast<object>().Any();
            return false;
        }
    }
}