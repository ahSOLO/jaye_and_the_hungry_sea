using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "Collision2D Game Event Listener")]
	public sealed class Collision2DGameEventListener : BaseGameEventListener<Collision2D, Collision2DGameEvent, Collision2DUnityEvent>
	{
	}
}