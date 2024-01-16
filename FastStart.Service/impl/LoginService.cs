using FastStart.Common.Exception;
using FastStart.Common.Utils;
using FastStart.Domain.Constant;
using FastStart.Domain.Models;

namespace FastStart.Service.impl
{
    public class LoginService
    {
        #region 依赖注入

        public readonly ISysUserService sysUserService;

        public LoginService(ISysUserService _sysUserService)
        {
            sysUserService = _sysUserService;
        }

        #endregion 依赖注入

        public async Task<string> Login(LoginBodyDTO loginBodyDTO)
        {
            // 登录前置校验
            LoginPreCheck(loginBodyDTO.username, loginBodyDTO.password);
            var sysUser = await sysUserService.GetEntityByWhereAsync(x => x.UserName == loginBodyDTO.username) ?? throw new AccountOrPassWordException();
            // 生成token
            return TokenUtils.GenerateToken(new TokenDTO { UserId = sysUser.UserId, UserName = sysUser.UserName });
        }

        public async Task<TokenDTO> LoginVerify(string token)
        {
            TokenDTO tokenDTO = TokenUtils.ValidateToken(token);
            return tokenDTO;
        }
            /// <summary>
            /// 登录前置校验
            /// </summary>
            /// <param name="username">用户名</param>
            /// <param name="password">密码</param>
            /// <exception cref="Exception"></exception>
            public void LoginPreCheck(string username, string password)
        {
            // 用户名或密码为空 错误
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                //throw new Exception("用户名或密码为空");
                throw new AccountOrPassWordException(301, ErrorMessage.userNameOrPasswordIsEmpty);
            }

            // 密码如果不在指定范围内 错误
            if (password.Length < UserConstants.PASSWORD_MIN_LENGTH)
            {
                //throw new Exception("用户不存在或密码错误");
                throw new AccountOrPassWordException();
            }

            // 用户名不在指定范围内 错误
            if (username.Length < UserConstants.USERNAME_MIN_LENGTH || username.Length > UserConstants.USERNAME_MAX_LENGTH)
            {
                //throw new Exception("用户不存在或密码错误");
                throw new AccountOrPassWordException();
            }
            //TODO IP黑名单校验
        }
    }
}