using CipherListTerminal.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CipherListTerminal.Entities
{
	internal class ScoreBoard : IGameEntity
	{
		private const int UI_WIDTH = 500;
		private const int UI_HEIGHT = 125;
		private const int SCORE_UI_X = 5;
		private const int SCORE_UI_Y = 655;
		private Rectangle _scoreUIRectangle = new Rectangle(SCORE_UI_X, SCORE_UI_Y, UI_WIDTH, UI_HEIGHT);

		public int Score { get; set; } = 0;
		public int HighScore { get; set; }

		private SpriteFont _font;
		private Texture2D _spriteSheet;

		public ScoreBoard(SpriteFont font, Texture2D spriteSheet)
		{
			_font = font;
			_spriteSheet = spriteSheet;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			spriteBatch.Draw(_spriteSheet, new Vector2(580, 85), _scoreUIRectangle,
								Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_font, "Score:      " + Score, new Vector2(600, 95), Color.White);
			spriteBatch.DrawString(_font, "High Score: " + HighScore, new Vector2(600, 130), Color.White);
		}

		public void Update(GameTime gameTime, InputStates inputState) { }
	}
}
