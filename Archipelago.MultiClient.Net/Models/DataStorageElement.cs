using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if !NET35
using System.Threading.Tasks;
using System.Numerics;
using System.Globalization;
#endif

// ReSharper disable ArrangeObjectCreationWhenTypeEvident

namespace Archipelago.MultiClient.Net.Models
{
    /// <summary>
    /// An entry in the DataStorage
    /// </summary>
    public class DataStorageElement
    {
        /// <summary>
        /// Event handler will be called when the server side value for this key changes
        /// </summary>
        public event DataStorageHelper.DataStorageUpdatedHandler OnValueChanged
        {
            add => Context.AddHandler(Context.Key, value);
            remove => Context.RemoveHandler(Context.Key, value);
        }

		internal DataStorageElementContext Context;
        internal List<OperationSpecification> Operations = new List<OperationSpecification>(0);
        internal DataStorageHelper.DataStorageUpdatedHandler Callbacks;
        internal Dictionary<string, JObject> AdditionalArguments = new Dictionary<string, JObject>(0);

		JObject cachedValue;

        internal DataStorageElement(DataStorageElementContext context)
        {
            Context = context;
        }
        internal DataStorageElement(OperationType operationType, JObject value)
        {
			Operations = new List<OperationSpecification>(1) {
                new OperationSpecification { OperationType = operationType, Value = value }
            };
        }
        internal DataStorageElement(DataStorageElement source, OperationType operationType, JObject value) : this(source.Context)
        {
            Operations = source.Operations.ToList();
            Callbacks = source.Callbacks;
            AdditionalArguments = source.AdditionalArguments;

            Operations.Add(new OperationSpecification { OperationType = operationType, Value = value });
		}
		internal DataStorageElement(DataStorageElement source, Callback callback) : this(source.Context)
        {
            Operations = source.Operations.ToList();
            Callbacks = source.Callbacks;
            AdditionalArguments = source.AdditionalArguments;

			Callbacks += callback.Method;
        }
		internal DataStorageElement(DataStorageElement source, AdditionalArgument additionalArgument) : this(source.Context)
		{
			Operations = source.Operations.ToList();
			Callbacks = source.Callbacks;
			AdditionalArguments = source.AdditionalArguments;

			AdditionalArguments[additionalArgument.Key] = additionalArgument.Value;
		}

#pragma warning disable CS1591
		public static DataStorageElement operator ++(DataStorageElement a) => new DataStorageElement(a, OperationType.Add, JObject.FromObject(1));
        public static DataStorageElement operator --(DataStorageElement a) => new DataStorageElement(a, OperationType.Add, JObject.FromObject(-1));
		public static DataStorageElement operator +(DataStorageElement a, int b) => new DataStorageElement(a, OperationType.Add, JObject.FromObject(b));
        public static DataStorageElement operator +(DataStorageElement a, long b) => new DataStorageElement(a, OperationType.Add, JObject.FromObject(b));
		public static DataStorageElement operator +(DataStorageElement a, float b) => new DataStorageElement(a, OperationType.Add, JObject.FromObject(b));
        public static DataStorageElement operator +(DataStorageElement a, double b) => new DataStorageElement(a, OperationType.Add, JObject.FromObject(b));
        public static DataStorageElement operator +(DataStorageElement a, decimal b) => new DataStorageElement(a, OperationType.Add, JObject.FromObject(b));
		public static DataStorageElement operator +(DataStorageElement a, string b) => new DataStorageElement(a, OperationType.Add, JObject.FromObject(b));
		public static DataStorageElement operator +(DataStorageElement a, JObject b) => new DataStorageElement(a, OperationType.Add, b);
		public static DataStorageElement operator +(DataStorageElement a, IEnumerable b) => new DataStorageElement(a, OperationType.Add, JObject.FromObject(b));
        public static DataStorageElement operator +(DataStorageElement a, OperationSpecification s) => new DataStorageElement(a, s.OperationType, s.Value);
        public static DataStorageElement operator +(DataStorageElement a, Callback c) => new DataStorageElement(a, c);
        public static DataStorageElement operator +(DataStorageElement a, AdditionalArgument arg) => new DataStorageElement(a, arg);
		public static DataStorageElement operator *(DataStorageElement a, int b) => new DataStorageElement(a, OperationType.Mul, JObject.FromObject(b));
        public static DataStorageElement operator *(DataStorageElement a, long b) => new DataStorageElement(a, OperationType.Mul, JObject.FromObject(b));
        public static DataStorageElement operator *(DataStorageElement a, float b) => new DataStorageElement(a, OperationType.Mul, JObject.FromObject(b));
        public static DataStorageElement operator *(DataStorageElement a, double b) => new DataStorageElement(a, OperationType.Mul, JObject.FromObject(b));
        public static DataStorageElement operator *(DataStorageElement a, decimal b) => new DataStorageElement(a, OperationType.Mul, JObject.FromObject(b));
		public static DataStorageElement operator %(DataStorageElement a, int b) => new DataStorageElement(a, OperationType.Mod, JObject.FromObject(b));
        public static DataStorageElement operator %(DataStorageElement a, long b) => new DataStorageElement(a, OperationType.Mod, JObject.FromObject(b));
        public static DataStorageElement operator %(DataStorageElement a, float b) => new DataStorageElement(a, OperationType.Mod, JObject.FromObject(b));
        public static DataStorageElement operator %(DataStorageElement a, double b) => new DataStorageElement(a, OperationType.Mod, JObject.FromObject(b));
        public static DataStorageElement operator %(DataStorageElement a, decimal b) => new DataStorageElement(a, OperationType.Mod, JObject.FromObject(b));
		public static DataStorageElement operator ^(DataStorageElement a, int b) => new DataStorageElement(a, OperationType.Pow, JObject.FromObject(b));
		public static DataStorageElement operator ^(DataStorageElement a, long b) => new DataStorageElement(a, OperationType.Pow, JObject.FromObject(b));
		public static DataStorageElement operator ^(DataStorageElement a, float b) => new DataStorageElement(a, OperationType.Pow, JObject.FromObject(b));
		public static DataStorageElement operator ^(DataStorageElement a, double b) => new DataStorageElement(a, OperationType.Pow, JObject.FromObject(b));
		public static DataStorageElement operator ^(DataStorageElement a, decimal b) => new DataStorageElement(a, OperationType.Pow, JObject.FromObject(b));
        public static DataStorageElement operator -(DataStorageElement a, int b) => new DataStorageElement(a, OperationType.Add, JObject.FromObject(-b));
        public static DataStorageElement operator -(DataStorageElement a, long b) => new DataStorageElement(a, OperationType.Add, JObject.FromObject(-b));
        public static DataStorageElement operator -(DataStorageElement a, float b) => new DataStorageElement(a, OperationType.Add, JObject.FromObject(-b));
        public static DataStorageElement operator -(DataStorageElement a, double b) => new DataStorageElement(a, OperationType.Add, JObject.FromObject(-b));
        public static DataStorageElement operator -(DataStorageElement a, decimal b) => new DataStorageElement(a, OperationType.Add, JObject.FromObject(-b));
        public static DataStorageElement operator /(DataStorageElement a, int b) => new DataStorageElement(a, OperationType.Mul, JObject.FromObject(1m / b));
        public static DataStorageElement operator /(DataStorageElement a, long b) => new DataStorageElement(a, OperationType.Mul, JObject.FromObject(1m / b));
        public static DataStorageElement operator /(DataStorageElement a, float b) => new DataStorageElement(a, OperationType.Mul, JObject.FromObject(1d / b));
        public static DataStorageElement operator /(DataStorageElement a, double b) => new DataStorageElement(a, OperationType.Mul, JObject.FromObject(1d / b));
        public static DataStorageElement operator /(DataStorageElement a, decimal b) => new DataStorageElement(a, OperationType.Mul, JObject.FromObject(1m / b));
        
