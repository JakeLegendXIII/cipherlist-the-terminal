using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CipherListTerminal.Entities
{
	public class Summary : IGameEntity
	{
		public int Score { get; set; } = 0;
		public int HighScore { get; set; }
		public DateTime HighScoreDate { get; set; }

		private Texture2D _summaryUI;
		private SpriteFont _armadaFont;

		private int _summaryUIPositionX = 160;
		private int _summaryUIPositionY = 85;		

		public Summary(Texture2D summaryUI, SpriteFont armadaFont)
		{
			_summaryUI = summaryUI;
			_armadaFont = armadaFont;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{			
			spriteBatch.Draw(_summaryUI, new Vector2(_summaryUIPositionX, _summaryUIPositionY), null, Color.White, 
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, "Session Summary", new Vector2(170, 95), Color.White, 
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, "Score: " + Score, new Vector2(180, 150), Color.White);
			spriteBatch.DrawString(_armadaFont, "High Score: " + HighScore, new Vector2(180, 175), Color.White);
			spriteBatch.DrawString(_armadaFont, "Date Achieved: " + HighScoreDate.ToShortDateString(), new Vector2(180, 200), Color.White);
		}

		public void Update(GameTime gameTime)
		{

		}
	}
}
