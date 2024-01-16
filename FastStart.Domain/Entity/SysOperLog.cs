using SqlSugar;

namespace FastStart.Domain.Entity
{
    /// <summary>
    /// 操作日志记录
    ///</summary>
    [SugarTable("sys_oper_log")]
    public class SysOperLog
    {
        /// <summary>
        /// 日志主键
        ///</summary>
        [SugarColumn(ColumnName = "oper_id", IsPrimaryKey = true, IsIdentity = true)]
        public long OperId { get; set; }

        /// <summary>
        /// 模块标题
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// 业务类型（0其它 1新增 2修改 3删除）
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "business_type")]
        public int? BusinessType { get; set; }

        /// <summary>
        /// 方法名称
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "method")]
        public string Method { get; set; }

        /// <summary>
        /// 请求方式
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "request_method")]
        public string RequestMethod { get; set; }

        /// <summary>
        /// 操作类别（0其它 1后台用户 2手机端用户）
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "operator_type")]
        public int? OperatorType { get; set; }

        /// <summary>
        /// 操作人员
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "oper_name")]
        public string OperName { get; set; }

        /// <summary>
        /// 部门名称
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "dept_name")]
        public string DeptName { get; set; }

        /// <summary>
        /// 请求URL
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "oper_url")]
        public string OperUrl { get; set; }

        /// <summary>
        /// 主机地址
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "oper_ip")]
        public string OperIp { get; set; }

        /// <summary>
        /// 操作地点
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "oper_location")]
        public string OperLocation { get; set; }

        /// <summary>
        /// 请求参数
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "oper_param")]
        public string OperParam { get; set; }

        /// <summary>
        /// 返回参数
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "json_result")]
        public string JsonResult { get; set; }

        /// <summary>
        /// 操作状态（0正常 1异常）
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "status")]
        public int? Status { get; set; }

        /// <summary>
        /// 错误消息
        /// 默认值:
        ///</summary>
        [SugarColumn(ColumnName = "error_msg")]
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 操作时间
        ///</summary>
        [SugarColumn(ColumnName = "oper_time")]
        public DateTime? OperTime { get; set; }

        /// <summary>
        /// 消耗时间
        /// 默认值: 0
        ///</summary>
        [SugarColumn(ColumnName = "cost_time")]
        public long? CostTime { get; set; }
    }
}