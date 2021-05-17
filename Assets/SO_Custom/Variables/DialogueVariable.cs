using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class DialogueEvent : UnityEvent<Dialogue> { }

	[CreateAssetMenu(
	    fileName = "DialogueVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "Dialogue",
	    order = 120)]
	public class DialogueVariable : BaseVariable<Dialogue, DialogueEvent>
	{
	}
}