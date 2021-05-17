using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class Collider2DUnityEvent : UnityEvent<Collider2D>
	{
	}
}