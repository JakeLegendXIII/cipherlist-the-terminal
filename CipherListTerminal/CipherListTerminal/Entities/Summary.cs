using CipherListTerminal.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CipherListTerminal.Entities
{
	public class Summary : IGameEntity
	{
		private const int UI_WIDTH = 330;
		private const int UI_HEIGHT = 380;
		private const int SUMMARY_UI_X = 515;
		private const int SUMMARY_UI_Y = 5;
		private Rectangle _summaryUIRectangle = new Rectangle(SUMMARY_UI_X, SUMMARY_UI_Y, UI_WIDTH, UI_HEIGHT);

		public int Score { get; set; } = 0;
		public int HighScore { get; set; }
		public string HighScoreDate { get; set; }

		private Texture2D _summaryUI;
		private Texture2D _spriteSheet;
		private SpriteFont _armadaFont;

		private int _summaryUIPositionX = 260;
		private int _summaryUIPositionY = 85;		

		public Summary(Texture2D spriteSheet, SpriteFont armadaFont)
		{
			_spriteSheet = spriteSheet;
			_armadaFont = armadaFont;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{			
			spriteBatch.Draw(_spriteSheet, new Vector2(_summaryUIPositionX, _summaryUIPositionY), _summaryUIRectangle, Color.White, 
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, "Session Summary", new Vector2(270, 95), Color.White, 
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, "Score: " + Score, new Vector2(280, 150), Color.White);
			spriteBatch.DrawString(_armadaFont, "High Score: " + HighScore, new Vector2(280, 175), Color.White);
			spriteBatch.DrawString(_armadaFont, "Date Achieved: " + HighScoreDate, new Vector2(280, 200), Color.White);
		}

		public void Update(GameTime gameTime, InputStates inputState) { }
	}
}
