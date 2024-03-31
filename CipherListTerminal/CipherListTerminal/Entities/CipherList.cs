using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CipherListTerminal.Entities
{
	internal class CipherList : IGameEntity
	{
		public int DrawOrder => 0;
		public int PointValue { get; private set; }
		public string[] CipherListValues { get; private set; }

		public CipherList(string[] cipherListValues, int pointValue)
		{
			CipherListValues = cipherListValues;
			PointValue = pointValue;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
		}

		public void Update(GameTime gameTime)
		{

		}
	}	
}
