using CipherListTerminal.Core;
using CipherListTerminal.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CipherListTerminal.Entities
{
	internal class Button : IGameEntity
	{
		private Texture2D _buttonUI;
		private string _buttonHeader;
		private string _keyboardMouse;
		private string _gamePad;

		private InputStates _state;

        public Button(Texture2D buttonUI, string buttonHeader, string keyboardMouse, string gamePad)
        {
            _buttonUI = buttonUI;
			_buttonHeader = buttonHeader;
			_keyboardMouse = keyboardMouse;
			_gamePad = gamePad;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale) { }

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale, SpriteFont font, Vector2 position, Color color)
		{
			spriteBatch.Draw(_buttonUI, position, null,
				color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(font, _buttonHeader, position + new Vector2(10, 10), Color.White);
			if (InputManager.IsGamePadConnected())
			{
				spriteBatch.DrawString(font, _keyboardMouse + " " + _gamePad, position + new Vector2(10, 40), Color.White);
			}
			else
			{
				spriteBatch.DrawString(font, _keyboardMouse, position + new Vector2(10, 40), Color.White);
			}						
		}

		public void Update(GameTime gameTime, InputStates inputState)
		{
			_state = inputState;
		}
	}
}
