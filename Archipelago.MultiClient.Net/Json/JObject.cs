using Archipelago.MultiClient.Net.Converters;
using Archipelago.MultiClient.Net.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Archipelago.MultiClient.Net.Json
{
	public class JObject
	{
		readonly object inner;

		JObject(object inner)
		{
			this.inner = inner;
		}

		public static JObject FromJSON(string json) => new JObject(MiniJson.Deserialize(json));

		public string ToJSON() => MiniJson.Serialize(inner);

		public static explicit operator int(JObject @object) => Convert.ToInt32(@object.inner);
		public static explicit operator string(JObject @object) => @object.ToString();
		public static explicit operator bool(JObject @object) => (bool)@object.inner;

		public override string ToString() => inner?.ToString() ?? "null";

		public JObjectType Type =>
			inner == null ? JObjectType.Null :
			inner is string ? JObjectType.String :
			inner is bool ? JObjectType.Boolean :
			inner is List<object> ? JObjectType.Array :
			inner is Dictionary<string, object> ? JObjectType.Object :
			JObjectType.Number;
		
		public void Merge(JObject other)
		{
			var self = (Dictionary<string, object>)inner;
			foreach (var pair in (Dictionary<string, object>)other.inner)
			{
				self[pair.Key] = pair.Value;
			}
		}

		public bool TryGetValue(string key, out JObject value)
		{
			if (inner is Dictionary<string, object> dictionary && dictionary.TryGetValue(key, out var innerValue))
			{
				value = new JObject(innerValue);
				return true;
			}
			else
			{
				value = null;
				return false;
			}
		}

		public JObject this[string key]
		{
			get {
				TryGetValue(key, out var value);
				return value;
			}
			set {
				if (inner is Dictionary<string, object> dictionary)
				{
					dictionary[key] = FromObject(value);
				}
			}
		}

		static string SnakeCase(string titleCase)
		{
			var sb = new StringBuilder();

			foreach (char c in titleCase)
			{
				if (char.IsUpper(c))
				{
					if (sb.Length > 0)
					{
						sb.Append('_');
					}

					sb.Append(new string(c, 1).ToLowerInvariant());
				}
				else
				{
					sb.Append(c);
				}
			}

			return sb.ToString();
		}

		public T ToObject<T>(StreamingContext context = default) => (T)ToObjectDyn(typeof(T), context);

		public object ToObjectDyn(Type t, StreamingContext context)
		{
			#if JSON_DEBUG
			Console.WriteLine($"Deserializing a {t.Name} from {ToJSON()}");
			#endif

			if (t.IsAssignableFrom(inner.GetType()))
			{
				return inner;
			}
			else if (t == typeof(Permissions))
			{
				return PermissionsEnumConverter.instance.ToObject(this);
			}
			else if (t == typeof(ArchipelagoPacketBase))
			{
				return ArchipelagoPacketConverter.instance.ToObject(this);
			}
			else if (t.IsEnum)
			{
				try
				{
					return Enum.Parse(t, inner.ToString().Replace("_", ""), true);
				}
				catch (ArgumentException)
				{
					return Enum.Parse(t, inner.ToString(), true);
				}
			}
			else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				if (inner == null)
				{
					return null;
				}
				else
				{
					return ToObjectDyn(t.GetGenericArguments()[0], context);
				}
			}
			else if (t.IsValueType && typeof(IConvertible).IsAssignableFrom(t))
			{
				return Convert.ChangeType(inner, t);
			}
			else if (t == typeof(JObject))
			{
				return this;
			}
			else if (inner == null)
			{
				return null;
			}
			else if (t.IsArray)
			{
				var list = (List<object>)inner;
				var array = Array.CreateInstance(t.GetElementType(), list.Count);

				for (int i = 0; i < array.Length; i++)
				{
					array.SetValue(new JObject(list[i]).ToObjectDyn(t.GetElementType(), context), i);
				}

				return array;
			}
			else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
			{
				var itemType = t.GetGenericArguments()[0];
				var list = Activator.CreateInstance(t);
				var add = t.GetMethod("Add");

				foreach (var item in (List<object>)inner)
				{
					add.Invoke(list, new object[] { new JObject(item).ToObjectDyn(itemType, context) });
				}

				return list;
			}
			else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>))
			{
				var keyType = t.GetGenericArguments()[0];
				var valueType = t.GetGenericArguments()[1];
				var dictionary = Activator.CreateInstance(t);
				var add = t.GetMethod("Add");

				foreach (var pair in (Dictionary<string, object>)inner)
				{
					add.Invoke(dictionary, new object[] { new JObject(pair.Key).ToObjectDyn(keyType, context), new JObject(pair.Value).ToObjectDyn(valueType, context) });
				}

				return dictionary;
			}
			else
			{
				var @object = Activator.CreateInstance(t);
				var dictionary = (Dictionary<string, object>)inner;

				foreach (var field in t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
				{
					if (Attribute.GetCustomAttribute(field, typeof(JsonIgnoreAttribute)) != null)
					{
						continue;
					}

					if (Attribute.GetCustomAttribute(field, typeof(JsonExtensionDataAttribute)) != null)
					{
						var extension = new Dictionary<string, JObject>();

						foreach (var pair in dictionary)
						{
							extension.Add(pair.Key, new JObject(pair.Value));
						}

						field.SetValue(@object, extension);
						continue;
					}

					string key;

					if (Attribute.GetCustomAttribute(field, typeof(JsonPropertyAttribute)) is JsonPropertyAttribute jpa)
					{
						key = jpa.propertyName;
					}
					else
					{
						key = field.Name;
					}

					if (dictionary.TryGetValue(key, out var value))
					{
						field.SetValue(@object, new JObject(value).ToObjectDyn(field.FieldType, context));
					}
					else if (dictionary.TryGetValue(SnakeCase(key), out value))
					{
						field.SetValue(@object, new JObject(value).ToObjectDyn(field.FieldType, context));
					}
				}

				foreach (var property in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
				{
					if (Attribute.GetCustomAttribute(property, typeof(JsonIgnoreAttribute)) != null)
					{
						continue;
					}

					if (Attribute.GetCustomAttribute(property, typeof(JsonExtensionDataAttribute)) != null)
					{
						var extension = new Dictionary<string, JObject>();

						foreach (var pair in dictionary)
						{
							extension.Add(pair.Key, new JObject(pair.Value));
						}

						try
						{
							property.SetValue(@object, extension, null);
						}
						catch (Exception e)
						{
							#if JSON_DEBUG
							Console.WriteLine($"Unable to write to {t.Name}.{property.Name}: {e.Message}");
							#endif
						}
						continue;
					}

					string key;

					if (Attribute.GetCustomAttribute(property, typeof(JsonPropertyAttribute)) is JsonPropertyAttribute jpa)
					{
						key = jpa.propertyName;
					}
					else
					{
						key = property.Name;
					}

					if (dictionary.TryGetValue(key, out var value))
					{
						var propertyValue = new JObject(value).ToObjectDyn(property.PropertyType, context);
						try
						{
							property.SetValue(@object, propertyValue, null);
						}
						catch (Exception e)
						{
							#if JSON_DEBUG
							Console.WriteLine($"Unable to write to {t.Name}.{property.Name}: {e.Message}");
							#endif
						}
					}
					else if (dictionary.TryGetValue(SnakeCase(key), out value))
					{
						var propertyValue = new JObject(value).ToObjectDyn(property.PropertyType, context);
						try
						{
							property.SetValue(@object, propertyValue, null);
						}
						catch (Exception e)
						{
							#if JSON_DEBUG
							Console.WriteLine($"Unable to write to {t.Name}.{property.Name}: {e.Message}");
							#endif
						}
					}
				}

				foreach (var method in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
				{
					if (Attribute.GetCustomAttribute(method, typeof(OnDeserializedAttribute)) != null)
					{
						method.Invoke(@object, new object[] { context });
						break;
					}
				}

				return @object;
			}
		}

		public static JObject FromObject(object @object, bool dropNulls = false)
		{
			if (
				@object == null ||
				@object is string ||
				@object is bool ||
				@object is byte ||
				@object is sbyte ||
				@object is short ||
				@object is ushort ||
				@object is int ||
				@object is uint ||
				@object is long ||
				@object is ulong ||
				@object is float ||
				@object is double ||
				@object is decimal
			)
			{
				return new JObject(@object);
			}

			else if (@object is JObject jObject)
			{
				return jObject;
			}

			else if (@object is Permissions)
			{
				return PermissionsEnumConverter.instance.FromObject(@object);
			}

			else if (@object.GetType().IsEnum)
			{
				var stringAttribute = (JsonStringEnumAttribute)Attribute.GetCustomAttribute(@object.GetType(), typeof(JsonStringEnumAttribute));
				if (stringAttribute == null)
				{
					return new JObject(Convert.ToInt32(@object));
				}
				else if (stringAttribute.snakeCase)
				{
					return new JObject(SnakeCase(@object.ToString()));
				}
				else
				{
					return new JObject(@object.ToString());
				}
			}

			else if (@object.GetType().IsGenericType && @object.GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
			{
				var dictionary = new Dictionary<string, object>();
				PropertyInfo key = null, value = null;
				foreach (var pair in (IEnumerable)@object)
				{
					if (key == null)
					{
						key = pair.GetType().GetProperty("Key");
						value = pair.GetType().GetProperty("Value");
					}

					dictionary.Add(
						key.GetValue(pair, null).ToString(),
						FromObject(value.GetValue(pair, null), dropNulls).inner
					);
				}
				if (dropNulls)
				{
					foreach (var pair in dictionary.ToList())
					{
						if (pair.Value == null)
						{
							dictionary.Remove(pair.Key);
						}
					}
				}
				return new JObject(dictionary);
			}

			else if (@object is IEnumerable enumerable)
			{
				var list = new List<object>();
				foreach (var item in enumerable)
				{
					list.Add(FromObject(item, dropNulls).inner);
				}
				return new JObject(list);
			}

			else
			{
				var dictionary = new Dictionary<string, object>();
				Dictionary<string, JObject> extension = null;

				foreach (var field in @object.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
				{
					if (field.Name[0] == '<' || Attribute.GetCustomAttribute(field, typeof(JsonIgnoreAttribute)) != null)
					{
						continue;
					}

					var value = field.GetValue(@object);

					if (Attribute.GetCustomAttribute(field, typeof(JsonExtensionDataAttribute)) != null)
					{
						extension = (Dictionary<string, JObject>)value;
						continue;
					}

					string key;

					if (Attribute.GetCustomAttribute(field, typeof(JsonPropertyAttribute)) is JsonPropertyAttribute jpa)
					{
						key = jpa.propertyName;
					}
					else
					{
						key = field.Name;
					}

					dictionary.Add(key, FromObject(value, dropNulls).inner);
				}

				foreach (var property in @object.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
				{
					if (Attribute.GetCustomAttribute(property, typeof(JsonIgnoreAttribute)) != null)
					{
						continue;
					}

					var value = property.GetValue(@object, null);

					if (Attribute.GetCustomAttribute(property, typeof(JsonExtensionDataAttribute)) != null)
					{
						extension = (Dictionary<string, JObject>)value;
						continue;
					}

					string key;

					if (Attribute.GetCustomAttribute(property, typeof(JsonPropertyAttribute)) is JsonPropertyAttribute jpa)
					{
						key = jpa.propertyName;
					}
					else
					{
						key = property.Name;
					}

					dictionary.Add(key, FromObject(value, dropNulls).inner);
				}

				if (extension != null)
				{
					foreach (var pair in extension)
					{
						if (!dictionary.ContainsKey(pair.Key))
						{
							dictionary.Add(pair.Key, pair.Value.inner);
						}
					}
				}

				if (dropNulls)
				{
					foreach (var pair in dictionary.ToList())
					{
						if (pair.Value == null)
						{
							dictionary.Remove(pair.Key);
						}
					}
				}

				return new JObject(dictionary);
			}
		}
	}
}