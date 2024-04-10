using CipherListTerminal.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
		public bool FirstSelectionMade { get; private set; } = true;

		public string CurrentlySelectedValue { get; private set; } = "__";

		public delegate void MatrixSelectionEventHandler(string selectedValue);
		public event MatrixSelectionEventHandler MatrixSelectionEvent;

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
					highlightRectangle = new Rectangle((_startX + highlightColumn * _cellWidth) - GetScaleValue(scale),
					_startY - GetScaleValue(scale), _cellWidth, _matrixHeight);
				}
				else
				{
					highlightRectangle = new Rectangle(_startX - GetScaleValue(scale),
						(_startY + highlightColumn * _cellHeight) - GetScaleValue(scale), _matrixWidth, _cellHeight);
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

			_spriteBatch.DrawString(_font, "CurrentlyVertical: " + CurrentlyVertical, new Vector2(100, 400), Color.White);
			_spriteBatch.DrawString(_font, "ScaleValue: " + GetScaleValue(scale), new Vector2(100, 450), Color.White);
			_spriteBatch.DrawString(_font, "CurrentlySelectedValue: " + CurrentlySelectedValue, new Vector2(100, 500), Color.White);
		}

		public void Update(GameTime gameTime)
		{		
			var mouseState = InputManager.GetTransformedMousePosition();
			if (InputManager.IsLeftMouseButtonDown() &&
				 mouseState.X >= 0 && mouseState.X <_matrixWidth &&
				 mouseState.Y >= 0 && mouseState.Y < _matrixHeight)
			{
				if (FirstSelectionMade)
				{
					FirstSelectionMade = false;
				}
				else
				{
					CurrentlyVertical = !CurrentlyVertical;
				}
				
				int columnIndex = (int)(mouseState.X / _cellWidth);
				int rowIndex = (int)(mouseState.Y / _cellHeight);

				CurrentlySelectedValue = _matrix[rowIndex, columnIndex];

				MatrixSelectionEvent?.Invoke(CurrentlySelectedValue);
			}
		}		

		private int GetScaleValue(float scale)
		{
			if (scale == 1f)
			{
				return (int)(10 * 1.5);
			}

			return (int)(10 * scale);
		}
	}
}
