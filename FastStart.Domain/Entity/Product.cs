using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 产品表
    ///</summary>
    [SugarTable("product")]
    public class Product
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true)]
        public long Id { get; set; }

        /// <summary>   
        /// 货品编号
        /// </summary>
        [SugarColumn(ColumnName = "product_code")]
        public string Code { get; set; }

        /// <summary>
        /// 产品名称
        ///</summary>
        [SugarColumn(ColumnName = "product_name")]
        public string Name { get; set; }

        /// <summary>
        /// 产品类型
        ///</summary>
        [SugarColumn(ColumnName = "product_type")]
        public string Type { get; set; }

        /// <summary>
        /// 产品价格
        ///</summary>
        [SugarColumn(ColumnName = "product_price")]
        public string Price { get; set; }

        /// <summary>
        /// 数量
        ///</summary>
        [SugarColumn(ColumnName = "product_quantity")]
        public int Quantity { get; set; }
    }
}