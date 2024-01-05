using System;
using Sandbox;

public sealed class Interactable : Component
{
	[Property] public bool ForceRecheck { get; set; } = false;
	public Action OnInteract;

	public void Interact()
	{
		OnInteract?.Invoke();
	}
}