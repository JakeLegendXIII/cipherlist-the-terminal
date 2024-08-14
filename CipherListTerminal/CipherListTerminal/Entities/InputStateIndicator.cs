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
		private Texture2D _gamePadIcon;

		public InputStateIndicator(SpriteFont armadaFont, InputStates inputState, Texture2D gamepadIcon)
		{
			_armadaFont = armadaFont;
			CurrentInputState = inputState;
			_gamePadIcon = gamepadIcon;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			if (CurrentInputState == InputStates.GamePad)
			{
				spriteBatch.Draw(_gamePadIcon, new Vector2(1150, 70), null,
				Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.DrawString(_armadaFont, "InputState: ", new Vector2(1050, 80), Color.White);
				spriteBatch.DrawString(_armadaFont, CurrentInputState.ToString(), new Vector2(1050, 100), Color.White);
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
