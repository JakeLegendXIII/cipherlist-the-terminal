using CipherListTerminal.Core;
using CipherListTerminal.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CipherListTerminal.Entities
{
	internal class InputStateIndicator : IGameEntity
	{
		InputStates CurrentInputState;

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{

		}

		public void Update(GameTime gameTime, InputStates inputState)
		{
			if (inputState == InputStates.GamePad)
			{
				if (InputManager.IsGamePadConnected())
					CurrentInputState = InputStates.GamePad;
				else
					CurrentInputState = InputStates.MouseKeyboard;
			}
			else if (inputState == InputStates.MouseKeyboard)
			{
				CurrentInputState = InputStates.MouseKeyboard;
			}
		}
	}
}
