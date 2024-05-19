using CipherListTerminal.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CipherListTerminal.Entities
{
	internal class CipherList : IGameEntity
	{
		public int PointValue { get; private set; }
		public int NumberOfValues { get; private set; }
		public string[] CipherListValues { get; private set; }
		public bool IsCompleted { get; set; } = false;

		private string[] _possibleValues;
		private Random _random = new Random();
		private SpriteFont _font;		
		private int _targetNumber;

		public CipherList(SpriteFont font, string[] possibleValues, int numberOfValues, int pointValue,
			int targetNumber)
		{
			PointValue = pointValue;
			NumberOfValues = numberOfValues;
			CipherListValues = new string[NumberOfValues];
			_possibleValues = possibleValues;
			_font = font;
			_targetNumber = targetNumber;

			// Initialize the CipherList
			for (int i = 0; i < NumberOfValues; i++)
			{
				int randomIndex = _random.Next(0, possibleValues.Length - 1);
				CipherListValues[i] = possibleValues[randomIndex];
			}
			
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			Color color = IsCompleted ? Color.Green : Color.White;			
			for (int i = 0; i < NumberOfValues; i++)
			{
				string text = CipherListValues[i];
				Vector2 position = new Vector2(600 + i * 50, 230 + (_targetNumber * 50));
				spriteBatch.DrawString(_font, text, position, color);
			}

			spriteBatch.DrawString(_font, PointValue.ToString() + " POINTS", new Vector2(850, 230 + (_targetNumber * 50)), color);
		}

		public void Update(GameTime gameTime, InputStates inputState) { }
	}	
}
