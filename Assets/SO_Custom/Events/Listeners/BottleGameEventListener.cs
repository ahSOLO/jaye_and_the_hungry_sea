using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "Bottle Game Event Listener")]
	public sealed class BottleGameEventListener : BaseGameEventListener<Bottle, BottleGameEvent, BottleUnityEvent>
	{
	}
}