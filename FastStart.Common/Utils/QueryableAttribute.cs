namespace FastStart.Common.Utils
{
    /// <summary>
    /// 标记 DTO 属性为可查询字段
    /// 不标记属性时，默认行为：
    /// - 字符串类型 → LIKE 查询（Contains）
    /// - 数值/日期类型 → 精确匹配（Equal）
    /// - 可空类型 → 精确匹配（Equal）
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryableAttribute : Attribute
    {
        /// <summary>
        /// 查询操作符（不指定时自动推断）
        /// </summary>
        public QueryOperator? Operator { get; set; }

        /// <summary>
        /// 是否忽略此属性（即使有值也跳过）
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// 自定义属性名映射（默认使用属性名）
        /// </summary>
        public string PropertyName { get; set; }

        public QueryableAttribute(QueryOperator? op = null, string propertyName = null)
        {
            Operator = op;
            PropertyName = propertyName;
        }
    }

    /// <summary>
    /// 查询操作符
    /// </summary>
    public enum QueryOperator
    {
        Equal,
        NotEqual,
        Contains,
        StartsWith,
        EndsWith,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        In,
        Between
    }
}
