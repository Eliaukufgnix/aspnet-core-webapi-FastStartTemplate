using FastStart.Domain;
using FastStart.Domain.Models;
using FastStart.Service.impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastStart.WebApi.Controllers
{
    /// <summary>
    /// 用户登录
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Login")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> logger;
        private readonly LoginService loginService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_logger"></param>
        /// <param name="_loginService"></param>
        public LoginController(ILogger<LoginController> _logger, LoginService _loginService)
        {
            logger = _logger;
            loginService = _loginService;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginBodyDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ResultModel<List<TokenVM>>> Login([FromBody] LoginBodyDTO loginBodyDTO)
        {
            TokenVM tokenVM = await loginService.Login(loginBodyDTO);
            return ResultModel<List<TokenVM>>.Success(new List<TokenVM> { tokenVM });
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("loginout")]
        [AllowAnonymous]
        public ResultModel<List<string>> LoginOut()
        {
            return ResultModel<List<string>>.Success();
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="registerBodyDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<ResultModel<bool>> Register([FromBody] RegisterBodyDTO registerBodyDTO)
        {
            bool result = await loginService.Register(registerBodyDTO);
            return result ? ResultModel<bool>.Success("注册成功", true) : ResultModel<bool>.Fail("注册失败", false);
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshToken">刷新token</param>
        /// <returns></returns>
        [HttpPost]
        [Route("refreshToken")]
        public async Task<ResultModel<List<TokenVM>>> RefreshToken(string refreshToken)
        {
            TokenVM tokenVM = await LoginService.RefreshToken(refreshToken);
            return ResultModel<List<TokenVM>>.Success(new List<TokenVM> { tokenVM });
        }
    }
}