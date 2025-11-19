public interface IGoogleAuth
{
    public string GenerateGoogleLoginUrl();
    Task<GoogleTokenResponse?> GetTokensFromCodeAsync(string code);
    Task<GoogleUserInfo?> GetGoogleUserAsync(string accessToken);
    Task<GoogleTokenResponse?> RefreshGoogleTokenAsync(string refreshToken);
}