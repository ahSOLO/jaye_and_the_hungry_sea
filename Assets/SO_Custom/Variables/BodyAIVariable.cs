using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class BodyAIEvent : UnityEvent<BodyAI> { }

	[CreateAssetMenu(
	    fileName = "BodyAIVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "BodyAI",
	    order = 120)]
	public class BodyAIVariable : BaseVariable<BodyAI, BodyAIEvent>
	{
	}
}