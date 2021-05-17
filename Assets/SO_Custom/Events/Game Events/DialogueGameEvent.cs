using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "DialogueGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "Dialogue",
	    order = 120)]
	public sealed class DialogueGameEvent : GameEventBase<Dialogue>
	{
	}
}