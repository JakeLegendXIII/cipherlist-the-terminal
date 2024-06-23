using CipherListTerminal.Core;
using CipherListTerminal.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CipherListTerminal.Entities
{
	internal class Button : IGameEntity
	{
		private Texture2D _buttonUI;
		private SpriteFont _headerFont;
		private string _buttonHeader;
		private string _keyboardMouse;
		private string _gamePad;

		private Color _color;
		private Color _originalColor = Color.White;

		private double colorChangeTime;
		private double colorChangeDuration = 0.25; // Duration in seconds

		private InputStates _state;

		public Button(Texture2D buttonUI, SpriteFont headerFont, 
			string buttonHeader, string keyboardMouse, string gamePad)
		{
			_buttonUI = buttonUI;
			_headerFont = headerFont;
			_buttonHeader = buttonHeader;
			_keyboardMouse = keyboardMouse;
			_gamePad = gamePad;
			_color = _originalColor;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale) { }

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale, Vector2 position)
		{
			spriteBatch.Draw(_buttonUI, position, null,
				_color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			if (InputManager.IsGamePadConnected())
			{
				spriteBatch.DrawString(_headerFont, _keyboardMouse + " " + _gamePad, position + new Vector2(10, 10), Color.White);
			}
			else
			{
				spriteBatch.DrawString(_headerFont, _keyboardMouse, position + new Vector2(10, 10), Color.White);
			}

			spriteBatch.DrawString(_headerFont, _buttonHeader, position + new Vector2(10, 40), Color.White);
		}

		public void Update(GameTime gameTime, InputStates inputState)
		{
			_state = inputState;

			if (_color != Color.White)
			{
				colorChangeTime += gameTime.ElapsedGameTime.TotalSeconds;
				if (colorChangeTime >= colorChangeDuration)
				{
					ResetColor();
					colorChangeTime = 0;
				}
			}
		}

		public void SetColor(Color color)
		{
			_color = color;
		}

		public void ResetColor()
		{
			_color = _originalColor;
		}
	}
}
