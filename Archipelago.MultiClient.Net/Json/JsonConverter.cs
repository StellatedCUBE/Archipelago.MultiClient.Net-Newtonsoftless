namespace Archipelago.MultiClient.Net.Json
{
	public abstract class JsonConverter
	{
		public abstract object ToObject(JObject data);
		public abstract JObject FromObject(object @object);
	}
}