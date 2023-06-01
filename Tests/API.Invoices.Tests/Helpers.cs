using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace API.Invoices.Tests;
internal class Helpers
{
    public static async Task<string?> GetAuthCode(string client_id, string client_secret, string baseAddress)
    {
        var client = new HttpClient() { BaseAddress = new Uri(baseAddress) };

        var contentKeyPairs = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", client_id },
            { "client_secret", client_secret }
        };

        var content = new FormUrlEncodedContent(contentKeyPairs);

        var response = await client.PostAsync("/connect/token", content);

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenResult>(json);
            return token?.Token;
        }
        else
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new Exception($"GetAuthCode failed.  Path:{client_id}.  Message: {message}");
        }
    }

    public class TokenResult
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }

    public static async Task<T2?> PostAsync<T1, T2>(T1 requestDto, string uri, HttpClient httpClient)
    {
        var json = JsonConvert.SerializeObject(requestDto);

        var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

        var response = await httpClient.PostAsync(uri, content);

        if (response.IsSuccessStatusCode)
        {
            var jsonResult = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T2>(jsonResult);
        }
        else
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new Exception($"Post Async failed.  Path:{uri}.  Message: {message}");
        }
    }

    public static async Task PostAsync<T1>(T1 requestDto, string uri, HttpClient httpClient)
    {
        var json = JsonConvert.SerializeObject(requestDto);

        var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);

        var response = await httpClient.PostAsync(uri, content);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new Exception($"Post Async failed.  Path:{uri}.  Message: {message}");
        }
    }

    public static async Task<T?> GetAsync<T>(string path, HttpClient httpClient)
    {
        if (string.IsNullOrWhiteSpace(path))
            return default;

        var response = await httpClient.GetAsync(path);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            T result = JsonConvert.DeserializeObject<T>(content);

            return result;
        }
        else
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new Exception($"Get Async failed.  Path:{path}.  Message: {message}");
        }
    }

    internal static async Task<bool> DeleteAsync(string path, HttpClient httpClient)
    {
        if (string.IsNullOrWhiteSpace(path))
            return default;

        var response = await httpClient.DeleteAsync(path);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new Exception($"Delete Async failed.  Path:{path}.  Message: {message}");
        }
    }
}
