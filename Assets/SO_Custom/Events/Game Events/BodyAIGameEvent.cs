using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "BodyAIGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "BodyAI",
	    order = 120)]
	public sealed class BodyAIGameEvent : GameEventBase<BodyAI>
	{
	}
}