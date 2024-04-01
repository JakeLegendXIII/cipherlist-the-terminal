using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CipherListTerminal.Entities
{
	internal class CipherList : IGameEntity
	{
		public int DrawOrder => 0;
		public int PointValue { get; private set; }
		public int NumberOfValues { get; private set; }
		public string[] CipherListValues { get; private set; }

		private string[] _possibleValues;

		public CipherList(string[] cipherListValues, int pointValue, int numberOfValues, string[] possibleValues)
		{
			CipherListValues = cipherListValues;
			PointValue = pointValue;
			NumberOfValues = numberOfValues;
			_possibleValues = possibleValues;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{

		}

		public void Update(GameTime gameTime)
		{

		}
	}	
}
