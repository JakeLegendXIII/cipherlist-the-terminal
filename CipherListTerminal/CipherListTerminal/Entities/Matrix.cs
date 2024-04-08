using CipherListTerminal.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CipherListTerminal.Entities
{
	internal class PuzzleMatrix : IGameEntity
	{
		public int DrawOrder => 0;
		private string[,] _matrix = new string[6, 6];
		private string[] _possibleValues;
		private SpriteFont _font;

		public bool CurrentlyVertical { get; private set; }

		private Random _random = new Random();
		private int _cellWidth = 50;	
		private int _cellHeight = 50;
		private int _startX = 100;
		private int _startY = 100;
		private int _matrixWidth;
		private int _matrixHeight;

		// Highlight color
		Color highlightColor = new Color(255, 255, 0, 128); // Semi-transparent yellow

		public PuzzleMatrix(SpriteFont font, string[] possibleValues)
		{
			_font = font;
			_possibleValues = possibleValues;
			CurrentlyVertical = true;
			_matrixWidth = _cellWidth * 6;
			_matrixHeight = _cellHeight * 6;

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
			int highlightColumn = -1;

			Vector2 transformedMousePosition = InputManager.GetTransformedMousePosition();
						
			if (transformedMousePosition.X >= 0 && transformedMousePosition.X < _matrixWidth)
			{
				if (transformedMousePosition.Y >= 0 && transformedMousePosition.Y < _matrixHeight)
				{
					if (CurrentlyVertical)
					{
						highlightColumn = (int)(transformedMousePosition.X / _cellWidth);
					}
					else
					{
						highlightColumn = (int)(transformedMousePosition.Y / _cellHeight);
					}					
				}					
			}

			if (highlightColumn >= 0)
			{
				Rectangle highlightRectangle;
				if (CurrentlyVertical)
				{
					highlightRectangle = new Rectangle((_startX + highlightColumn * _cellWidth) - (int)(10 * scale),
					_startY - (int)(10 * scale), _cellWidth, _matrixHeight);
				}
				else
				{
					highlightRectangle = new Rectangle(_startX - (int)(10 * scale),
						(_startY + highlightColumn * _cellHeight) - (int)(10 * scale), _matrixWidth, _cellHeight);
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
			MouseState mouseState = InputManager.GetMousePosition();

			if (mouseState.LeftButton == ButtonState.Pressed)
			{
				CurrentlyVertical = !CurrentlyVertical;
			}
		}
	}
}
