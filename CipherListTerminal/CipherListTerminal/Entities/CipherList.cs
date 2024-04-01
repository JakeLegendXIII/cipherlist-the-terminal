using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CipherListTerminal.Entities
{
	internal class CipherList : IGameEntity
	{
		public int DrawOrder => 0;
		public int PointValue { get; private set; }
		public int NumberOfValues { get; private set; }
		public string[] CipherListValues { get; private set; }

		private string[] _possibleValues;
		private Random _random = new Random();
		private SpriteFont _font;

		public CipherList(int pointValue, int numberOfValues, string[] possibleValues, SpriteFont font)
		{
			PointValue = pointValue;
			NumberOfValues = numberOfValues;
			CipherListValues = new string[NumberOfValues];
			_possibleValues = possibleValues;
			_font = font;

			// Initialize the CipherList
			for (int i = 0; i < NumberOfValues; i++)
			{
				int randomIndex = _random.Next(0, possibleValues.Length - 1);
				CipherListValues[i] = possibleValues[randomIndex];
			}
			
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			for (int i = 0; i < NumberOfValues; i++)
			{
				string text = CipherListValues[i];
				Vector2 position = new Vector2(500 + i * 50, 100);
				spriteBatch.DrawString(_font, text, position, Color.White);
			}
		}

		public void Update(GameTime gameTime)
		{

		}
	}	
}
