namespace NewInterlex.Api.Helpers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public sealed class JsonSerializer
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
//            NullValueHandling = NullValueHandling.Ignore
        };

        public static string SerializeObject(object o)
        {
            return JsonConvert.SerializeObject(o, Formatting.Indented, Settings);
        }
    }
}