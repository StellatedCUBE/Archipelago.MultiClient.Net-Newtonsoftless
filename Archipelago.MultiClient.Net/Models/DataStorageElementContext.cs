using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Json;
using System;

#if !NET35
using System.Threading.Tasks;
#endif

namespace Archipelago.MultiClient.Net.Models
{
    class DataStorageElementContext
    {
        internal string Key { get; set; }

        internal Action<string, DataStorageHelper.DataStorageUpdatedHandler> AddHandler { get; set; }
        internal Action<string, DataStorageHelper.DataStorageUpdatedHandler> RemoveHandler { get; set; }
        internal Func<string, JObject> GetData { get; set; }
        internal Action<string, JObject> Initialize { get; set; }
#if NET35
        internal Action<string, Action<JObject>> GetAsync { get; set; }
#else
        internal Func<string, Task<JObject>> GetAsync { get; set; }
#endif

        public override string ToString() => $"Key: {Key}";
    }
}