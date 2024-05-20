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
        /// accessToken密钥
        /// </summary>
        public const string AccessTokenSecret = JWTConstants.AccessTokenSecret;

        /// <summary>
        /// refreshToken密钥
        /// </summary>
        public const string RefreshTokenSecret = JWTConstants.RefreshTokenSecret;

        #region 生成token

        /// <summary>
        /// 生成双token
        /// </summary>
        /// <param name="tokenDTO"></param>
        /// <returns>双token</returns>
        public static (string accessToken, string refreshToken) GenerateToken(TokenDTO tokenDTO)
        {
            string accessToken = GenerateToken(tokenDTO, DateTime.Now.AddSeconds(120), AccessTokenSecret);
            string refreshToken = GenerateToken(tokenDTO, DateTime.Now.AddDays(7), RefreshTokenSecret);
            return (accessToken, refreshToken);
        }

        public static string GenerateToken(TokenDTO tokenDTO, DateTime expireTime, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secret);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = Issuer,
                Audience = Audience,
                Expires = expireTime,
                SigningCredentials = signingCredentials
            };
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                    new Claim("userId", tokenDTO.UserId.ToString()),
                    new Claim("username", tokenDTO.UserName),
                    new Claim("roleId", tokenDTO.RoleId.ToString())
                });
            tokenDescriptor.Subject = claimsIdentity;

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        #endregion 生成token

        #region 验证refreshToken

        /// <summary>
        /// 验证refreshToken
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public static bool ValidateRefreshToken(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(RefreshTokenSecret);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,// 验证发行人的签名密钥
                IssuerSigningKey = new SymmetricSecurityKey(key),// 提供用于验证签名的密钥
                ValidateIssuer = true,// 验证 token 的Issuer
                ValidIssuer = Issuer,// 指定有效的Issuer
                ValidateAudience = true,// 验证 token 的Audience
                ValidAudience = Audience,
            };

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        #endregion 验证refreshToken

        #region 解析token

        /// <summary>
        /// 解析token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static TokenDTO ParsingToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(RefreshTokenSecret);
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

            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            // 提取 token 信息
            long userId = Convert.ToInt64(principal.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);
            string? userName = principal.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
            long roleId = Convert.ToInt64(principal.Claims.FirstOrDefault(c => c.Type == "roleId")?.Value);
            // 创建 TokenDTO 对象
            return new TokenDTO
            {
                UserId = userId,
                UserName = userName,
                RoleId = roleId
            };
        }

        #endregion 解析token
    }
}