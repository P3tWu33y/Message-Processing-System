using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StackExchange.Redis;
using System.Collections.Generic;

public class IndexModel : PageModel
{
    private readonly IConnectionMultiplexer _redis;

    public IndexModel(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public JsonResult OnGetGetKeys()
    {
        var keys = GetRedisKeys();
        return new JsonResult(keys);
    }

    private IEnumerable<KeyValuePair<string, string>> GetRedisKeys()
    {
        var keyValuePairs = new List<KeyValuePair<string, string>>();

        try
        {
            var endpoints = _redis.GetEndPoints();
            var server = _redis.GetServer(endpoints.First());

            foreach (var key in server.Keys())
            {
                var value = _redis.GetDatabase().StringGet(key);

                keyValuePairs.Add(new KeyValuePair<string, string>(key, value));
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions
            Console.WriteLine(ex.Message);
        }

        return keyValuePairs;
    }

}
