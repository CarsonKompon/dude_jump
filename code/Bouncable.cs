using Sandbox;

public sealed class Bouncable : Component
{
	Interactable interactable;

	protected override void OnStart()
	{
		interactable = Components.Get<Interactable>();
		interactable.OnInteract += Bounce;
	}

	public void Bounce()
	{
		var player = DudeGameManager.Instance.Player;
		player.Jump( player.JumpForce * 1.5f );
		Sound.Play( "ui.upvote" );
	}
}