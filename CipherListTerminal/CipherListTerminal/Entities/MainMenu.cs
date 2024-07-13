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
		private Texture2D _menuLogo;
		private Texture2D _buttonUI;
		private SpriteFont _armadaFont;
		private SpriteFont _farawayFont;

		private SoundEffect _buttonPress;
		private SoundEffect _flickingASwitch;

		private int _buttonPosition1X = 150;
		private int _buttonPosition1Y = 375;

		private int _buttonPosition2X = 500;
		private int _buttonPosition2Y = 375;

		private int _buttonPosition3X = 850;
		private int _buttonPosition3Y = 375;

		private int _buttonWidth = 300;
		private int _buttonHeight = 200;

		public delegate void MenuButtonSelectedEventHandler(GameStates newGameState);
		public event MenuButtonSelectedEventHandler MenuButtonSelectionEvent;

		private int _currentlySelectedButton = 1;
		private bool thumbstickMoved = false;

		public MainMenu(Texture2D menuLogo, Texture2D buttonUI, SpriteFont armadaFont, SpriteFont farawayFont, 
			SoundEffect buttonPress, SoundEffect flickingASwitch)
		{
			_menuLogo = menuLogo;
			_buttonUI = buttonUI;
			_armadaFont = armadaFont;
			_farawayFont = farawayFont;
			_buttonPress = buttonPress;
			_flickingASwitch = flickingASwitch;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			spriteBatch.Draw(_menuLogo, new Vector2(400, 100), null,
					Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);						

			if (_currentlySelectedButton == 1)
			{
				spriteBatch.Draw(_buttonUI, new Vector2(_buttonPosition1X, _buttonPosition1Y), null,
										Color.Gray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.Draw(_buttonUI, new Vector2(_buttonPosition1X, _buttonPosition1Y), null,
					Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}

			spriteBatch.DrawString(_armadaFont, "Free Play", new Vector2(250, 420), Color.White);
			spriteBatch.DrawString(_farawayFont, "No time or puzzle limits.", new Vector2(240, 520), Color.White);
			spriteBatch.DrawString(_farawayFont, "Just have fun with it!", new Vector2(240, 540), Color.White);

			if (_currentlySelectedButton == 2)
			{
				spriteBatch.Draw(_buttonUI, new Vector2(_buttonPosition2X, _buttonPosition2Y), null,
										Color.Gray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.Draw(_buttonUI, new Vector2(_buttonPosition2X, _buttonPosition2Y), null,
										Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}

			spriteBatch.DrawString(_armadaFont, "Best of 10 Timed", new Vector2(575, 420), Color.White);
			spriteBatch.DrawString(_farawayFont, "60 seconds per puzzle.", new Vector2(595, 520), Color.White);
			spriteBatch.DrawString(_farawayFont, "10 puzzles. Do your best!", new Vector2(595, 540), Color.White);

			if (_currentlySelectedButton == 3)
			{
				spriteBatch.Draw(_buttonUI, new Vector2(_buttonPosition3X, _buttonPosition3Y), null,
										Color.Gray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.Draw(_buttonUI, new Vector2(_buttonPosition3X, _buttonPosition3Y), null,
										Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}

			spriteBatch.DrawString(_armadaFont, "Time Trial", new Vector2(950, 420), Color.White);
			spriteBatch.DrawString(_farawayFont, "5 minutes total.", new Vector2(910, 520), Color.White);
			spriteBatch.DrawString(_farawayFont, "As many puzzles as you can solve!", new Vector2(910, 540), Color.White);

			// spriteBatch.DrawString(_armadaFont, $"Selected button: {_currentlySelectedButton}", new Vector2(450, 100), Color.White);
		}

		public void Update(GameTime gameTime, InputStates inputState)
		{

			if (inputState == InputStates.MouseKeyboard)
			{
				Vector2 transformedMousePositionButton1 = InputManager.GetTransformedMousePosition(_buttonPosition1X, _buttonPosition1Y);
				Vector2 transformedMousePositionButton2 = InputManager.GetTransformedMousePosition(_buttonPosition2X, _buttonPosition2Y);
				Vector2 transformedMousePositionButton3 = InputManager.GetTransformedMousePosition(_buttonPosition3X, _buttonPosition3Y);

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

					if (_currentlySelectedButton > 3 || _currentlySelectedButton < 1)
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
						}
					}
				}
			}          
        }

		private void MoveRight()
		{
			if (_currentlySelectedButton == 3)
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
				_currentlySelectedButton = 3;
			}
			else
			{
				_currentlySelectedButton--;
			}
		}
	}
}
