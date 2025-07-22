using Archipelago.MultiClient.Net.Json;

namespace Archipelago.MultiClient.Net.Enums
{
	/// <summary>
	/// The type of DataStorage operation
	/// </summary>
    [JsonStringEnum(snakeCase: true)]
    public enum OperationType
    {
		Add,
        Mul,
        Max,
        Min,
        Replace,
        Default,
        Mod,
        Pow,
        Xor,
        Or,
        And,
        LeftShift,
        RightShift,
		Remove,
		Pop,
		Update,
		Floor,
		Ceil
    }
}