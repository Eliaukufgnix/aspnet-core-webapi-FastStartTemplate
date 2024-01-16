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
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> logger;
        private readonly LoginService loginService;
        public LoginController(ILogger<LoginController> _logger,LoginService _loginService)
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
            // 生成令牌
            string token = await loginService.Login(loginBodyDTO);
            logger.LogInformation(token);
            return ResultModel<List<TokenVM>>.Success(new List<TokenVM> { new TokenVM { token = token } });
        }

        [HttpPost]
        [Route("loginVerify")]
        public async Task<ResultModel<List<TokenDTO>>> LoginVerify(string token)
        {
            TokenDTO tokenDTO = await loginService.LoginVerify(token);
            return ResultModel<List<TokenDTO>>.Success(new List<TokenDTO> { tokenDTO });
        }
    }
}
