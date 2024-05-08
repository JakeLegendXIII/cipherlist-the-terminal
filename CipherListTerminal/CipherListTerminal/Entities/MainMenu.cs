using CipherListTerminal.Core;
using CipherListTerminal.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CipherListTerminal.Entities
{
	internal class MainMenu : IGameEntity
	{
		private Texture2D _menuLogo;
		private Texture2D _buttonUI;
		private SpriteFont _armadaFont;
		private SpriteFont _farawayFont;

		private int _buttonPosition1X = 250;
		private int _buttonPosition1Y = 400;

		private int _buttonPosition2X = 550;
		private int _buttonPosition2Y = 400;

		private int _buttonPosition3X = 850;
		private int _buttonPosition3Y = 400;

		private int _buttonWidth = 200;
		private int _buttonHeight = 200;

		public delegate void MenuButtonSelectedEventHandler(GameStates newGameState);
		public event MenuButtonSelectedEventHandler MenuButtonSelectionEvent;

		public MainMenu(Texture2D menuLogo, Texture2D buttonUI, SpriteFont armadaFont, SpriteFont farawayFont)
		{
			_menuLogo = menuLogo;
			_buttonUI = buttonUI;
			_armadaFont = armadaFont;
			_farawayFont = farawayFont;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			Vector2 transformedMousePositionButton1 = InputManager.GetTransformedMousePosition(_buttonPosition1X, _buttonPosition1Y);
			Vector2 transformedMousePositionButton2 = InputManager.GetTransformedMousePosition(_buttonPosition2X, _buttonPosition2Y);
			Vector2 transformedMousePositionButton3 = InputManager.GetTransformedMousePosition(_buttonPosition3X, _buttonPosition3Y);

			spriteBatch.Draw(_menuLogo, new Vector2(400, 125), null,
					Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			if (transformedMousePositionButton1.X >= 0 && transformedMousePositionButton1.X <= _buttonWidth &&
				transformedMousePositionButton1.Y >= 0 && transformedMousePositionButton1.Y <= _buttonHeight)
			{
				spriteBatch.Draw(_buttonUI, new Vector2(_buttonPosition1X, _buttonPosition1Y), null,
										Color.Gray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.Draw(_buttonUI, new Vector2(_buttonPosition1X, _buttonPosition1Y), null,
					Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}

			spriteBatch.DrawString(_armadaFont, "Free Play", new Vector2(300, 460), Color.White);

			if (transformedMousePositionButton2.X >= 0 && transformedMousePositionButton2.X <= _buttonWidth &&
				transformedMousePositionButton2.Y >= 0 && transformedMousePositionButton2.Y <= _buttonHeight)
			{
				spriteBatch.Draw(_buttonUI, new Vector2(_buttonPosition2X, _buttonPosition2Y), null,
										Color.Gray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.Draw(_buttonUI, new Vector2(_buttonPosition2X, _buttonPosition2Y), null,
										Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}

			spriteBatch.DrawString(_armadaFont, "Single Timed", new Vector2(585, 460), Color.White);

			if (transformedMousePositionButton3.X >= 0 && transformedMousePositionButton3.X <= _buttonWidth &&
			transformedMousePositionButton3.Y >= 0 && transformedMousePositionButton3.Y <= _buttonHeight)
			{
				spriteBatch.Draw(_buttonUI, new Vector2(_buttonPosition3X, _buttonPosition3Y), null,
										Color.Gray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.Draw(_buttonUI, new Vector2(_buttonPosition3X, _buttonPosition3Y), null,
										Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}

			spriteBatch.DrawString(_armadaFont, "Time Trial", new Vector2(900, 460), Color.White);
		}

		public void Update(GameTime gameTime)
		{
			Vector2 transformedMousePositionButton1 = InputManager.GetTransformedMousePosition(_buttonPosition1X, _buttonPosition1Y);
			Vector2 transformedMousePositionButton2 = InputManager.GetTransformedMousePosition(_buttonPosition2X, _buttonPosition2Y);
			Vector2 transformedMousePositionButton3 = InputManager.GetTransformedMousePosition(_buttonPosition3X, _buttonPosition3Y);

			if (transformedMousePositionButton1.X >= 0 && transformedMousePositionButton1.X <= _buttonWidth &&
				transformedMousePositionButton1.Y >= 0 && transformedMousePositionButton1.Y <= _buttonHeight)
			{
				if (InputManager.IsLeftMouseButtonDown())
				{
					MenuButtonSelectionEvent?.Invoke(GameStates.FreePlay);
				}
			}

			if (transformedMousePositionButton2.X >= 0 && transformedMousePositionButton2.X <= _buttonWidth &&
								transformedMousePositionButton2.Y >= 0 && transformedMousePositionButton2.Y <= _buttonHeight)
			{
				if (InputManager.IsLeftMouseButtonDown())
				{
					MenuButtonSelectionEvent?.Invoke(GameStates.SinglePuzzleTimed);
				}
			}

			if (transformedMousePositionButton3.X >= 0 && transformedMousePositionButton3.X <= _buttonWidth &&
								transformedMousePositionButton3.Y >= 0 && transformedMousePositionButton3.Y <= _buttonHeight)
			{
				if (InputManager.IsLeftMouseButtonDown())
				{
					MenuButtonSelectionEvent?.Invoke(GameStates.SinglePuzzleTimed);
				}
			}
		}
	}
}