		public static implicit operator DataStorageElement(bool b) => new DataStorageElement(OperationType.Replace, JObject.FromObject(b));
		public static implicit operator DataStorageElement(int i) => new DataStorageElement(OperationType.Replace, JObject.FromObject(i));
		public static implicit operator DataStorageElement(long l) => new DataStorageElement(OperationType.Replace, JObject.FromObject(l));
		public static implicit operator DataStorageElement(decimal m) => new DataStorageElement(OperationType.Replace, JObject.FromObject(m));
		public static implicit operator DataStorageElement(double d) => new DataStorageElement(OperationType.Replace, JObject.FromObject(d));
		public static implicit operator DataStorageElement(float f) => new DataStorageElement(OperationType.Replace, JObject.FromObject(f));
		public static implicit operator DataStorageElement(string s) => new DataStorageElement(OperationType.Replace, JObject.FromObject(s));
		public static implicit operator DataStorageElement(JObject o) => new DataStorageElement(OperationType.Replace, o);
		public static implicit operator DataStorageElement(Array a) => new DataStorageElement(OperationType.Replace, JObject.FromObject(a));
		public static implicit operator DataStorageElement(List<bool> l) => new DataStorageElement(OperationType.Replace, JObject.FromObject(l));
		public static implicit operator DataStorageElement(List<int> l) => new DataStorageElement(OperationType.Replace, JObject.FromObject(l));
		public static implicit operator DataStorageElement(List<long> l) => new DataStorageElement(OperationType.Replace, JObject.FromObject(l));
		public static implicit operator DataStorageElement(List<decimal> l) => new DataStorageElement(OperationType.Replace, JObject.FromObject(l));
		public static implicit operator DataStorageElement(List<double> l) => new DataStorageElement(OperationType.Replace, JObject.FromObject(l));
		public static implicit operator DataStorageElement(List<float> l) => new DataStorageElement(OperationType.Replace, JObject.FromObject(l));
		public static implicit operator DataStorageElement(List<string> l) => new DataStorageElement(OperationType.Replace, JObject.FromObject(l));
		public static implicit operator DataStorageElement(List<object> l) => new DataStorageElement(OperationType.Replace, JObject.FromObject(l));

