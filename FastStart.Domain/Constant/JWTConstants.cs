namespace FastStart.Domain.Constant
{
    public class JWTConstants
    {
        /// <summary>
        /// 颁发者
        /// </summary>
        public const string Issuer = "Eliaukufgnix";

        /// <summary>
        /// 接收者
        /// </summary>
        public const string Audience = "Eliaukufgnix";

        /// <summary>
        /// accessToken密钥
        /// </summary>
        /// Bujing123... base 64 5次
        public const string AccessTokenSecret = "Vmxaak1WWXlUbGhTYkdoUFZucFdUMVpyVm5OT2JGSklZWHBDYTFWVU1Eaz0=";

        /// <summary>
        /// refreshToken密钥
        /// </summary>
        /// Eliaukufgnix base 64 5次
        public const string RefreshTokenSecret = "VmxkNGEwNUhUa2RpUm14WFltdHdjbFpxUm5ka1ZteDBUVmhPVDFGVU1Eaz0=";
    }
}