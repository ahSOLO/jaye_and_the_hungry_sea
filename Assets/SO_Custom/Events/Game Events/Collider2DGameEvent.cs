using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "Collider2DGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "Collider 2D",
	    order = 120)]
	public sealed class Collider2DGameEvent : GameEventBase<Collider2D>
	{
	}
}