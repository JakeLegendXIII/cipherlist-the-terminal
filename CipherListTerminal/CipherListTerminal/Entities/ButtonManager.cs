﻿using CipherListTerminal.Core;
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

		private Texture2D _spriteSheet;
		private SpriteFont _font;
		private SoundEffect _buttonPress;

		private Vector2 _firstButtonPosition = new Vector2(150, 610);
		private Vector2 _secondButtonPosition = new Vector2(350, 610);
		private Vector2 _thirdButtonPosition = new Vector2(550, 610);
		private Vector2 _fourthButtonPosition = new Vector2(750, 610);
		private Vector2 _fifthButtonPosition = new Vector2(950, 610);
		private Vector2 _sixthButtonPosition = new Vector2(1150, 610);

		private Vector2 _sixButtonOffset = new Vector2(10, 0);
		private Vector2 _eightButtonOffset = new Vector2(20, 0);
		private Vector2 _outsideOffset = new Vector2(-80, 0);

		public ButtonManager(Texture2D spriteSheet, SpriteFont font, InputStates inputState, GameStates gameState, SoundEffect flickingASwitch)
		{
			_spriteSheet = spriteSheet;
			_inputState = inputState;
			_gameState = gameState;
			_font = font;
			_buttonPress = flickingASwitch;

			// Pre-load the various buttons for each game mode
			_buttons = [
				new Button(_spriteSheet, _font, "Back", "ESC", "Back", "Back", true),
				new Button(_spriteSheet, _font, "Next Puzzle", "F5", "RT", "R2", false),
				new Button(_spriteSheet, _font, "Reset", "F7", "RB", "R1", false),
				new Button(_spriteSheet, _font, "Switch Input", "F10", "LT", "L2", false),
				new Button(_spriteSheet, _font, "Continue", "Enter", "A", "X", true),
				new Button(_spriteSheet, _font, "Full Screen", "F11", "LB", "L1", false),
				new Button(_spriteSheet, _font, "CRT FX", "F12", "X", "[]", false),
				new Button(_spriteSheet, _font, "Music", "F8", "Y", @"/\", false),
				new Button(_spriteSheet, _font, "Quit", "ESC", "Back", "Back", true),
			];
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			if (_gameState == GameStates.Menu)
			{
				_buttons[8].Draw(spriteBatch, gameTime, scale, _thirdButtonPosition + _eightButtonOffset);
			}

			if (_gameState == GameStates.FreePlay || _gameState == GameStates.SinglePuzzleTimed || _gameState == GameStates.TimeTrial)
			{

				_buttons[0].Draw(spriteBatch, gameTime, scale, _firstButtonPosition);

				_buttons[1].Draw(spriteBatch, gameTime, scale, _secondButtonPosition);

				//_buttons[2].Draw(spriteBatch, gameTime, scale, _thirdButtonPosition);

				if (InputManager.IsGamePadConnected())
				{
					_buttons[3].Draw(spriteBatch, gameTime, scale, _thirdButtonPosition);					
					
				}				
			}

			if (_gameState == GameStates.Summary)
			{
				_buttons[0].Draw(spriteBatch, gameTime, scale, _firstButtonPosition);

				_buttons[4].Draw(spriteBatch, gameTime, scale, _secondButtonPosition);

				if (InputManager.IsGamePadConnected())
				{
					_buttons[3].Draw(spriteBatch, gameTime, scale, _thirdButtonPosition);					
				}
			}

			if (_gameState == GameStates.Settings)
			{				
				if (InputManager.IsGamePadConnected())
				{
					_buttons[0].Draw(spriteBatch, gameTime, scale, _firstButtonPosition  + (_outsideOffset + _sixButtonOffset));
					_buttons[2].Draw(spriteBatch, gameTime, scale, _secondButtonPosition + _outsideOffset);
					_buttons[7].Draw(spriteBatch, gameTime, scale, _thirdButtonPosition  + _outsideOffset);
					_buttons[3].Draw(spriteBatch, gameTime, scale, _fourthButtonPosition + _outsideOffset);																						 
					_buttons[5].Draw(spriteBatch, gameTime, scale, _fifthButtonPosition  + _outsideOffset);																						 
					_buttons[6].Draw(spriteBatch, gameTime, scale, _sixthButtonPosition  + _outsideOffset);
				}
				else
				{
					_buttons[0].Draw(spriteBatch, gameTime, scale, _firstButtonPosition);
					_buttons[2].Draw(spriteBatch, gameTime, scale, _secondButtonPosition);
					_buttons[7].Draw(spriteBatch, gameTime, scale, _thirdButtonPosition);
					_buttons[5].Draw(spriteBatch, gameTime, scale, _fourthButtonPosition);
					_buttons[6].Draw(spriteBatch, gameTime, scale, _fifthButtonPosition);
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

				if (InputManager.IsGamePadButtonPressed(Buttons.RightShoulder) || InputManager.IsKeyPressed(Keys.F7))
				{
					_buttonPress.Play();
					_buttons[2].SetColor(Color.Gray);
				}

				if (InputManager.IsGamePadButtonPressed(Buttons.Y) || InputManager.IsKeyPressed(Keys.F8))
				{
					_buttonPress.Play();
					_buttons[7].SetColor(Color.Gray);
				}

				if (InputManager.IsGamePadButtonPressed(Buttons.LeftTrigger) || InputManager.IsKeyPressed(Keys.F10))
				{
					_buttonPress.Play();
					_buttons[3].SetColor(Color.Gray);
				}

				if (_gameState == GameStates.Summary)
				{
					if (InputManager.IsGamePadButtonPressed(Buttons.A) || InputManager.IsKeyPressed(Keys.Enter))
					{
						_buttonPress.Play();
						_buttons[4].SetColor(Color.Gray);
					}
				}				

				if (InputManager.IsGamePadButtonPressed(Buttons.LeftShoulder) || InputManager.IsKeyPressed(Keys.F11))
				{
					_buttonPress.Play();
					_buttons[5].SetColor(Color.Gray);
				}

				if (InputManager.IsGamePadButtonPressed(Buttons.X) || InputManager.IsKeyPressed(Keys.F12))
				{
					_buttonPress.Play();
					_buttons[6].SetColor(Color.Gray);
				}

				for (var i = 0; i < _buttons.Count; i++)
				{
					_buttons[i].Update(gameTime, inputState);
				}
			}
			else
			{
				if (InputManager.IsGamePadButtonPressed(Buttons.Back) || InputManager.IsKeyPressed(Keys.Escape))
				{
					_buttonPress.Play();
					_buttons[8].SetColor(Color.Gray);
				}
			}
		}
	}
}
