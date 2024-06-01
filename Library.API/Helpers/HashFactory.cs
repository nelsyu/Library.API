using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Library.API.Helpers
{
    public class HashFactory
    {
        public static string GetHash(object entity)
        {
            string result = string.Empty;

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(entity, jsonSettings);
            var bytes = Encoding.UTF8.GetBytes(json);

            using (var hasher = MD5.Create())
            {
                var hash = hasher.ComputeHash(bytes);
                result = BitConverter.ToString(hash);
                result = result.Replace("-", "");
            }

            return result;
        }
    }
}
