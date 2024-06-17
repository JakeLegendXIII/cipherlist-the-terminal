using CipherListTerminal.Core;
using CipherListTerminal.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CipherListTerminal.Entities
{
	internal class ButtonManager : IGameEntity
	{
		private InputStates _inputState;
		private GameStates _gameState;
		private List<Button> _buttons;

		private Texture2D _buttonUI;
		private SpriteFont _font;

		public ButtonManager(Texture2D buttonUI, SpriteFont font, InputStates inputState, GameStates gameState)
		{
			_buttonUI = buttonUI;
			_inputState = inputState;
			_gameState = gameState;
			_font = font;

			// Pre-load the various buttons for each game mode
			_buttons = [
				new Button(_buttonUI, "Back", "ESC", "Back"),
				new Button(_buttonUI, "Next Puzzle", "F5", "RT"),
				new Button(_buttonUI, "Switch Input", "F10", "LT"),
				new Button(_buttonUI, "Continue", "Enter", "A"),
				new Button(_buttonUI, "Full Screen", "F11", "Y"),
			];
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			if (_gameState == GameStates.FreePlay)
			{
				if (InputManager.IsGamePadButtonPressed(Buttons.Back) || InputManager.IsKeyPressed(Keys.Escape))
				{
					_buttons[0].Draw(spriteBatch, gameTime, scale, _font, new Vector2(250, 610), Color.Gray);
				}
				else
				{
					_buttons[0].Draw(spriteBatch, gameTime, scale, _font, new Vector2(250, 610), Color.White);
				}

				if (InputManager.IsGamePadButtonPressed(Buttons.RightTrigger) || InputManager.IsKeyPressed(Keys.F5))
                {
					_buttons[1].Draw(spriteBatch, gameTime, scale, _font, new Vector2(450, 610), Color.Gray);
				}
				else
				{
					_buttons[1].Draw(spriteBatch, gameTime, scale, _font, new Vector2(450, 610), Color.White);
				}
                

				_buttons[2].Draw(spriteBatch, gameTime, scale, _font, new Vector2(650, 610), Color.White);

				_buttons[4].Draw(spriteBatch, gameTime, scale, _font, new Vector2(850, 610), Color.White);
			}
		}

		public void Update(GameTime gameTime, InputStates inputState) {}

		public void Update(GameTime gameTime, InputStates inputState, GameStates gameState)
		{
			_inputState = inputState;
			_gameState = gameState;
		}
	}
}
