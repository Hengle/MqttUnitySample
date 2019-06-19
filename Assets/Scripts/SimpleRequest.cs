using System;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;

public static class SimpleRequest
{
    public static R Request<R>(string url, object req = null, string authToken = null)
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "MQTTSample-Agent");
        if (!String.IsNullOrEmpty(authToken))
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
        }
        if (req == null)
        {
            req = new object() { };
        }
        var content = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
        var result = client.PostAsync(url, content).Result;
        var json = result.Content.ReadAsStringAsync().Result;
        if (!result.IsSuccessStatusCode)
        {
            throw new Exception(json);
        }
        return JsonConvert.DeserializeObject<R>(json);
    }

    public static string ConvertJson(object o)
    {
        return JsonConvert.SerializeObject(o);
    }

}
