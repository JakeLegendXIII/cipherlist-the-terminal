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
			if (_gameState == GameStates.FreePlay || _gameState == GameStates.SinglePuzzleTimed || _gameState == GameStates.TimeTrial)
			{

				_buttons[0].Draw(spriteBatch, gameTime, scale, _font, new Vector2(250, 610));

				_buttons[1].Draw(spriteBatch, gameTime, scale, _font, new Vector2(450, 610));

				_buttons[2].Draw(spriteBatch, gameTime, scale, _font, new Vector2(650, 610));

				_buttons[4].Draw(spriteBatch, gameTime, scale, _font, new Vector2(850, 610));
			}

			if (_gameState == GameStates.Summary)
			{
				_buttons[0].Draw(spriteBatch, gameTime, scale, _font, new Vector2(250, 610));

				_buttons[3].Draw(spriteBatch, gameTime, scale, _font, new Vector2(450, 610));

				_buttons[2].Draw(spriteBatch, gameTime, scale, _font, new Vector2(650, 610));

				_buttons[4].Draw(spriteBatch, gameTime, scale, _font, new Vector2(850, 610));
			}
		}

		public void Update(GameTime gameTime, InputStates inputState) { }

		public void Update(GameTime gameTime, InputStates inputState, GameStates gameState)
		{
			_inputState = inputState;
			_gameState = gameState;

			if (_gameState != GameStates.Menu)
			{
				if (InputManager.IsGamePadButtonPressed(Buttons.Back) || InputManager.IsKeyPressed(Keys.Escape))
				{
					_buttons[0].SetColor(Color.Gray);
				}

				if (InputManager.IsGamePadButtonPressed(Buttons.RightTrigger) || InputManager.IsKeyPressed(Keys.F5))
				{
					_buttons[1].SetColor(Color.Gray);
				}

				if (InputManager.IsGamePadButtonPressed(Buttons.LeftTrigger) || InputManager.IsKeyPressed(Keys.F10))
				{
					_buttons[2].SetColor(Color.Gray);
				}

				if (InputManager.IsGamePadButtonPressed(Buttons.A) || InputManager.IsKeyPressed(Keys.Enter))
				{
					_buttons[3].SetColor(Color.Gray);
				}

				if (InputManager.IsGamePadButtonPressed(Buttons.Y) || InputManager.IsKeyPressed(Keys.F11))
				{
					_buttons[4].SetColor(Color.Gray);
				}

				for (var i = 0; i < _buttons.Count; i++)
				{
					_buttons[i].Update(gameTime, inputState);
				}
			}
		}
	}
}
