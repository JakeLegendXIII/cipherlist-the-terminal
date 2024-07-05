using CipherListTerminal.Core;
using CipherListTerminal.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
		private SoundEffect _buttonPress;

		private Vector2 _firstButtonPosition = new Vector2(200, 610);
		private Vector2 _secondButtonPosition = new Vector2(400, 610);
		private Vector2 _thirdButtonPosition = new Vector2(600, 610);
		private Vector2 _fourthButtonPosition = new Vector2(800, 610);
		private Vector2 _fifthButtonPosition = new Vector2(1000, 610);

		public ButtonManager(Texture2D buttonUI, SpriteFont font, InputStates inputState, GameStates gameState, SoundEffect flickingASwitch)
		{
			_buttonUI = buttonUI;
			_inputState = inputState;
			_gameState = gameState;
			_font = font;
			_buttonPress = flickingASwitch;

			// Pre-load the various buttons for each game mode
			_buttons = [
				new Button(_buttonUI, _font, "Back", "ESC", "Back", true),
				new Button(_buttonUI, _font, "Next Puzzle", "F5", "RT", false),
				new Button(_buttonUI, _font, "Clear Saves", "F8", "RB", false),
				new Button(_buttonUI, _font, "Switch Input", "F10", "LT", false),
				new Button(_buttonUI, _font, "Continue", "Enter", "A", true),
				new Button(_buttonUI, _font, "Full Screen", "F11", "LB", false),
			];
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			if (_gameState == GameStates.FreePlay || _gameState == GameStates.SinglePuzzleTimed || _gameState == GameStates.TimeTrial)
			{

				_buttons[0].Draw(spriteBatch, gameTime, scale, _firstButtonPosition);

				_buttons[1].Draw(spriteBatch, gameTime, scale, _secondButtonPosition);

				_buttons[2].Draw(spriteBatch, gameTime, scale, _thirdButtonPosition);

				if (InputManager.IsGamePadConnected())
				{
					_buttons[3].Draw(spriteBatch, gameTime, scale, _fourthButtonPosition);

					_buttons[5].Draw(spriteBatch, gameTime, scale, _fifthButtonPosition);
				}
				else
				{
					_buttons[5].Draw(spriteBatch, gameTime, scale, _fourthButtonPosition);
				}
				
			}

			if (_gameState == GameStates.Summary)
			{
				_buttons[0].Draw(spriteBatch, gameTime, scale, _firstButtonPosition);

				_buttons[4].Draw(spriteBatch, gameTime, scale, _secondButtonPosition);

				if (InputManager.IsGamePadConnected())
				{
					_buttons[3].Draw(spriteBatch, gameTime, scale, _thirdButtonPosition);

					_buttons[5].Draw(spriteBatch, gameTime, scale, _fourthButtonPosition);
				}
				else
				{
					_buttons[5].Draw(spriteBatch, gameTime, scale, _thirdButtonPosition);
				}
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
					_buttonPress.Play();
					_buttons[0].SetColor(Color.Gray);
				}

				if (InputManager.IsGamePadButtonPressed(Buttons.RightTrigger) || InputManager.IsKeyPressed(Keys.F5))
				{
					_buttonPress.Play();
					_buttons[1].SetColor(Color.Gray);
				}

				if (InputManager.IsGamePadButtonPressed(Buttons.LeftTrigger) || InputManager.IsKeyPressed(Keys.F10))
				{
					_buttonPress.Play();
					_buttons[2].SetColor(Color.Gray);
				}

				if (_gameState == GameStates.Summary)
				{
					if (InputManager.IsGamePadButtonPressed(Buttons.A) || InputManager.IsKeyPressed(Keys.Enter))
					{
						_buttonPress.Play();
						_buttons[3].SetColor(Color.Gray);
					}
				}				

				if (InputManager.IsGamePadButtonPressed(Buttons.Y) || InputManager.IsKeyPressed(Keys.F11))
				{
					_buttonPress.Play();
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
