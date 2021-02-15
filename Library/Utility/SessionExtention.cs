using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Library.Utility
{
    public static class SessionExtention
    {
        public static void Set<T>(this ISession session,string key,T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static L Get<L>(this ISession session,string key)
        {
            var value = session.GetString(key);
            return value is null ? default : JsonSerializer.Deserialize<L>(value);
        }

    }
}
