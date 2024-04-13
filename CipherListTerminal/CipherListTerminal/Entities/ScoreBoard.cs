using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CipherListTerminal.Entities
{
	internal class ScoreBoard : IGameEntity
	{
		public int DrawOrder => 0;
		public int Score { get; set; } = 0;

		private SpriteFont _font;

		public ScoreBoard(SpriteFont font)
		{
			_font = font;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			spriteBatch.DrawString(_font, "Score: " + Score, new Vector2(600, 150), Color.White);
		}

		public void Update(GameTime gameTime)
		{
		}
	}
}
