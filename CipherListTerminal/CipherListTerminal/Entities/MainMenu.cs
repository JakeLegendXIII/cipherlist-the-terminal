using CipherListTerminal.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
			//Vector2 transformedMousePosition = InputManager.GetMenuTransformedMousePosiiton();
			MouseState mouseState = InputManager.GetMousePosition();
			Vector2 transformedMousePosition = new Vector2(mouseState.X, mouseState.Y);

			spriteBatch.Draw(_menuLogo, new Vector2(400, 125), null,
					Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_farawayFont, $"MousePos: X:{transformedMousePosition.X} Y:{transformedMousePosition.Y}", new Vector2(425, 150), Color.White);
			
			if (transformedMousePosition.X >= _buttonPosition1X && transformedMousePosition.X <= (_buttonPosition1X + 200) &&
												transformedMousePosition.Y >= _buttonPosition1Y && transformedMousePosition.Y <= (_buttonPosition1Y + 200))
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

			if (transformedMousePosition.X >= _buttonPosition2X && transformedMousePosition.X <= (_buttonPosition2X + 200) &&
								transformedMousePosition.Y >= _buttonPosition2Y && transformedMousePosition.Y <= (_buttonPosition2Y + 200))
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
