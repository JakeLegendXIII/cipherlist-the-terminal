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

		private InputStates _state;

        public Button(Texture2D buttonUI, string buttonHeader, string keyboardMouse, string gamePad)
        {
            
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
