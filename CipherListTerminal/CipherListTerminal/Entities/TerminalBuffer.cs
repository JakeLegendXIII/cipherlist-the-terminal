using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CipherListTerminal.Entities
{
	public class TerminalBuffer : IGameEntity
	{
		protected SpriteFont _font;
		private Texture2D _bufferUI;
		public string Text { get; set; } = "__ __ __ __ __ __ __ __ ";
		public bool IsCompleted { get; set; } = false;

		public int DrawOrder => 100;

		public TerminalBuffer(SpriteFont font, Texture2D bufferUI)
		{
			_font = font;
			_bufferUI = bufferUI;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			// Draw the Background UI			
			spriteBatch.Draw(_bufferUI, new Vector2(160, 85), null,
				Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			spriteBatch.DrawString(_font, Text, new Vector2(180, 150), Color.White);
		}

		public void Update(GameTime gameTime)
		{
			
		}
	}
}
