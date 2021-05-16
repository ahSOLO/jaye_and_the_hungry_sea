using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class BottleReference : BaseReference<Bottle, BottleVariable>
	{
	    public BottleReference() : base() { }
	    public BottleReference(Bottle value) : base(value) { }
	}
}