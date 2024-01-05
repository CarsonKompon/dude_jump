using System;
using Sandbox;
using Sandbox.Citizen;

public sealed class DudeJumpPlayer : Component
{
	[Property] public float Speed { get; set; } = 100;
	[Property] public float Friction { get; set; } = 6;
	[Property] public float JumpForce { get; set; } = 200;

	CitizenAnimationHelper animationHelper;

	public Vector3 Velocity;
	public float VSpeed = 0;

	protected override void OnStart()
	{
		animationHelper = Components.Get<CitizenAnimationHelper>( FindMode.EverythingInSelfAndChildren );
	}

	protected override void OnUpdate()
	{
		if ( !DudeGameManager.Instance.IsPlaying ) return;

		if ( Input.Down( "Left" ) )
		{
			VSpeed += Friction * Time.Delta;
			if ( VSpeed > Speed ) VSpeed = Speed;
		}
		else if ( Input.Down( "Right" ) )
		{
			VSpeed -= Friction * Time.Delta;
			if ( VSpeed < -Speed ) VSpeed = -Speed;
		}
		else
		{
			VSpeed -= Friction * MathF.Sign( VSpeed ) * Time.Delta;
			if ( MathF.Abs( VSpeed ) < Friction * Time.Delta ) VSpeed = 0;
		}

		Transform.Position = Transform.Position.WithY( Math.Clamp( Transform.Position.y, -150, 150 ) );

		if ( animationHelper is not null )
		{
			animationHelper.IsGrounded = false;
			animationHelper.WithWishVelocity( -Velocity );
			animationHelper.WithVelocity( -Velocity );
			animationHelper.Height = 0.5f;
		}
	}

	protected override void OnFixedUpdate()
	{
		if ( !DudeGameManager.Instance.IsPlaying ) return;

		Velocity = Velocity.WithY( VSpeed );
		Velocity += Scene.PhysicsWorld.Gravity * Time.Delta * 0.5f;

		if ( GroundCheck() )
		{
			Jump();
		}

		Transform.Position += Velocity * Time.Delta;

		Velocity += Scene.PhysicsWorld.Gravity * Time.Delta * 0.5f;

		if ( GroundCheck() )
		{
			Jump();
		}
	}

	bool GroundCheck()
	{
		if ( Velocity.z > 0 )
		{
			return false;
		}

		var tr = Scene.Trace.Ray( Transform.Position, Transform.Position + Vector3.Up * Velocity.z * Time.Delta )
			.Radius( 8f )
			.WithoutTags( "player", "trigger" )
			.Run();

		if ( tr.Hit )
		{
			var interactable = tr.GameObject.Components.Get<Interactable>( FindMode.EverythingInSelfAndChildren );
			if ( interactable is not null )
			{
				interactable.Interact();
				if ( interactable.ForceRecheck )
					return false;
			}
		}

		return tr.Hit;
	}

	public void Jump( float force = 0 )
	{
		if ( force == 0 ) force = JumpForce;
		Velocity = Velocity.WithZ( force );
		animationHelper?.TriggerJump();
		Sound.Play( "ui.navigate.forward" );
	}


}