using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "BodyAI Game Event Listener")]
	public sealed class BodyAIGameEventListener : BaseGameEventListener<BodyAI, BodyAIGameEvent, BodyAIUnityEvent>
	{
	}
}