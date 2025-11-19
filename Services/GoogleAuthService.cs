using System.Web;

public class GoogleAuthService : IGoogleAuth
{
    private readonly GoogleAuth _config;
    private readonly HttpClient _http = new HttpClient();
    public GoogleAuthService(GoogleAuth config)
    {
        _config = config;
    }
    public string GenerateGoogleLoginUrl()
    {
        string googleAuthUrl = "https://accounts.google.com/o/oauth2/v2/auth";

        var query = HttpUtility.ParseQueryString(string.Empty);
        query["client_id"] = _config.ClientId;
        query["redirect_uri"] = _config.RedirectUrl;
        query["response_type"] = "code";
        query["scope"] = "openid email profile";
        query["access_type"] = "offline";
        query["prompt"] = "consent";

        return $"{googleAuthUrl}?{query}";
    }

    // 2️⃣ Exchange CODE → Tokens
    public async Task<GoogleTokenResponse?> GetTokensFromCodeAsync(string code)
    {
        string tokenUrl = "https://oauth2.googleapis.com/token";

        var data = new Dictionary<string, string>
        {
            { "client_id", _config.ClientId },
            { "client_secret", _config.ClientSecret },
            { "code", code },
            { "grant_type", "authorization_code" },
            { "redirect_uri", _config.RedirectUrl }
        };

        var res = await _http.PostAsync(tokenUrl, new FormUrlEncodedContent(data));

        if (!res.IsSuccessStatusCode)
            return null;

        return await res.Content.ReadFromJsonAsync<GoogleTokenResponse>();
    }

    // 3️⃣ Use Access Token → Get user email/profile
    public async Task<GoogleUserInfo?> GetGoogleUserAsync(string accessToken)
    {
        string userInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

        var request = new HttpRequestMessage(HttpMethod.Get, userInfoUrl);
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var res = await _http.SendAsync(request);

        if (!res.IsSuccessStatusCode)
            return null;

        return await res.Content.ReadFromJsonAsync<GoogleUserInfo>();
    }

    // 4️⃣ Refresh Token → new Access Token
    public async Task<GoogleTokenResponse?> RefreshGoogleTokenAsync(string refreshToken)
    {
        string tokenUrl = "https://oauth2.googleapis.com/token";

        var data = new Dictionary<string, string>
        {
            { "client_id", _config.ClientId },
            { "client_secret", _config.ClientSecret },
            { "refresh_token", refreshToken },
            { "grant_type", "refresh_token" }
        };

        var res = await _http.PostAsync(tokenUrl, new FormUrlEncodedContent(data));

        if (!res.IsSuccessStatusCode)
            return null;

        return await res.Content.ReadFromJsonAsync<GoogleTokenResponse>();
    }
}