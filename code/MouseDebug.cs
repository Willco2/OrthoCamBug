using Sandbox;
using Sandbox.UI;
using System.Linq;

public sealed class MouseDebug : Component
{

	private Panel panel;

	protected override void OnEnabled()
	{
		panel = Scene.GetAllComponents<ScreenPanel>().First().GetPanel().Add.Panel();
		panel.Style.PointerEvents = PointerEvents.All;
	}

	protected override void OnDisabled()
	{
		panel.Delete();
		panel = null;
	}

	protected override void OnUpdate()
	{
		var cam = Scene.Camera;
		var mouseRay = cam.ScreenPixelToRay( Mouse.Position );
		var planeOrigin = cam.Transform.Position + cam.Transform.Rotation.Forward * 100;
		var planeNormal = cam.Transform.Rotation.Backward;

		var worldPos = new Plane( planeOrigin, planeNormal ).Trace( mouseRay ) ?? Vector3.Zero;

		using ( Gizmo.Scope( "MouseDebug" ) )
		{
			Gizmo.Draw.IgnoreDepth = true;
			Gizmo.Draw.Color = Color.Red;
			Gizmo.Draw.SolidBox( BBox.FromPositionAndSize( worldPos, 4 ) );
		}

		if ( Input.Pressed( "attack1" ) )
		{
			var screenPos = Scene.Camera.PointToScreenPixels( worldPos );
			Log.Info( $"Moving mouse to {screenPos}" );
			Mouse.Position = screenPos;
		}

		if ( Input.Pressed( "attack2" ) )
		{
			cam.Orthographic = !cam.Orthographic;
		}
	}

}
