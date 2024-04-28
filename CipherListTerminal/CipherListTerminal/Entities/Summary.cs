using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CipherListTerminal.Entities
{
	public class Summary : IGameEntity
	{
		public int DrawOrder => 0;

		public int Score { get; set; } = 0;

		private Texture2D _summaryUI;

		private SpriteFont _armadaFont;

		public Summary(Texture2D summaryUI, SpriteFont armadaFont)
		{
			_summaryUI = summaryUI;
			_armadaFont = armadaFont;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{			
			spriteBatch.Draw(_summaryUI, new Vector2(160, 85), null, Color.White, 
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, "Summary", new Vector2(170, 95), Color.White, 
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, "Score: " + Score, new Vector2(180, 150), Color.White);
		}

		public void Update(GameTime gameTime)
		{

		}
	}
}
