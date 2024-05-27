using CipherListTerminal.Core;
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
		private string[,] _matrix = new string[6, 6];
		private string[] _possibleValues;
		private SpriteFont _font;
		private Texture2D _matrixUI;

		public MatrixState State { get; set; }
		private InputStates CurrentInputState;

		public string CurrentlySelectedValue { get; private set; } = "__";

		public delegate void MatrixSelectionEventHandler(string selectedValue);
		public event MatrixSelectionEventHandler MatrixSelectionEvent;

		private Random _random = new Random();
		private int _cellWidth = 50;
		private int _cellHeight = 50;
		private int _startX = 200;
		private int _startY = 290;
		private int _matrixWidth;
		private int _matrixHeight;

		private int _selectedRowIndex = -1;
		private int _selectedColumnIndex = -1;
		private int _displayRowIndex = -1;
		private int _displayColumnIndex = -1;
		private int _highlightColumn = -1;
		private int _highlightCell = -1;
		private bool thumbstickMoved = false;

		// Highlight color
		Color highlightColor = new Color(255, 255, 0, 128); // Semi-transparent yellow

		public PuzzleMatrix(SpriteFont font, Texture2D matrixUI, string[] possibleValues)
		{
			_font = font;
			_matrixUI = matrixUI;
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
			// Draw the Background UI			
			_spriteBatch.Draw(_matrixUI, new Vector2(_startX - 30, _startY - 75), null,
				Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			_spriteBatch.DrawString(_font, "Matrix", new Vector2(_startX + 100, _startY - 65), Color.White);
			_spriteBatch.DrawString(_font, $"display info: col {_displayColumnIndex} row {_displayRowIndex}", new Vector2(50, 10), Color.White);
			_spriteBatch.DrawString(_font, $"selected info: col {_selectedColumnIndex} row {_selectedRowIndex}", new Vector2(150, 10), Color.White);
			_spriteBatch.DrawString(_font, $"highlight info: col {_highlightColumn}  cell{_highlightCell}", new Vector2(50, 40), Color.White);


			Rectangle highlightRectangle;

			Vector2 transformedMousePosition = InputManager.GetTransformedMousePosition(_startX, _startY);
			GamePadState gamePadState = InputManager.GetGamePadState();

			if (State == MatrixState.FirstSelection)
			{
				if (CurrentInputState == InputStates.MouseKeyboard)
				{
					if (transformedMousePosition.X >= 0 && transformedMousePosition.X < _matrixWidth)
					{
						if (transformedMousePosition.Y >= 0 && transformedMousePosition.Y < _matrixHeight)
						{
							if (State == MatrixState.Vertical || State == MatrixState.FirstSelection)
							{
								_highlightColumn = (int)(transformedMousePosition.X / _cellWidth);
								_highlightCell = (int)(transformedMousePosition.X / _cellWidth);
							}
							else
							{
								_highlightColumn = (int)(transformedMousePosition.Y / _cellHeight);
							}
						}
					}
				}
				else if (CurrentInputState == InputStates.GamePad)
				{
					if (InputManager.IsGamePadConnected())
					{
						if (_highlightColumn == -1)
						{
							_highlightColumn = 0;
						}
						if (_highlightCell == -1)
						{
							_highlightCell = 0;
						}
						if (_displayRowIndex == -1)
						{
							_displayRowIndex = 0;
						}
						if (_displayColumnIndex == -1)
						{
							_displayColumnIndex = 0;
						}

						HandleGamePadLeftRightColumn(gamePadState);						
					}
				}

				if (_highlightColumn >= 0)
				{

					highlightRectangle = new Rectangle((_startX + _highlightColumn * _cellWidth) - GetScaleValue(scale),
					_startY - GetScaleValue(scale), _cellWidth, _matrixHeight);

					RectangleSprite.DrawRectangle(_spriteBatch, highlightRectangle, highlightColor, 6);
				}
			}
			else if (State == MatrixState.Vertical)
			{
				_highlightColumn = _selectedColumnIndex;

				highlightRectangle = new Rectangle((_startX + _highlightColumn * _cellWidth) - GetScaleValue(scale),
					_startY - GetScaleValue(scale), _cellWidth, _matrixHeight);

				RectangleSprite.DrawRectangle(_spriteBatch, highlightRectangle, highlightColor, 6);

				if (CurrentInputState == InputStates.MouseKeyboard)
				{
					if (transformedMousePosition.X >= 0 && transformedMousePosition.X < _matrixWidth)
					{
						if (transformedMousePosition.Y >= 0 && transformedMousePosition.Y < _matrixHeight)
						{
							_highlightCell = (int)(transformedMousePosition.Y / _cellWidth);
						}
					}
				}
				else if (CurrentInputState == InputStates.GamePad)
				{
					_displayColumnIndex = _selectedColumnIndex;

					HandleGamePadUpDown(gamePadState);				
				}
			}
			else if (State == MatrixState.Horizontal)
			{
				_highlightColumn = _selectedRowIndex;

				highlightRectangle = new Rectangle(_startX - GetScaleValue(scale),
							(_startY + _highlightColumn * _cellHeight) - GetScaleValue(scale), _matrixWidth, _cellHeight);

				RectangleSprite.DrawRectangle(_spriteBatch, highlightRectangle, highlightColor, 6);

				if (CurrentInputState == InputStates.MouseKeyboard)
				{
					if (transformedMousePosition.X >= 0 && transformedMousePosition.X < _matrixWidth)
					{
						if (transformedMousePosition.Y >= 0 && transformedMousePosition.Y < _matrixHeight)
						{
							_highlightCell = (int)(transformedMousePosition.X / _cellWidth);
						}
					}
				}
				else if (CurrentInputState == InputStates.GamePad)
				{
					_displayRowIndex = _selectedRowIndex;

					HandleGamePadLeftRightCell(gamePadState);
				}				
			}

			// Draw the matrix
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					Color color = Color.White;
					if (State == MatrixState.FirstSelection && i == 0 && j == _highlightColumn)
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
					Vector2 position = new Vector2(_startX + j * 50, _startY + i * 50);
					_spriteBatch.DrawString(_font, text, position, color);

					if (State == MatrixState.FirstSelection && i == 0 && j == _highlightCell)
					{
						RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle((int)position.X - GetScaleValue(scale),
																					(int)position.Y - GetScaleValue(scale), _cellWidth, _cellHeight), Color.Teal, 6);
					}
					if (State == MatrixState.Horizontal && i == _selectedRowIndex && j == _highlightCell)
					{
						RectangleSprite.DrawRectangle(_spriteBatch, new Rectangle((int)position.X - GetScaleValue(scale),
														(int)position.Y - GetScaleValue(scale), _cellWidth, _cellHeight), Color.Teal, 6);
					}
					if (State == MatrixState.Vertical && i == _highlightCell && j == _selectedColumnIndex)
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

		public void Update(GameTime gameTime, InputStates inputState)
		{
			CurrentInputState = inputState;

			if (inputState == InputStates.MouseKeyboard)
			{
				Vector2 mouseState = InputManager.GetTransformedMousePosition(_startX, _startY);

				_displayColumnIndex = (int)(mouseState.X / _cellWidth);
				_displayRowIndex = (int)(mouseState.Y / _cellHeight);

				if (InputManager.IsLeftMouseButtonDown() &&
					 mouseState.X >= 0 && mouseState.X < _matrixWidth &&
					 mouseState.Y >= 0 && mouseState.Y < _matrixHeight)
				{
					ManageSelectedInput();
				}
			}
			else if (inputState == InputStates.GamePad)
			{
				if (InputManager.IsGamePadConnected())
				{
					//_displayColumnIndex = _highlightColumn;
					//_displayRowIndex = _highlightCell;					

					if (InputManager.IsGamePadButtonPressed(Buttons.A))
					{
						ManageSelectedInput();					
					}
				}
			}
		}

		private void ManageSelectedInput()
		{
			bool select = false;

			if (State == MatrixState.FirstSelection)
			{
				if (_displayColumnIndex >= 0 && _displayRowIndex == 0)
				{
					if (_matrix[_displayRowIndex, _displayColumnIndex] != "__")
					{
						select = true;
						State = MatrixState.Vertical;
					}
				}
			}
			else if (State == MatrixState.Vertical)
			{
				if (_displayRowIndex >= 0 && _displayColumnIndex == _selectedColumnIndex)
				{
					if (_matrix[_displayRowIndex, _displayColumnIndex] != "__")
					{
						select = true;
						State = MatrixState.Horizontal;
					}

				}
			}
			else if (State == MatrixState.Horizontal)
			{
				if (_displayColumnIndex >= 0 && _displayRowIndex == _selectedRowIndex)
				{
					if (_matrix[_displayRowIndex, _displayColumnIndex] != "__")
					{
						select = true;
						State = MatrixState.Vertical;
					}
				}
			}

			if (select)
			{
				CurrentlySelectedValue = _matrix[_displayRowIndex, _displayColumnIndex];
				_selectedRowIndex = _displayRowIndex;
				_selectedColumnIndex = _displayColumnIndex;

				//if (State == MatrixState.FirstSelection)
				//{
				//	_highlightColumn = _selectedRowIndex;
				//	_highlightCell = _selectedColumnIndex;
				//}
				//else if (State == MatrixState.Horizontal)
				//{

				//}
				//else if (State == MatrixState.Vertical)
				//{

				//}

				_matrix[_displayRowIndex, _displayColumnIndex] = "__";

				MatrixSelectionEvent?.Invoke(CurrentlySelectedValue);
			}
		}

		private void HandleGamePadUpDown(GamePadState gamePadState)
		{
			if (InputManager.IsGamePadButtonPressed(Buttons.DPadUp))
			{
				GamePadMoveHighlightCellUp();
			}

			if (InputManager.IsGamePadButtonPressed(Buttons.DPadDown))
			{
				GamePadMoveHighlightCellDown();
			}

			// Check if the thumbstick is moved to the up or down
			if (Math.Abs(gamePadState.ThumbSticks.Left.Y) > 0.5f)
			{
				if (!thumbstickMoved)
				{
					if (gamePadState.ThumbSticks.Left.Y > 0)
					{
						GamePadMoveHighlightCellUp();
					}
					else if (gamePadState.ThumbSticks.Left.Y < 0)
					{
						GamePadMoveHighlightCellDown();
					}
					thumbstickMoved = true;
				}
			}
			else
			{
				thumbstickMoved = false;
			}
		}

		private void GamePadMoveHighlightCellDown()
		{
			if (_highlightCell < 5)
			{
				_highlightCell++;
			}
			else if (_highlightCell >= 5)
			{
				_highlightCell = 0;
			}

			if (_displayRowIndex < 5)
			{
				_displayRowIndex++;
			}
			else if (_displayRowIndex >= 5)
			{		
				_displayRowIndex = 0;
			}
		}

		private void GamePadMoveHighlightCellUp()
		{
			if (_highlightCell > 0)
			{
				_highlightCell--;
			}
			else if (_highlightCell <= 0)
			{
				_highlightCell = 5;
			}

			if (_displayRowIndex > 0)
			{
				_displayRowIndex--;
			}
			else if (_displayRowIndex <= 0)
			{
				_displayRowIndex = 5;
			}
		}

		private void HandleGamePadLeftRightColumn(GamePadState gamePadState)
		{
			if (InputManager.IsGamePadButtonPressed(Buttons.DPadLeft))
			{
				MoveHighlightLeft();
			}

			if (InputManager.IsGamePadButtonPressed(Buttons.DPadRight))
			{
				MoveHighlightRight();
			}

			// Check if the thumbstick is moved to the right or left
			if (Math.Abs(gamePadState.ThumbSticks.Left.X) > 0.5f)
			{
				if (!thumbstickMoved)
				{
					if (gamePadState.ThumbSticks.Left.X > 0)
					{
						MoveHighlightRight();
					}
					else if (gamePadState.ThumbSticks.Left.X < 0)
					{
						MoveHighlightLeft();

					}
					thumbstickMoved = true;
				}
			}
			else
			{
				thumbstickMoved = false;
			}
		}		

		private void MoveHighlightLeft()
		{
			if (_highlightColumn > 0)
			{
				_highlightColumn--;
				_highlightCell--;
			}
			else if (_highlightColumn <= 0)
			{
				_highlightColumn = 5;
				_highlightCell = 5;
			}

			if (_displayColumnIndex > 0)
			{
				_displayColumnIndex--;
			}
			else if (_displayColumnIndex <= 0)
			{
				_displayColumnIndex = 5;
			}
		}

		private void MoveHighlightRight()
		{
			if (_highlightColumn < 5)
			{
				_highlightColumn++;
				_highlightCell++;
			}
			else if (_highlightColumn >= 5)
			{
				_highlightColumn = 0;
				_highlightCell = 0;
			}

			if (_displayColumnIndex < 5)
			{
				_displayColumnIndex++;
			}
			else if (_displayColumnIndex >= 5)
			{
				_displayColumnIndex = 0;
			}
		}

		private void HandleGamePadLeftRightCell(GamePadState gamePadState)
		{
			if (InputManager.IsGamePadButtonPressed(Buttons.DPadLeft))
			{
				MoveHighlightCellLeft();
			}

			if (InputManager.IsGamePadButtonPressed(Buttons.DPadRight))
			{
				MoveHighlightCellRight();
			}

			// Check if the thumbstick is moved to the right or left
			if (Math.Abs(gamePadState.ThumbSticks.Left.X) > 0.5f)
			{
				if (!thumbstickMoved)
				{
					if (gamePadState.ThumbSticks.Left.X > 0)
					{
						MoveHighlightCellRight();
					}
					else if (gamePadState.ThumbSticks.Left.X < 0)
					{
						MoveHighlightCellLeft();

					}
					thumbstickMoved = true;
				}
			}
			else
			{
				thumbstickMoved = false;
			}
		}

		private void MoveHighlightCellLeft()
		{
			if (_highlightCell > 0)
			{				
				_highlightCell--;
			}
			else if (_highlightCell <= 0)
			{				
				_highlightCell = 5;
			}

			if (_displayColumnIndex > 0)
			{
				_displayColumnIndex--;
			}
			else if (_displayColumnIndex <= 0)
			{
				_displayColumnIndex = 5;
			}
		}

		private void MoveHighlightCellRight()
		{
			if (_highlightCell < 5)
			{				
				_highlightCell++;
			}
			else if (_highlightCell >= 5)
			{				
				_highlightCell = 0;
			}

			if (_displayColumnIndex < 5)
			{
				_displayColumnIndex++;
			}
			else if (_displayColumnIndex >= 5)
			{
				_displayColumnIndex = 0;
			}
		}

		private int GetScaleValue(float scale)
		{
			if (scale == 1f)
			{
				return (int)(10 * 1.5);
			}
			if (scale < 1f)
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
