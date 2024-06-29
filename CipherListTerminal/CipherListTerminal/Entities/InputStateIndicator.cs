using CipherListTerminal.Core;
using CipherListTerminal.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CipherListTerminal.Entities
{
	internal class InputStateIndicator : IGameEntity
	{
		private SpriteFont _armadaFont;
		InputStates CurrentInputState;

        public InputStateIndicator(SpriteFont armadaFont, InputStates inputState)
        {
			_armadaFont = armadaFont;
            CurrentInputState = inputState;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			if (InputManager.IsGamePadConnected())
			{
				spriteBatch.DrawString(_armadaFont, "InputState: ", new Vector2(930, 100), Color.White);
				spriteBatch.DrawString(_armadaFont, CurrentInputState.ToString(), new Vector2(930, 120), Color.White);
			}			
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
