using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class BodyAIReference : BaseReference<BodyAI, BodyAIVariable>
	{
	    public BodyAIReference() : base() { }
	    public BodyAIReference(BodyAI value) : base(value) { }
	}
}