		public static implicit operator bool(DataStorageElement e) => RetrieveAndReturnBoolValue<bool>(e);
		public static implicit operator bool?(DataStorageElement e) => RetrieveAndReturnBoolValue<bool?>(e);
		public static implicit operator int(DataStorageElement e) => RetrieveAndReturnDecimalValue<int>(e);
		public static implicit operator int?(DataStorageElement e) => RetrieveAndReturnDecimalValue<int?>(e);
		public static implicit operator long(DataStorageElement e) => RetrieveAndReturnDecimalValue<long>(e);
		public static implicit operator long?(DataStorageElement e) => RetrieveAndReturnDecimalValue<long?>(e);
		public static implicit operator float(DataStorageElement e) => RetrieveAndReturnDecimalValue<float>(e);
		public static implicit operator float?(DataStorageElement e) => RetrieveAndReturnDecimalValue<float?>(e);
		public static implicit operator double(DataStorageElement e) => RetrieveAndReturnDecimalValue<double>(e);
		public static implicit operator double?(DataStorageElement e) => RetrieveAndReturnDecimalValue<double?>(e);
		public static implicit operator decimal(DataStorageElement e) => RetrieveAndReturnDecimalValue<decimal>(e);
		public static implicit operator decimal?(DataStorageElement e) => RetrieveAndReturnDecimalValue<decimal?>(e);
		public static implicit operator string(DataStorageElement e) => RetrieveAndReturnStringValue(e);
		public static implicit operator bool[](DataStorageElement e) => RetrieveAndReturnArrayValue<bool[]>(e);
		public static implicit operator int[](DataStorageElement e) => RetrieveAndReturnArrayValue<int[]>(e);
		public static implicit operator long[](DataStorageElement e) => RetrieveAndReturnArrayValue<long[]>(e);
		public static implicit operator decimal[](DataStorageElement e) => RetrieveAndReturnArrayValue<decimal[]>(e);
		public static implicit operator double[](DataStorageElement e) => RetrieveAndReturnArrayValue<double[]>(e);
		public static implicit operator float[](DataStorageElement e) => RetrieveAndReturnArrayValue<float[]>(e);
		public static implicit operator string[](DataStorageElement e) => RetrieveAndReturnArrayValue<string[]>(e);
		public static implicit operator object[](DataStorageElement e) => RetrieveAndReturnArrayValue<object[]>(e);
		public static implicit operator List<bool>(DataStorageElement e) => RetrieveAndReturnArrayValue<List<bool>>(e);
		public static implicit operator List<int>(DataStorageElement e) => RetrieveAndReturnArrayValue<List<int>>(e);
		public static implicit operator List<long>(DataStorageElement e) => RetrieveAndReturnArrayValue<List<long>>(e);
		public static implicit operator List<decimal>(DataStorageElement e) => RetrieveAndReturnArrayValue<List<decimal>>(e);
		public static implicit operator List<double>(DataStorageElement e) => RetrieveAndReturnArrayValue<List<double>>(e);
		public static implicit operator List<float>(DataStorageElement e) => RetrieveAndReturnArrayValue<List<float>>(e);
		public static implicit operator List<string>(DataStorageElement e) => RetrieveAndReturnArrayValue<List<string>>(e);
		public static implicit operator List<object>(DataStorageElement e) => RetrieveAndReturnArrayValue<List<object>>(e);
		public static implicit operator Array(DataStorageElement e) => RetrieveAndReturnArrayValue<Array>(e);
		public static implicit operator JObject(DataStorageElement e) => e.Context.GetData(e.Context.Key);

#if !NET35
		public static DataStorageElement operator +(DataStorageElement a, BigInteger b) => new DataStorageElement(a, OperationType.Add, JObject.FromJSON(b.ToString()));
		public static DataStorageElement operator *(DataStorageElement a, BigInteger b) => new DataStorageElement(a, OperationType.Mul, JObject.FromJSON(b.ToString()));
		public static DataStorageElement operator %(DataStorageElement a, BigInteger b) => new DataStorageElement(a, OperationType.Mod, JObject.FromJSON(b.ToString()));
		public static DataStorageElement operator ^(DataStorageElement a, BigInteger b) => new DataStorageElement(a, OperationType.Pow, JObject.FromJSON(b.ToString()));
		public static DataStorageElement operator -(DataStorageElement a, BigInteger b) => new DataStorageElement(a, OperationType.Add, JObject.FromJSON((-b).ToString()));
		public static DataStorageElement operator /(DataStorageElement a, BigInteger b) =>
			throw new InvalidOperationException(
				"DataStorage[Key] / BigInterger is not supported, due to loss of precision when using integer division");

