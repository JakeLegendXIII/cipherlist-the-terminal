using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CipherListTerminal.Entities
{
	internal class ScoreBoard : IGameEntity
	{
		public int Score { get; set; } = 0;
		public int HighScore { get; set; }

		private SpriteFont _font;
		private Texture2D _scoreUI;

		public ScoreBoard(SpriteFont font, Texture2D scoreUI)
		{
			_font = font;
			_scoreUI = scoreUI;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			spriteBatch.Draw(_scoreUI, new Vector2(580, 85), null,
								Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_font, "Score:      " + Score, new Vector2(600, 100), Color.White);
			spriteBatch.DrawString(_font, "High Score: " + HighScore, new Vector2(600, 135), Color.White);
		}

		public void Update(GameTime gameTime)
		{
		}
	}
}
