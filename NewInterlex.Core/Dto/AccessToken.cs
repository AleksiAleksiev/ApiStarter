namespace NewInterlex.Core.Dto
{
    public sealed class AccessToken
    {
        public string Token { get; set; }

        public int ExpiresIn { get; set; }

        public AccessToken(string token, int expiresIn)
        {
            this.Token = token;
            this.ExpiresIn = expiresIn;
        }
        
    }
}