		public static implicit operator DataStorageElement(BigInteger bi) => new DataStorageElement(OperationType.Replace, JObject.FromJSON(bi.ToString()));

		public static implicit operator BigInteger(DataStorageElement e) => RetrieveAndReturnBigIntegerValue<BigInteger>(e);
		public static implicit operator BigInteger?(DataStorageElement e) => RetrieveAndReturnBigIntegerValue<BigInteger?>(e);
		
		static T RetrieveAndReturnBigIntegerValue<T>(DataStorageElement e)
		{
			if (e.cachedValue != null)
			{
				return BigInteger.TryParse(e.cachedValue.ToString(), out var cachedBigInteger)
					? (T)Convert.ChangeType(cachedBigInteger, IsNullable<T>() ? Nullable.GetUnderlyingType(typeof(T)) : typeof(T))
					: default;
			}

			var value = BigInteger.TryParse(e.Context.GetData(e.Context.Key).ToString(), out var parsedValue)
				? parsedValue
				: (BigInteger?)null;

			if (!value.HasValue && !IsNullable<T>())
				value = Activator.CreateInstance<BigInteger>();
		
			foreach (var operation in e.Operations)
			{
				if (operation.OperationType == OperationType.Floor || operation.OperationType == OperationType.Ceil)
					continue;

				if (!BigInteger.TryParse(operation.Value.ToString(), NumberStyles.AllowLeadingSign, null, out var operatorValue))
					throw new InvalidOperationException($"DataStorage[Key] cannot be converted to BigInterger as its value its not an integer number, value: {operation.Value}");

				switch (operation.OperationType)
				{ 
					case OperationType.Replace:
						value = operatorValue;
						break;

					case OperationType.Add:
						value += operatorValue;
						break;

					case OperationType.Mul:
						value *= operatorValue;
						break;

					case OperationType.Mod:
						value %= operatorValue;
						break;

					case OperationType.Pow:
						value = BigInteger.Pow(value.Value, (int)operation.Value);
						break;

					case OperationType.Max:
						if (operatorValue > value)
							value = operatorValue;
						break;

					case OperationType.Min:
						if (operatorValue < value)
							value = operatorValue;
						break;

					case OperationType.Xor:
						value ^= operatorValue;
						break;

					case OperationType.Or:
						value |= operatorValue;
						break;

					case OperationType.And:
						value &= operatorValue;
						break;

					case OperationType.LeftShift:
						value <<= (int)operation.Value;
						break;

					case OperationType.RightShift:
						value >>= (int)operation.Value;
						break;
				}
			}

			e.cachedValue = JObject.FromJSON(value.ToString());

            return value.HasValue
	            ? (T)Convert.ChangeType(value.Value, IsNullable<T>() ? Nullable.GetUnderlyingType(typeof(T)) : typeof(T))
	            : default;
		}
#endif


#pragma warning restore CS1591
		/// <summary>
		/// Initializes a value in the server side data storage
		/// Will not override any existing value, only set the default value if none existed
		/// </summary>
		/// <param name="value">The default value for the key</param>
		public void Initialize(JObject value) => Context.Initialize(Context.Key, value);

