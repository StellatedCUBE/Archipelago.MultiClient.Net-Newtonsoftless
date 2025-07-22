using System;

namespace Archipelago.MultiClient.Net.Json
{
	[AttributeUsage(AttributeTargets.Enum, Inherited = true)]
	public class JsonStringEnumAttribute : Attribute
	{
		public readonly bool snakeCase = false;

		public JsonStringEnumAttribute() {}
		public JsonStringEnumAttribute(bool snakeCase)
		{
			this.snakeCase = snakeCase;
		}
	}
}