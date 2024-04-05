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
		private Texture2D _highlightTexture;

		private bool _currentlyVertical = true;

		private Random _random = new Random();
		// Highlight color
		Color highlightColor = new Color(255, 255, 0, 128); // Semi-transparent yellow

		public PuzzleMatrix(SpriteFont font, string[] possibleValues, Texture2D highlightTexture)
		{
			_font = font;
			_possibleValues = possibleValues;
			_highlightTexture = highlightTexture;

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

		public void Draw(SpriteBatch _spriteBatch, GameTime gameTime, Rectangle renderTarget, float scale)
		{
			int cellWidth = 50;
			int cellHeight = 50;
			int startX = 100;
			int startY = 100;

			int highlightColumn = -1;
			MouseState mouseState = InputManager.GetMousePosition();

			Vector2 transformedMousePosition = new Vector2((mouseState.X - (renderTarget.X + (100 * scale ))) / scale,
														(mouseState.Y - (renderTarget.Y + (100 * scale))) / scale);

			_spriteBatch.DrawString(_font, "Scale: " + scale.ToString(), new Vector2(600, 50), Color.White);

			if (transformedMousePosition.X >= 0 && transformedMousePosition.X < 6 * cellWidth)
			{
				highlightColumn = (int)(transformedMousePosition.X / cellWidth);
			}

			if (highlightColumn >= 0)
			{
				// Currently an issue where it does not dynamically scale with the matrix
				// Need the mouse position to know about the RenderTarget2D position
				Rectangle highlightRectangle = new Rectangle(startX + highlightColumn * cellWidth, startY, cellWidth, cellHeight * 6);
				_spriteBatch.Draw(_highlightTexture, highlightRectangle, highlightColor);
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
