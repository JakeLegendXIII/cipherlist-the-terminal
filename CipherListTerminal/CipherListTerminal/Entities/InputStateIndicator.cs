using CipherListTerminal.Core;
using CipherListTerminal.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CipherListTerminal.Entities
{
	internal class InputStateIndicator : IGameEntity
	{
		private SpriteFont _armadaFont;
		InputStates CurrentInputState;

		public delegate void InputStateEventHandler(InputStates inputState);
		public event InputStateEventHandler InputStateEvent;

		public InputStateIndicator(SpriteFont armadaFont, InputStates inputState)
		{
			_armadaFont = armadaFont;
			CurrentInputState = inputState;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			spriteBatch.DrawString(_armadaFont, "InputState: ", new Vector2(930, 100), Color.White);
			spriteBatch.DrawString(_armadaFont, CurrentInputState.ToString(), new Vector2(930, 120), Color.White);
		}

		public void Update(GameTime gameTime)
		{
			if (InputManager.IsKeyPressed(Keys.F10) || InputManager.IsGamePadButtonPressed(Buttons.LeftTrigger))
			{
				ToggleInputState();
			}

			if (InputManager.PreviousGamePadConnected() && !InputManager.IsGamePadConnected() && CurrentInputState == InputStates.GamePad)
			{
				ToggleInputState();
			}
		}

		private void ToggleInputState()
		{
			if (CurrentInputState == InputStates.GamePad)
			{
				CurrentInputState = InputStates.MouseKeyboard;
			}
			else if (CurrentInputState == InputStates.MouseKeyboard)
			{
				if (InputManager.IsGamePadConnected())
				{
					CurrentInputState = InputStates.GamePad;
				}
				else
				{
					CurrentInputState = InputStates.MouseKeyboard;
				}
			}

			// TODO : broadcast message instead of passing CurrentInputState in Update
			//if (inputState == InputStates.GamePad)
			//{
			//	if (InputManager.IsGamePadConnected())
			//		CurrentInputState = InputStates.GamePad;
			//	else
			//		CurrentInputState = InputStates.MouseKeyboard;
			//}
			//else if (inputState == InputStates.MouseKeyboard)
			//{
			//	CurrentInputState = InputStates.MouseKeyboard;
			//}
		}
	}
}
