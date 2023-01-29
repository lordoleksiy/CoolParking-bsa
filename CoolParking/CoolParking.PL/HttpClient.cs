
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ServiceStack;

namespace CoolParking.PL;

internal class Client
{
    private string baseUrl = "https://localhost:7043/api/";
    private readonly HttpClient _client;

    internal Client() 
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(baseUrl);
    }
    public T? Get<T>(string url)
    {
        var resp = _client.GetAsync(url);

        resp.Wait();
        if (resp.Result.IsSuccessStatusCode)
        {
            return resp.Result.Content.ReadFromJsonAsync<T>().Result;
        }
        Console.WriteLine($"{resp.Result.StatusCode}: {resp.Result.Content.ReadAsStringAsync().Result}");
        return default;
    }

    public string Get(string url)
    {
        var resp = _client.GetStringAsync(url);
        return resp.Result;
    }

    public T? Post<T>(string url, T obj)
    {
        var jsonObject = JsonSerializer.Serialize(obj);
        var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
        
        var resp = _client.PostAsync(url, content);

        resp.Wait();
        if (resp.Result.IsSuccessStatusCode)
        {
            return resp.Result.Content.ReadFromJsonAsync<T>().Result;
        }
        Console.WriteLine($"{resp.Result.StatusCode}: {resp.Result.Content.ReadAsStringAsync().Result}");
        return default;
    }

    public T? Put<T> (string url, T obj) 
    { 
        var jsonObject = JsonSerializer.Serialize(obj);
        var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");

        var resp = _client.PutAsync(url, content);

        resp.Wait();
        if (resp.Result.IsSuccessStatusCode)
        {
            return resp.Result.Content.ReadFromJsonAsync<T>().Result;
        }
        Console.WriteLine($"{resp.Result.StatusCode}: {resp.Result.Content.ReadAsStringAsync().Result}");
        return default;
    }

    public string Delete(string url)
    {
        var resp = _client.DeleteAsync(url);

        resp.Wait();
        if (resp.Result.IsSuccessStatusCode)
        {
            return "Succesfully deleted!";
        }
        return $"{resp.Result.StatusCode}: {resp.Result.Content.ReadAsStringAsync().Result}";
    }
}