using System.Net.Http.Headers;
namespace MatrixMultiplicationConsoleApp;

internal class NumbersServiceConsumer
{
    private readonly HttpClient _httpClient;

    public NumbersServiceConsumer(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://recruitment-test.investcloud.com/");
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<string> InitDatasetsAsync(int size)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/numbers/init/{size}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetDataAsync(string dataset, string type, int idx)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"api/numbers/{dataset}/{type}/{idx}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> ValidateCalculationAsync(string data)
    {
        var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync("api/numbers/validate", content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}


