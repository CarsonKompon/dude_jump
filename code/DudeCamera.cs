using System;
using Sandbox;

public sealed class DudeCamera : Component
{
	[Property] GameObject Target { get; set; }
	[Property] float Offset { get; set; } = 100;


	protected override void OnUpdate()
	{
		if ( Target.Transform.Position.z > Transform.Position.z - Offset )
		{
			Transform.Position = Transform.Position.WithZ( MathF.Max( Target.Transform.Position.z - Offset, Transform.Position.z ) );
		}
	}
}