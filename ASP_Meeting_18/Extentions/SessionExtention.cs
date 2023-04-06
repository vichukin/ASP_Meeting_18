using System.Text.Json;
using System.Text.Json.Serialization;

namespace ASP_Meeting_18.Extentions
{
    public static class SessionExtention
    {
        public static void Set<T>(this ISession session,string key, T value)
        {
            var option = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };
            session.SetString(key,JsonSerializer.Serialize(value,option));
        }
        public static T? Get<T>(this ISession session, string key)
        {
            var option = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };
            string str = session.GetString(key);
            return str != null ? JsonSerializer.Deserialize<T>(str,option):default(T);
        }
    }
}
