using System;

namespace Archipelago.MultiClient.Net.Json
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
	public class JsonPropertyAttribute : Attribute
	{
		internal string propertyName;

		public JsonPropertyAttribute(string propertyName)
		{
			this.propertyName = propertyName;
		}
	}
}