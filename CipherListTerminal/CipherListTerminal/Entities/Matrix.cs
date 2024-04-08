using CipherListTerminal.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CipherListTerminal.Entities
{
	internal class PuzzleMatrix : IGameEntity
	{
		public int DrawOrder => 0;
		private string[,] _matrix = new string[6, 6];
		private string[] _possibleValues;
		private SpriteFont _font;

		private bool _currentlyVertical = false;

		private Random _random = new Random();
		// Highlight color
		Color highlightColor = new Color(255, 255, 0, 128); // Semi-transparent yellow

		public PuzzleMatrix(SpriteFont font, string[] possibleValues)
		{
			_font = font;
			_possibleValues = possibleValues;

			// Initialize the matrix
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					int randomIndex = _random.Next(0, possibleValues.Length - 1);
					_matrix[i, j] = possibleValues[randomIndex];
				}
			}
		}

		public void Draw(SpriteBatch _spriteBatch, GameTime gameTime, float scale)
		{
			int cellWidth = 50;
			int cellHeight = 50;
			int startX = 100;
			int startY = 100;

			int highlightColumn = -1;

			Vector2 transformedMousePosition = InputManager.GetTransformedMousePosition();
						
			if (transformedMousePosition.X >= 0 && transformedMousePosition.X < 6 * cellWidth)
			{
				if (transformedMousePosition.Y >= 0 && transformedMousePosition.Y < 6 * cellHeight)
				{
					if (_currentlyVertical)
					{
						highlightColumn = (int)(transformedMousePosition.X / cellWidth);
					}
					else
					{
						highlightColumn = (int)(transformedMousePosition.Y / cellHeight);
					}					
				}					
			}

			if (highlightColumn >= 0)
			{
				Rectangle highlightRectangle;
				if (_currentlyVertical)
				{
					highlightRectangle = new Rectangle((startX + highlightColumn * cellWidth) - (int)(10 * scale),
					startY - (int)(10 * scale), cellWidth, cellHeight * 6);
				}
				else
				{
					highlightRectangle = new Rectangle(startX - (int)(10 * scale),
						(startY + highlightColumn * cellHeight) - (int)(10 * scale), cellWidth * 6, cellHeight);
				}
				
				RectangleSprite.DrawRectangle(_spriteBatch, highlightRectangle, highlightColor, 6);				
			}

			// Draw the matrix
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					string text = _matrix[i, j];
					Vector2 position = new Vector2(100 + j * 50, 100 + i * 50);
					_spriteBatch.DrawString(_font, text, position, Color.White);
				}
			}
		}

		public void Update(GameTime gameTime)
		{
			
		}
	}
}
