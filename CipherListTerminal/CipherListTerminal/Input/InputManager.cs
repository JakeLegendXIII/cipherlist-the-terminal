using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CipherListTerminal.Input
{
	public static class InputManager
	{
		private static MouseState mouseState, lastMouseState;

		public static void Update()
		{
			lastMouseState = mouseState;
			mouseState = Mouse.GetState();
		}

		public static MouseState GetMousePosition()
		{
			return mouseState;
		}
	}
}
