using System;

namespace MixerCore.Authentication
{
    [Serializable]
    public class AuthInfo
    {   
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; } = "Bearer";
        public DateTime Expires { get; set; }   
    }

}