using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CipherListTerminal.Input
{
	public static class InputManager
	{
		private static MouseState mouseState, lastMouseState;
		private static KeyboardState keyboardState, lastKeyboardState;
		private static GamePadState gamePadState, lastGamePadState;
		private static Rectangle _renderTarget;
		private static float _scale;

		public static void Update(Rectangle renderTarget, float scale)
		{
			lastMouseState = mouseState;
			mouseState = Mouse.GetState();
			lastKeyboardState = keyboardState;
			keyboardState = Keyboard.GetState();
			lastGamePadState = gamePadState;
			gamePadState = GamePad.GetState(PlayerIndex.One);

			_renderTarget = renderTarget;
			_scale = scale;
		}

		public static MouseState GetMousePosition()
		{
			return mouseState;
		}

		public static bool IsLeftMouseButtonDown()
		{
			return mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton != ButtonState.Pressed;
		}

		public static bool IsF5Pressed()
		{
			return keyboardState.IsKeyDown(Keys.F5) && lastKeyboardState.IsKeyUp(Keys.F5);
		}

		public static Vector2 GetTransformedMousePosition()
		{
			Vector2 transformedMousePosition = new Vector2((mouseState.X - (_renderTarget.X + (200 * _scale))) / _scale,
																		(mouseState.Y - (_renderTarget.Y + (200 * _scale))) / _scale);
			return transformedMousePosition;
		}
	}
}
