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

		public MatrixState State { get; set; }

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

		private int _selectedRowIndex = -1;
		private int _selectedColumnIndex = -1;

		// Highlight color
		Color highlightColor = new Color(255, 255, 0, 128); // Semi-transparent yellow

		public PuzzleMatrix(SpriteFont font, string[] possibleValues)
		{
			_font = font;
			_possibleValues = possibleValues;
			_matrixWidth = _cellWidth * 6;
			_matrixHeight = _cellHeight * 6;
			State = MatrixState.FirstSelection;

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
			int highlightCell = -1;
			Rectangle highlightRectangle;

			Vector2 transformedMousePosition = InputManager.GetTransformedMousePosition();

			if (State == MatrixState.FirstSelection)
			{				
				if (transformedMousePosition.X >= 0 && transformedMousePosition.X < _matrixWidth)
				{
					if (transformedMousePosition.Y >= 0 && transformedMousePosition.Y < _matrixHeight)
					{
						if (State == MatrixState.Vertical || State == MatrixState.FirstSelection)
						{
							highlightColumn = (int)(transformedMousePosition.X / _cellWidth);
							highlightCell = (int)(transformedMousePosition.X / _cellWidth);
						}
						else
						{
							highlightColumn = (int)(transformedMousePosition.Y / _cellHeight);
						}
					}
				}

				if (highlightColumn >= 0)
				{

					highlightRectangle = new Rectangle((_startX + highlightColumn * _cellWidth) - GetScaleValue(scale),
					_startY - GetScaleValue(scale), _cellWidth, _matrixHeight);

					RectangleSprite.DrawRectangle(_spriteBatch, highlightRectangle, highlightColor, 6);
				}
			}
			else if (State == MatrixState.Vertical)
			{
				highlightColumn = _selectedColumnIndex;

				highlightRectangle = new Rectangle((_startX + highlightColumn * _cellWidth) - GetScaleValue(scale),
					_startY - GetScaleValue(scale), _cellWidth, _matrixHeight);

				RectangleSprite.DrawRectangle(_spriteBatch, highlightRectangle, highlightColor, 6);

				if (transformedMousePosition.X >= 0 && transformedMousePosition.X < _matrixWidth)
				{
					if (transformedMousePosition.Y >= 0 && transformedMousePosition.Y < _matrixHeight)
					{						
						highlightCell = (int)(transformedMousePosition.Y / _cellWidth);
					}
				}
				
			}
			else if (State == MatrixState.Horizontal)
			{
				highlightColumn = _selectedRowIndex;

				highlightRectangle = new Rectangle(_startX - GetScaleValue(scale),
							(_startY + highlightColumn * _cellHeight) - GetScaleValue(scale), _matrixWidth, _cellHeight);

				RectangleSprite.DrawRectangle(_spriteBatch, highlightRectangle, highlightColor, 6);	
								
				if (transformedMousePosition.X >= 0 && transformedMousePosition.X < _matrixWidth)
				{
					if (transformedMousePosition.Y >= 0 && transformedMousePosition.Y < _matrixHeight)
					{
						highlightCell = (int)(transformedMousePosition.X / _cellWidth);
					}
				}
			}

			// Draw the matrix
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					Color color = Color.White;
					if (State == MatrixState.FirstSelection && i == 0 && j == highlightColumn)
					{
						color = Color.LightGreen;
					}
					else if (State == MatrixState.Vertical && _selectedColumnIndex == j)
					{
						color = Color.LightGreen;
					}
					else if (State == MatrixState.Horizontal && _selectedRowIndex == i)
					{
						color = Color.LightGreen;
					}
					string text = _matrix[i, j];
					Vector2 position = new Vector2(100 + j * 50, 100 + i * 50);
					_spriteBatch.DrawString(_font, text, position, color);

					if (State == MatrixState.FirstSelection && i == 0 && j == highlightCell)
					{
						RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle((int)position.X - GetScaleValue(scale),
																					(int)position.Y - GetScaleValue(scale), _cellWidth, _cellHeight), Color.Teal, 6);
					}
					if (State == MatrixState.Horizontal && i == _selectedRowIndex && j == highlightCell)
					{
						RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle((int)position.X - GetScaleValue(scale),
														(int)position.Y - GetScaleValue(scale), _cellWidth, _cellHeight), Color.Teal, 6);
					}
					if (State == MatrixState.Vertical && i == highlightCell && j == _selectedColumnIndex)
					{
						RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle((int)position.X - GetScaleValue(scale),
																					(int)position.Y - GetScaleValue(scale), _cellWidth, _cellHeight), Color.Teal, 6);
					}
				}
			}

			//_spriteBatch.DrawString(_font, "State: " + State, new Vector2(100, 400), Color.White);
			//_spriteBatch.DrawString(_font, "ScaleValue: " + GetScaleValue(scale), new Vector2(100, 450), Color.White);
			//_spriteBatch.DrawString(_font, "Currently Selected Value: " + CurrentlySelectedValue, new Vector2(100, 500), Color.White);
			//_spriteBatch.DrawString(_font, "Selected Row: " + _selectedRowIndex, new Vector2(100, 550), Color.White);
			//_spriteBatch.DrawString(_font, "Selected Column: " + _selectedColumnIndex, new Vector2(100, 600), Color.White);
		}

		public void Update(GameTime gameTime)
		{
			var mouseState = InputManager.GetTransformedMousePosition();
			if (InputManager.IsLeftMouseButtonDown() &&
				 mouseState.X >= 0 && mouseState.X < _matrixWidth &&
				 mouseState.Y >= 0 && mouseState.Y < _matrixHeight)
			{
				int columnIndex = (int)(mouseState.X / _cellWidth);
				int rowIndex = (int)(mouseState.Y / _cellHeight);
				bool select = false;

				if (State == MatrixState.FirstSelection)
				{
					if (columnIndex >= 0 && rowIndex == 0)
					{
						if (_matrix[rowIndex, columnIndex] != "__")
						{
							select = true;
							State = MatrixState.Vertical;
						}						
					}					
				}
				else if (State == MatrixState.Vertical)
				{
					if (rowIndex >= 0 && columnIndex == _selectedColumnIndex)
					{
						if (_matrix[rowIndex, columnIndex] != "__")
						{
							select = true;
							State = MatrixState.Horizontal;
						}						
						
					}					
				}
				else if (State == MatrixState.Horizontal)
				{
					if (columnIndex >= 0 && rowIndex == _selectedRowIndex)
					{
						if (_matrix[rowIndex, columnIndex] != "__")
						{
							select = true;
							State = MatrixState.Vertical;
						}						
					}					
				}
				
				if (select)
				{
					CurrentlySelectedValue = _matrix[rowIndex, columnIndex];
					_selectedRowIndex = rowIndex;
					_selectedColumnIndex = columnIndex;
					_matrix[rowIndex, columnIndex] = "__";

					MatrixSelectionEvent?.Invoke(CurrentlySelectedValue);
				}				
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

	public enum MatrixState
	{
		FirstSelection,
		Vertical,
		Horizontal
	}
}
