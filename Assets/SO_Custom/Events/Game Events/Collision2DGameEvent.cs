using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "Collision2DGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "Collision 2D",
	    order = 120)]
	public sealed class Collision2DGameEvent : GameEventBase<Collision2D>
	{
	}
}