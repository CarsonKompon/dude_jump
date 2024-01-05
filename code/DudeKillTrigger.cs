using Sandbox;

public sealed class DudeKillTrigger : Component, Component.ITriggerListener
{
	protected override void OnUpdate()
	{

	}

	public void OnTriggerEnter( Collider other )
	{
		Log.Info( $"Triggered by {other}" );
		var player = other.Components.Get<DudeJumpPlayer>( FindMode.EverythingInSelfAndParent );
		if ( player is not null )
		{
			DudeGameManager.Instance.EndGame();
		}

		if ( other.Tags.Has( "platform" ) )
		{
			other.GameObject.Destroy();
		}
	}

	public void OnTriggerExit( Collider other )
	{

	}
}