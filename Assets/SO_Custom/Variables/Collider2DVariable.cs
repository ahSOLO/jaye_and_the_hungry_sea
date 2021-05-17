using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class Collider2DEvent : UnityEvent<Collider2D> { }

	[CreateAssetMenu(
	    fileName = "Collider2DVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "Collider 2D",
	    order = 120)]
	public class Collider2DVariable : BaseVariable<Collider2D, Collider2DEvent>
	{
	}
}