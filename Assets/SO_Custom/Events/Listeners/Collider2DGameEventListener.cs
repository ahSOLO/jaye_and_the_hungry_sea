using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "Collider2D Game Event Listener")]
	public sealed class Collider2DGameEventListener : BaseGameEventListener<Collider2D, Collider2DGameEvent, Collider2DUnityEvent>
	{
	}
}