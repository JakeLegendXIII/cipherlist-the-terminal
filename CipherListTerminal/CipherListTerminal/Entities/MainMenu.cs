using CipherListTerminal.Core;
using CipherListTerminal.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CipherListTerminal.Entities
{
	internal class MainMenu : IGameEntity
	{
		private const int UI_WIDTH = 500;
		private const int UI_HEIGHT = 250;
		private const int MENULOGO_UI_X = 265;
		private const int MENULOGO_UI_Y = 395;
		private Rectangle _menuLogoUIRectangle = new Rectangle(MENULOGO_UI_X, MENULOGO_UI_Y, UI_WIDTH, UI_HEIGHT);

		private const int BUTTON_UI_WIDTH = 250;
		private const int BUTTON_UI_HEIGHT = 150;
		private const int BUTTON_UI_X = 5;
		private const int BUTTON_UI_Y = 395;
		private Rectangle _buttonUIRectangle = new Rectangle(BUTTON_UI_X, BUTTON_UI_Y, BUTTON_UI_WIDTH, BUTTON_UI_HEIGHT);

		private Texture2D _spriteSheet;
		private SpriteFont _armadaFont;
		private SpriteFont _farawayFont;

		private SoundEffect _buttonPress;
		private SoundEffect _flickingASwitch;

		private int _buttonPosition1X = 100;
		private int _buttonPosition1Y = 425;

		private int _buttonPosition2X = 375;
		private int _buttonPosition2Y = 425;

		private int _buttonPosition3X = 650;
		private int _buttonPosition3Y = 425;

		private int _buttonPosition4X = 925;
		private int _buttonPosition4Y = 425;

		private int _buttonWidth = 250;
		private int _buttonHeight = 150;

		public delegate void MenuButtonSelectedEventHandler(GameStates newGameState);
		public event MenuButtonSelectedEventHandler MenuButtonSelectionEvent;

		private int _currentlySelectedButton = 1;
		private bool thumbstickMoved = false;

		private InputStates CurrentInputState;

		public MainMenu(Texture2D spriteSheet, SpriteFont armadaFont, SpriteFont farawayFont, 
			SoundEffect buttonPress, SoundEffect flickingASwitch)
		{
			_spriteSheet = spriteSheet;
			_armadaFont = armadaFont;
			_farawayFont = farawayFont;
			_buttonPress = buttonPress;
			_flickingASwitch = flickingASwitch;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			spriteBatch.Draw(_spriteSheet, new Vector2(400, 125), _menuLogoUIRectangle,
					Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);						

			if (_currentlySelectedButton == 1)
			{
				spriteBatch.Draw(_spriteSheet, new Vector2(_buttonPosition1X, _buttonPosition1Y), _buttonUIRectangle,
										Color.Gray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

				if (CurrentInputState == InputStates.GamePad)
				{
					spriteBatch.DrawString(_armadaFont, "A", new Vector2(330, 550), Color.White);
				}				
			}
			else
			{
				spriteBatch.Draw(_spriteSheet, new Vector2(_buttonPosition1X, _buttonPosition1Y), _buttonUIRectangle,
					Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}

			spriteBatch.DrawString(_armadaFont, "Free Play", new Vector2(180, 445), Color.White);
			spriteBatch.DrawString(_farawayFont, "No time or puzzle limits.", new Vector2(170, 520), Color.White);
			spriteBatch.DrawString(_farawayFont, "Just have fun with it!", new Vector2(170, 535), Color.White);

			spriteBatch.DrawString(_armadaFont, "F1", new Vector2(110, 550), Color.White);

			if (_currentlySelectedButton == 2)
			{
				spriteBatch.Draw(_spriteSheet, new Vector2(_buttonPosition2X, _buttonPosition2Y), _buttonUIRectangle,
										Color.Gray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.Draw(_spriteSheet, new Vector2(_buttonPosition2X, _buttonPosition2Y), _buttonUIRectangle,
										Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}

			spriteBatch.DrawString(_armadaFont, "Best of 10 Timed", new Vector2(425, 445), Color.White);
			spriteBatch.DrawString(_farawayFont, "60 seconds per puzzle.", new Vector2(445, 520), Color.White);
			spriteBatch.DrawString(_farawayFont, "10 puzzles. Do your best!", new Vector2(445, 535), Color.White);

			if (_currentlySelectedButton == 3)
			{
				spriteBatch.Draw(_spriteSheet, new Vector2(_buttonPosition3X, _buttonPosition3Y), _buttonUIRectangle,
										Color.Gray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.Draw(_spriteSheet, new Vector2(_buttonPosition3X, _buttonPosition3Y), _buttonUIRectangle,
										Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}

			spriteBatch.DrawString(_armadaFont, "Time Trial", new Vector2(730, 445), Color.White);
			spriteBatch.DrawString(_farawayFont, "5 minutes total.", new Vector2(700, 520), Color.White);
			spriteBatch.DrawString(_farawayFont, "As many puzzles as you can solve!", new Vector2(700, 535), Color.White);

			if (_currentlySelectedButton == 4)
			{
				spriteBatch.Draw(_spriteSheet, new Vector2(_buttonPosition4X, _buttonPosition4Y), _buttonUIRectangle,
										Color.Gray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.Draw(_spriteSheet, new Vector2(_buttonPosition4X, _buttonPosition4Y), _buttonUIRectangle,
										Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}

			spriteBatch.DrawString(_armadaFont, "Settings", new Vector2(1005, 445), Color.White);
			spriteBatch.DrawString(_farawayFont, "Update game settings.", new Vector2(1000, 520), Color.White);
			spriteBatch.DrawString(_farawayFont, "Review High Scores.", new Vector2(1000, 535), Color.White);

			// spriteBatch.DrawString(_armadaFont, $"Selected button: {_currentlySelectedButton}", new Vector2(450, 100), Color.White);
		}

		public void Update(GameTime gameTime, InputStates inputState)
		{
			CurrentInputState = inputState;

			if (inputState == InputStates.MouseKeyboard)
			{
				Vector2 transformedMousePositionButton1 = InputManager.GetTransformedMousePosition(_buttonPosition1X, _buttonPosition1Y);
				Vector2 transformedMousePositionButton2 = InputManager.GetTransformedMousePosition(_buttonPosition2X, _buttonPosition2Y);
				Vector2 transformedMousePositionButton3 = InputManager.GetTransformedMousePosition(_buttonPosition3X, _buttonPosition3Y);
				Vector2 transformedMousePositionButton4 = InputManager.GetTransformedMousePosition(_buttonPosition4X, _buttonPosition4Y);

				if (transformedMousePositionButton1.X >= 0 && transformedMousePositionButton1.X <= _buttonWidth &&
					transformedMousePositionButton1.Y >= 0 && transformedMousePositionButton1.Y <= _buttonHeight)
				{
					_currentlySelectedButton = 1;

					if (InputManager.IsLeftMouseButtonDown())
					{
						_buttonPress.Play();
						MenuButtonSelectionEvent?.Invoke(GameStates.FreePlay);
					}
				}
				else if (transformedMousePositionButton2.X >= 0 && transformedMousePositionButton2.X <= _buttonWidth &&
									transformedMousePositionButton2.Y >= 0 && transformedMousePositionButton2.Y <= _buttonHeight)
				{
					_currentlySelectedButton = 2;

					if (InputManager.IsLeftMouseButtonDown())
					{
						_buttonPress.Play();
						MenuButtonSelectionEvent?.Invoke(GameStates.SinglePuzzleTimed);
					}
				}
				else if (transformedMousePositionButton3.X >= 0 && transformedMousePositionButton3.X <= _buttonWidth &&
									transformedMousePositionButton3.Y >= 0 && transformedMousePositionButton3.Y <= _buttonHeight)
				{
					_currentlySelectedButton = 3;

					if (InputManager.IsLeftMouseButtonDown())
					{
						_buttonPress.Play();
						MenuButtonSelectionEvent?.Invoke(GameStates.TimeTrial);
					}
				}
				else if (transformedMousePositionButton4.X >= 0 && transformedMousePositionButton4.X <= _buttonWidth &&
					transformedMousePositionButton4.Y >= 0 && transformedMousePositionButton4.Y <= _buttonHeight)
				{
					_currentlySelectedButton = 4;

					if (InputManager.IsLeftMouseButtonDown())
					{
						_buttonPress.Play();
						MenuButtonSelectionEvent?.Invoke(GameStates.Settings);
					}
				}
				else
				{
					_currentlySelectedButton = 0;
				}
			}
			else if (inputState == InputStates.GamePad)
			{
				if (InputManager.IsGamePadConnected())
				{
					GamePadState gamePadState = InputManager.GetGamePadState();

					if (_currentlySelectedButton > 4 || _currentlySelectedButton < 1)
					{
						_currentlySelectedButton = 1;
					}

					if (InputManager.IsGamePadButtonPressed(Buttons.DPadLeft))
					{
						_flickingASwitch.Play();
						MoveLeft();				
					}

					if (InputManager.IsGamePadButtonPressed(Buttons.DPadRight))
					{
						_flickingASwitch.Play();
						MoveRight();					
					}

					// Check if the thumbstick is moved to the right or left
					if (Math.Abs(gamePadState.ThumbSticks.Left.X) > 0.5f)
					{
						if (!thumbstickMoved)
						{
							if (gamePadState.ThumbSticks.Left.X > 0)
							{
								_flickingASwitch.Play();
								MoveRight();
							}
							else if (gamePadState.ThumbSticks.Left.X < 0)
							{
								_flickingASwitch.Play();
								MoveLeft();
							}
							thumbstickMoved = true;
						}
					}
					else
					{
						thumbstickMoved = false;
					}

					if (InputManager.IsGamePadButtonPressed(Buttons.A))
					{
						_buttonPress.Play();
						switch (_currentlySelectedButton)
						{
							case 1:
								MenuButtonSelectionEvent?.Invoke(GameStates.FreePlay);
								break;
							case 2:
								MenuButtonSelectionEvent?.Invoke(GameStates.SinglePuzzleTimed);
								break;
							case 3:
								MenuButtonSelectionEvent?.Invoke(GameStates.TimeTrial);
								break;
							case 4:
								MenuButtonSelectionEvent?.Invoke(GameStates.Settings);
								break;
						}
					}
				}
			}          
        }

		private void MoveRight()
		{
			if (_currentlySelectedButton == 4)
			{
				_currentlySelectedButton = 1;
			}
			else
			{
				_currentlySelectedButton++;
			}
		}

		private void MoveLeft()
		{
			if (_currentlySelectedButton == 1)
			{
				_currentlySelectedButton = 4;
			}
			else
			{
				_currentlySelectedButton--;
			}
		}
	}
}
