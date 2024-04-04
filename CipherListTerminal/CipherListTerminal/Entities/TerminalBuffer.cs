using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CipherListTerminal.Entities
{
	public class TerminalBuffer : IGameEntity
	{
		protected SpriteFont _font;
		public string Text { get; set; } = "__ __ __ __ __ __ __ __";

		public int DrawOrder => 100;

		public TerminalBuffer(SpriteFont font)
		{
			_font = font;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			spriteBatch.DrawString(_font, Text, new Vector2(20, 10), Color.White);
		}

		public void Update(GameTime gameTime)
		{
			
		}
	}
}