        /// <summary>
        /// Initializes a value in the server side data storage
        /// Will not override any existing value, only set the default value if none existed
        /// </summary>
        /// <param name="value">The default value for the key</param>
        public void Initialize(IEnumerable value) => Context.Initialize(Context.Key, JObject.FromObject(value));

#if NET35
        /// <summary>
        /// Retrieves the value of a certain key from server side data storage.
        /// </summary>
        /// <param name="callback">The callback that will be called when the value is retrieved</param>
        public void GetAsync<T>(Action<T> callback) => GetAsync(t => callback(t.ToObject<T>()));

        /// <summary>
        /// Retrieves the value of a certain key from server side data storage.
        /// </summary>
        /// <param name="callback">The callback that will be called when the value is retrieved</param>
        public void GetAsync(Action<JObject> callback) => Context.GetAsync(Context.Key, callback);
#else
        /// <summary>
        /// Retrieves the value of a certain key from server side data storage.
        /// </summary>
        public Task<T> GetAsync<T>() => GetAsync().ContinueWith(r => r.Result.ToObject<T>());

        /// <summary>
        /// Retrieves the value of a certain key from server side data storage.
        /// </summary>
        public Task<JObject> GetAsync() => Context.GetAsync(Context.Key);
#endif

        static T RetrieveAndReturnArrayValue<T>(DataStorageElement e)
        {
            if (e.cachedValue != null)
                return ((JObject)e.cachedValue).ToObject<T>();

            var value = e.Context.GetData(e.Context.Key) ?? JObject.FromJSON("{}");

            foreach (var operation in e.Operations)
            {
                switch (operation.OperationType)
                {
                    case OperationType.Add:
                        if (operation.Value.Type != JObjectType.Array)
                            throw new InvalidOperationException(
                                $"Cannot perform operation {OperationType.Add} on Array value, with a non Array value: {operation.Value}");

                        value.Merge(operation.Value);
                        break;

                    case OperationType.Replace:
                        if (operation.Value.Type != JObjectType.Array)
                            throw new InvalidOperationException($"Cannot replace Array value, with a non Array value: {operation.Value}");

                        value = operation.Value;
                        break;

                    default:
                        throw new InvalidOperationException($"Cannot perform operation {operation.OperationType} on Array value");
                }
            }

            e.cachedValue = value;

            return value.ToObject<T>();
        }

        static string RetrieveAndReturnStringValue(DataStorageElement e)
        {
            if (e.cachedValue != null)
                return (string)e.cachedValue;

			var yayToken = e.Context.GetData(e.Context.Key);
			var value = (yayToken.Type == JObjectType.Null)
				? null
				: yayToken.ToString();
			
			foreach (var operation in e.Operations)
            {
                switch (operation.OperationType)
                {
                    case OperationType.Add:
                        value += (string)operation.Value;
                        break;

                    case OperationType.Mul:
                        if (operation.Value.Type != JObjectType.Number)
                            throw new InvalidOperationException($"Cannot perform operation {OperationType.Mul} on string value, with a non interger value: {operation.Value}");

                        value = string.Concat(Enumerable.Repeat(value, (int)operation.Value));
                        break;

                    case OperationType.Replace:
                        value = (string)operation.Value;
                        break;

                    default:
                        throw new InvalidOperationException($"Cannot perform operation {operation.OperationType} on string value");
                }
            }

			e.cachedValue = JObject.FromObject(value);
			
            return (string)e.cachedValue;
        }

