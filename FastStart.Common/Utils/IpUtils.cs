namespace FastStart.Common.Utils
{
    public class IpUtils
    {
        public static string REGX_0_255 = "(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]\\d|\\d)";

        /// <summary>
        ///  匹配 ip
        /// </summary>
        public static string REGX_IP = "((" + REGX_0_255 + "\\.){3}" + REGX_0_255 + ")";

        public static string REGX_IP_WILDCARD = "(((\\*\\.){3}\\*)|(" + REGX_0_255 + "(\\.\\*){3})|(" + REGX_0_255 + "\\." + REGX_0_255 + ")(\\.\\*){2}" + "|((" + REGX_0_255 + "\\.){3}\\*))";

        /// <summary>
        /// 匹配网段
        /// </summary>
        public static string REGX_IP_SEG = "(" + REGX_IP + "\\-" + REGX_IP + ")";
    }
}