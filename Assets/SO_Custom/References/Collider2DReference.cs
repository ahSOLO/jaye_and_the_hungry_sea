using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class Collider2DReference : BaseReference<Collider2D, Collider2DVariable>
	{
	    public Collider2DReference() : base() { }
	    public Collider2DReference(Collider2D value) : base(value) { }
	}
}