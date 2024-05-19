using CipherListTerminal.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CipherListTerminal.Entities
{
	public class TerminalBuffer : IGameEntity
	{
		protected SpriteFont _armadaFont;		
		private Texture2D _bufferUI;
		public string Text { get; set; } = "__ __ __ __ __ __ __ __ ";
		public bool IsCompleted { get; set; } = false;

		public TerminalBuffer(SpriteFont armadaFont, Texture2D bufferUI)
		{
			_armadaFont = armadaFont;			
			_bufferUI = bufferUI;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			// Draw the Background UI			
			spriteBatch.Draw(_bufferUI, new Vector2(160, 85), null,
				Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, "Buffer:", new Vector2(170, 95), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, Text, new Vector2(180, 150), Color.White);
		}

		public void Update(GameTime gameTime, InputStates inputState) { }
	}
}
