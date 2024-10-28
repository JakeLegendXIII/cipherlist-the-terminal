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

		public static GamePadType GetControllerType()
		{
			return GamePad.GetCapabilities(PlayerIndex.One).GamePadType;
		}

		public static string GetGamePadDisplayName()
		{
			return GamePad.GetCapabilities(PlayerIndex.One).DisplayName;
		}

		public static MouseState GetMousePosition()
		{
			return mouseState;
		}

		public static bool IsLeftMouseButtonDown()
		{
			return mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton != ButtonState.Pressed;
		}

		public static bool IsKeyPressed(Keys key)
		{
			return keyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
		}

		public static bool IsKeyDown(Keys key)
		{
			return keyboardState.IsKeyDown(key);
		}

		public static Vector2 GetTransformedMousePosition(int startX, int startY)
		{
			Vector2 transformedMousePosition = new Vector2((mouseState.X - (_renderTarget.X + (startX * _scale))) / _scale,
																		(mouseState.Y - (_renderTarget.Y + (startY * _scale))) / _scale);
			return transformedMousePosition;
		}		

		public static bool IsGamePadConnected()
		{
			return gamePadState.IsConnected;
		}

		public static bool PreviousGamePadConnected()
		{
			return lastGamePadState.IsConnected;
		}

		public static bool IsGamePadButtonPressed(Buttons button)
		{
			return gamePadState.IsButtonDown(button) && lastGamePadState.IsButtonUp(button);
		}

		public static GamePadState GetGamePadState()
		{
			return gamePadState;
		}
	}
}
