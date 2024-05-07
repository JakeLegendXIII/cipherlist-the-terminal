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

		private int _buttonPosition1X = 350;
		private int _buttonPosition1Y = 450;

		private int _buttonPosition2X = 650;
		private int _buttonPosition2Y = 450;

		public MainMenu(Texture2D menuLogo, Texture2D buttonUI, SpriteFont armadaFont, SpriteFont farawayFont)
		{
			_menuLogo = menuLogo;
			_buttonUI = buttonUI;
			_armadaFont = armadaFont;
			_farawayFont = farawayFont;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			Vector2 transformedMousePosition = InputManager.GetTransformedMousePosition();			

			spriteBatch.Draw(_menuLogo, new Vector2(400, 125), null,
					Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_farawayFont, $"MousePos: X:{transformedMousePosition.X} Y:{transformedMousePosition.Y}", new Vector2(425, 150), Color.White);

			if (transformedMousePosition.X >= 0 && transformedMousePosition.X < _buttonPosition1X &&
								transformedMousePosition.Y >= 0 && transformedMousePosition.Y < _buttonPosition1Y)
			{
				spriteBatch.Draw(_buttonUI, new Vector2(_buttonPosition1X, _buttonPosition1Y), null,
										Color.Gray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{

				spriteBatch.Draw(_buttonUI, new Vector2(_buttonPosition1X, _buttonPosition1Y), null,
					Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}

			spriteBatch.DrawString(_armadaFont, "Free Play", new Vector2(400, 460), Color.White);

			if (transformedMousePosition.X >= _buttonPosition2X && transformedMousePosition.X < 1280 &&
								transformedMousePosition.Y >= 0 && transformedMousePosition.Y < _buttonPosition2Y)
			{
				spriteBatch.Draw(_buttonUI, new Vector2(_buttonPosition2X, _buttonPosition2Y), null,
										Color.Gray, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.Draw(_buttonUI, new Vector2(_buttonPosition2X, _buttonPosition2Y), null,
										Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}

			spriteBatch.DrawString(_armadaFont, "Time Trial", new Vector2(700, 460), Color.White);
		}

		public void Update(GameTime gameTime)
		{
			
		}
	}
}