        static T RetrieveAndReturnBoolValue<T>(DataStorageElement e)
        {
	        if (e.cachedValue != null)
		        return e.cachedValue.ToObject<T>();

	        var value = e.Context.GetData(e.Context.Key).ToObject<bool?>() ?? (bool?)Activator.CreateInstance(typeof(T));

	        foreach (var operation in e.Operations)
	        {
		        switch (operation.OperationType)
		        {
			        case OperationType.Replace:
				        value = operation.Value.Type == JObjectType.Boolean ? (bool)operation.Value : (bool?)null;
				        break;

			        default:
				        throw new InvalidOperationException($"Cannot perform operation {operation.OperationType} on boolean value");
		        }
	        }

	        e.cachedValue = JObject.FromObject(value);
			
			return value.HasValue 
				? (T)Convert.ChangeType(value.Value, IsNullable<T>() ? Nullable.GetUnderlyingType(typeof(T)) : typeof(T))
				: default;
        }
		
		static T RetrieveAndReturnDecimalValue<T>(DataStorageElement e)
        {
            if (e.cachedValue != null)
                return e.cachedValue.ToObject<T>();

            var value = e.Context.GetData(e.Context.Key).ToObject<decimal?>();

            if (!value.HasValue && !IsNullable<T>())
	            value = Activator.CreateInstance<decimal>();

            foreach (var operation in e.Operations)
            {
               switch (operation.OperationType)
               {
                    case OperationType.Replace:
                        value = (decimal)operation.Value;
                        break;

                    case OperationType.Add:
                        value += (decimal)operation.Value;
                        break;

                    case OperationType.Mul:
                        value *= (decimal)operation.Value;
                        break;
                    
                    case OperationType.Mod:
                        value %= (decimal)operation.Value;
                        break;

                    case OperationType.Pow:
                        value = (decimal)Math.Pow((double)value.Value, (double)operation.Value);
                        break;

                    case OperationType.Max:
                        value = Math.Max(value.Value, (decimal)operation.Value);
                        break;

                    case OperationType.Min:
                        value = Math.Min(value.Value, (decimal)operation.Value);
                        break;

                    case OperationType.Xor:
                        value = (long)value ^ (long)operation.Value;
                        break;

                    case OperationType.Or:
                        value = (long)value | (long)operation.Value;
                        break;

                    case OperationType.And:
                        value = (long)value & (long)operation.Value;
                        break;

                    case OperationType.LeftShift:
                        value = (long)value << (int)operation.Value;
                        break;
                    
                    case OperationType.RightShift:
                        value = (long)value >> (int)operation.Value;
                        break;

					case OperationType.Floor:
						value = Math.Floor(value.Value);
						break;

                    case OperationType.Ceil:
	                    value = Math.Ceiling(value.Value);
	                    break;
				}
            }

            e.cachedValue = JObject.FromObject(value);

            return value.HasValue
	            ? (T)Convert.ChangeType(value.Value, IsNullable<T>() ? Nullable.GetUnderlyingType(typeof(T)) : typeof(T))
	            : default;
		}

        static bool IsNullable<T>() =>
	        typeof(T).IsGenericType
	        && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>).GetGenericTypeDefinition();

		/// <summary>
		/// Retrieves the value from the server and casts it to the given type
		/// Cannot be used in combination with other operators
		/// </summary>
		/// <returns>The value from server as the given type</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///     DataStorageElement.To() cannot be used together with other operations on the DataStorageElement
		///     Other operations include =, +=, /=, + etc
		/// </exception>
		/// <exception cref="T:Archipelago.MultiClient.Net.Exceptions.ArchipelagoSocketClosedException">
		///     The websocket connection is not alive
		/// </exception>
		public T To<T>()
        {
            if (Operations.Count != 0)
                throw new InvalidOperationException(
                    "DataStorageElement.To<T>() cannot be used together with other operations on the DataStorageElement");

            return Context.GetData(Context.Key).ToObject<T>();
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
		public override string ToString() => $"{Context?.ToString() ?? "(null)"}, ({ListOperations()})";

        string ListOperations() => Operations == null 
	        ? "none" 
	        : string.Join(", ", Operations.Select(o => o.ToString()).ToArray());
    }
}