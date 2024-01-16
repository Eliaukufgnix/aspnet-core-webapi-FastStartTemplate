using FastStart.Domain.Constant;
using FastStart.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FastStart.Common.Utils
{
    public static class TokenUtils
    {
        /// <summary>
        /// 颁发者
        /// </summary>
        public const string Issuer = JWTConstants.Issuer;

        /// <summary>
        /// 接收者
        /// </summary>
        public const string Audience = JWTConstants.Audience;

        /// <summary>
        /// 密钥
        /// </summary>
        public const string Secret = JWTConstants.Secret;

        #region 生成token

        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="tokenDTO">JWT加密信息</param>
        /// <returns>token</returns>
        public static string GenerateToken(TokenDTO tokenDTO)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Secret);
            var claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Sid, tokenDTO.UserId.ToString()),
                new Claim(ClaimTypes.Name, tokenDTO.UserName)
            });
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = Issuer,
                Audience = Audience,
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = signingCredentials,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        #endregion 生成token

        #region 解析token
        /// <summary>
        /// 解析token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static TokenDTO ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Secret);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,// 验证发行人的签名密钥
                IssuerSigningKey = new SymmetricSecurityKey(key),// 提供用于验证签名的密钥
                ValidateIssuer = true,// 验证 token 的Issuer
                ValidIssuer = Issuer,// 指定有效的Issuer
                ValidateAudience = true,// 验证 token 的Audience
                ValidAudience = Audience,// 指定有效的Audience
                ValidateLifetime = true, // 验证 token 的有效期，确保 token 没有过期
                ClockSkew = TimeSpan.Zero // 减少默认的时间偏差，用于生命周期验证
            };

            SecurityToken validatedToken;
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            // 提取 token 信息
            long userId = Convert.ToInt64(principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value);
            string? userName = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            //string? roleName = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            // 创建 TokenDTO 对象
            return new TokenDTO
            {
                UserId = userId,
                UserName = userName,
                //RoleName = roleName
            };
        }
        #endregion
    }
}