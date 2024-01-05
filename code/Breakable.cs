using Sandbox;

public sealed class Breakable : Component
{
	Interactable interactable;

	protected override void OnStart()
	{
		interactable = Components.Get<Interactable>();
		interactable.OnInteract += Break;
	}

	public void Break()
	{
		GameObject.Destroy();
		Sound.Play( "ui.downvote" );
	}
}