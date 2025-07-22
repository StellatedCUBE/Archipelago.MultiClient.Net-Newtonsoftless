using Archipelago.MultiClient.Net.Json;

namespace Archipelago.MultiClient.Net.Enums
{
	[JsonStringEnum]
    public enum JsonMessageType
    {
		ItemSend,
		ItemCheat,
        Hint,
		Join,
		Part,
		Chat,
		ServerChat,
		Tutorial,
		TagsChanged,
		CommandResult,
	    AdminCommandResult,
	    Goal,
	    Release,
	    Collect,
	    Countdown,
	}
}
