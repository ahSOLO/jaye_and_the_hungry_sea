using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "BottleGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "Bottle",
	    order = 120)]
	public sealed class BottleGameEvent : GameEventBase<Bottle>
	{
	}
}