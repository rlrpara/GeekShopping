using System.Net.Http.Headers;
using System.Text.Json;

namespace GeekShopping.Web.Utils;

public static class HttpClientExtensions
{
    #region [Private Properties]
    private static MediaTypeHeaderValue _contentType = new("application/json");
    #endregion


    #region [Public Methods]
    public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
            throw new ApplicationException($"Something wrong calling in API: {response.ReasonPhrase}");

        var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        var jsonSerializeOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<T>(dataAsString, jsonSerializeOptions);
    }
    public static async Task<HttpResponseMessage> PostAsJson<T>(this HttpClient httpClient, string url, T data)
    {
        var dataAsString = JsonSerializer.Serialize(data);

        var content = new StringContent(dataAsString);
        content.Headers.ContentType = _contentType;

        return await httpClient.PostAsync(url, content);

    }

    public static async Task<HttpResponseMessage> PutAsJson<T>(this HttpClient httpClient, string url, T data)
    {
        var dataAsString = JsonSerializer.Serialize(data);

        var content = new StringContent(dataAsString);
        content.Headers.ContentType = _contentType;

        return await httpClient.PutAsync(url, content);

    }
    #endregion
}
