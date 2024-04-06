using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CipherListTerminal.Input
{
	public static class InputManager
	{
		private static MouseState mouseState, lastMouseState;
		private static Rectangle _renderTarget;
		private static float _scale;

		public static void Update(Rectangle renderTarget, float scale)
		{
			lastMouseState = mouseState;
			mouseState = Mouse.GetState();
			_renderTarget = renderTarget;
			_scale = scale;
		}

		public static MouseState GetMousePosition()
		{
			return mouseState;
		}

		public static Vector2 GetTransformedMousePosition()
		{
			Vector2 transformedMousePosition = new Vector2((mouseState.X - (_renderTarget.X + (100 * _scale))) / _scale,
																		(mouseState.Y - (_renderTarget.Y + (100 * _scale))) / _scale);
			return transformedMousePosition;
		}
	}
}
