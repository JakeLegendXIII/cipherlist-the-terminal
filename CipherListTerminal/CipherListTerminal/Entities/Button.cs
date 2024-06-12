using CipherListTerminal.Core;
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
		private Vector2 _position;

		private InputStates _state;

        public Button(Texture2D buttonUI, Vector2 position, string buttonHeader, string keyboardMouse, string gamePad)
        {
            _buttonUI = buttonUI;
			_position = position;
			_buttonHeader = buttonHeader;
			_keyboardMouse = keyboardMouse;
			_gamePad = gamePad;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			
		}

		public void Update(GameTime gameTime, InputStates inputState)
		{
			_state = inputState;
		}
	}
}
