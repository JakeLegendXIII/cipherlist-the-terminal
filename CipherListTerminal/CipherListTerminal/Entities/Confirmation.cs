using CipherListTerminal.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CipherListTerminal.Entities
{
	internal class Confirmation : IGameEntity
	{
		public int Score { get; set; } = 0;
		public int HighScore { get; set; }
		public DateTime HighScoreDate { get; set; }

		private Texture2D _confirmationUI;
		private SpriteFont _armadaFont;

		private int _confirmationUIPositionX = 260;
		private int _confirmationUIPositionY = 85;

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			
		}

		public void Update(GameTime gameTime, InputStates inputState)
		{
			
		}
	}
}
