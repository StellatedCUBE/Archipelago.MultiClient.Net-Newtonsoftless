using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Json;
using System;

namespace Archipelago.MultiClient.Net.Converters
{
    public class PermissionsEnumConverter : JsonConverter
    {
        public static readonly PermissionsEnumConverter instance = new PermissionsEnumConverter();

        public override object ToObject(JObject data)
        {
            var value = data.ToString();
            var isInt = int.TryParse(value, out var intValue);

            if (isInt)
                return (Permissions)intValue;

            var returnValue = Permissions.Disabled;

            if (value.Contains("enabled"))
                returnValue |= Permissions.Enabled;

            if (value.Contains("auto"))
                returnValue |= Permissions.Auto;

            if (value.Contains("goal"))
                returnValue |= Permissions.Goal;

            return returnValue;
        }

        public override JObject FromObject(object @object)
        {
            var permissionsValue = (Permissions)@object;

            return JObject.FromObject((int)permissionsValue);
        }
    }
}
