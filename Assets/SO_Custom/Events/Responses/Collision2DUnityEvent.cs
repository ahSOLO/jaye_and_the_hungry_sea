using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class Collision2DUnityEvent : UnityEvent<Collision2D>
	{
	}
}