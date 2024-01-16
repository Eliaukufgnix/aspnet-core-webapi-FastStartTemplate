using FastStart.Domain.Constant;
using Microsoft.AspNetCore.Http;

namespace FastStart.Common.Exception
{
    public class AccountOrPassWordException : BaseException
    {
        private const int DefaultCode = StatusCodes.Status401Unauthorized;
        private new const string DefaultMessage = ErrorMessage.userDoesNotExistOrPasswordIsIncorrect;

        // 无参数构造函数
        public AccountOrPassWordException()
            : base(null, DefaultCode, null, DefaultMessage)
        {
        }

        public AccountOrPassWordException(string defaultMessage) : base(defaultMessage)
        {
        }

        public AccountOrPassWordException(int code, string defaultMessage) : base(code, defaultMessage)
        {
        }
    }
}