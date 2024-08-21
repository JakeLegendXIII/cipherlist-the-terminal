using CipherListTerminal.Core;
using CipherListTerminal.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CipherListTerminal.Entities
{
	internal class InputStateIndicator : IGameEntity
	{
		private const int ICON_WIDTH = 64;
		private const int ICON_HEIGHT = 64;
		private const int GAMEPAD_ICON_X = 5;
		private const int GAMEPAD_ICON_Y = 5;
		private const int MOUSE_KEYBOARD_ICON_X = 419;
		private const int MOUSE_KEYBOARD_ICON_Y = 5;
		private Rectangle _gamePadRectangle = new Rectangle(GAMEPAD_ICON_X, GAMEPAD_ICON_Y, ICON_WIDTH, ICON_HEIGHT);
		private Rectangle _mouseKeyboardRectangle = new Rectangle(MOUSE_KEYBOARD_ICON_X, MOUSE_KEYBOARD_ICON_Y, ICON_WIDTH, ICON_HEIGHT);

		private SpriteFont _armadaFont;
		InputStates CurrentInputState;
		private Texture2D _spriteSheet;

		private Vector2 _iconLocation = new Vector2(1135, 65);

		public InputStateIndicator(SpriteFont armadaFont, InputStates inputState, Texture2D spriteSheet)
		{
			_armadaFont = armadaFont;
			CurrentInputState = inputState;
			_spriteSheet = spriteSheet;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			if (CurrentInputState == InputStates.GamePad)
			{
				spriteBatch.Draw(_spriteSheet, _iconLocation, _gamePadRectangle,
				Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.Draw(_spriteSheet, _iconLocation, _mouseKeyboardRectangle,
				Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}

			//spriteBatch.DrawString(_armadaFont, "InputState: ", new Vector2(1050, 80), Color.White);
			//spriteBatch.DrawString(_armadaFont, CurrentInputState.ToString(), new Vector2(1050, 100), Color.White);
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
