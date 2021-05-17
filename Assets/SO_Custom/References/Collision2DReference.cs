using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class Collision2DReference : BaseReference<Collision2D, Collision2DVariable>
	{
	    public Collision2DReference() : base() { }
	    public Collision2DReference(Collision2D value) : base(value) { }
	}
}