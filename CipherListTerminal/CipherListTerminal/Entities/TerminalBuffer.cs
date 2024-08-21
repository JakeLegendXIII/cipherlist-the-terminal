using CipherListTerminal.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CipherListTerminal.Entities
{
	public class TerminalBuffer : IGameEntity
	{
		private const int UI_WIDTH = 330;
		private const int UI_HEIGHT = 100;
		private const int BUFFER_UI_X = 79;
		private const int BUFFER_UI_Y = 5;
		private Rectangle _bufferUIRectangle = new Rectangle(BUFFER_UI_X, BUFFER_UI_Y, UI_WIDTH, UI_HEIGHT);

		protected SpriteFont _armadaFont;		
		private Texture2D _spriteSheet;
		public string Text { get; set; } = "__ __ __ __ __ __ __ __ ";
		public bool IsCompleted { get; set; } = false;

		public TerminalBuffer(SpriteFont armadaFont,Texture2D spriteSheet)
		{
			_armadaFont = armadaFont;			
			_spriteSheet = spriteSheet;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			// Draw the Background UI			
			spriteBatch.Draw(_spriteSheet, new Vector2(170, 85), _bufferUIRectangle,
				Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, "Buffer:", new Vector2(180, 95), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, Text, new Vector2(190, 150), Color.White);
		}

		public void Update(GameTime gameTime, InputStates inputState) { }
	}
}
