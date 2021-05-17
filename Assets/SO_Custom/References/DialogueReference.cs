using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class DialogueReference : BaseReference<Dialogue, DialogueVariable>
	{
	    public DialogueReference() : base() { }
	    public DialogueReference(Dialogue value) : base(value) { }
	}
}