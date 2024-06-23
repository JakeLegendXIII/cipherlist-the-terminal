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

		private Vector2 _headerTextSize;
		private Vector2 _headerTextPositionOffset;

		private Vector2 _inputTextSize;
		private Vector2 _inputTextWithGamePadSize;
		private Vector2 _inputTextPositionOffset;
		private Vector2 _inputTextWithGamePadPositionOffset;

		public Button(Texture2D buttonUI, SpriteFont headerFont,
			string buttonHeader, string keyboardMouse, string gamePad)
		{
			_buttonUI = buttonUI;
			_headerFont = headerFont;
			_buttonHeader = buttonHeader;
			_keyboardMouse = keyboardMouse;
			_gamePad = gamePad;
			_color = _originalColor;

			_headerTextSize = _headerFont.MeasureString(_buttonHeader);
			_headerTextPositionOffset = new Vector2((_buttonUI.Width - _headerTextSize.X) / 2, 40);

			_inputTextSize = _headerFont.MeasureString(_keyboardMouse);
			_inputTextWithGamePadSize = _headerFont.MeasureString(_keyboardMouse + " " + _gamePad);
			_inputTextPositionOffset = new Vector2((_buttonUI.Width - _inputTextSize.X) / 2, 10);
			_inputTextWithGamePadPositionOffset = new Vector2((_buttonUI.Width - _inputTextWithGamePadSize.X) / 2, 10);
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale) { }

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale, Vector2 position)
		{
			spriteBatch.Draw(_buttonUI, position, null,
				_color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			string inputText = _keyboardMouse;

			if (InputManager.IsGamePadConnected())
			{
				inputText += " " + _gamePad;
				spriteBatch.DrawString(_headerFont, inputText, position + _inputTextWithGamePadPositionOffset, Color.White);
			}
			else
			{
				spriteBatch.DrawString(_headerFont, inputText, position + _inputTextPositionOffset, Color.White);
			}
			
			spriteBatch.DrawString(_headerFont, _buttonHeader, position + _headerTextPositionOffset, Color.White);
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
