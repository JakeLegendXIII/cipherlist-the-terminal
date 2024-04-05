using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CipherListTerminal;

namespace CipherListTerminal.Input
{
	public static class InputManager
	{
		private static MouseState mouseState, lastMouseState;
		private static Rectangle _renderTarget;

		public static void Update(Rectangle renderTarget)
		{
			lastMouseState = mouseState;
			mouseState = Mouse.GetState();
			_renderTarget = renderTarget;
		}

		public static MouseState GetMousePosition()
		{
			return mouseState;
		}		
	}
}
