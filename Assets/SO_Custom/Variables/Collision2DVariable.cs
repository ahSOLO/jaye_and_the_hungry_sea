using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class Collision2DEvent : UnityEvent<Collision2D> { }

	[CreateAssetMenu(
	    fileName = "Collision2DVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "Collision 2D",
	    order = 120)]
	public class Collision2DVariable : BaseVariable<Collision2D, Collision2DEvent>
	{
	}
}