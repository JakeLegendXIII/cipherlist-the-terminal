using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CipherListTerminal.Entities
{
	public class Summary : IGameEntity
	{
		public int DrawOrder => 0;

		private Texture2D _summaryUI;

		public Summary(Texture2D summaryUI)
		{
			_summaryUI = summaryUI;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{

		}

		public void Update(GameTime gameTime)
		{

		}
	}
}
