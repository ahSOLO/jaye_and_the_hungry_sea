using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class BottleEvent : UnityEvent<Bottle> { }

	[CreateAssetMenu(
	    fileName = "BottleVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "Bottle",
	    order = 120)]
	public class BottleVariable : BaseVariable<Bottle, BottleEvent>
	{
	}
}