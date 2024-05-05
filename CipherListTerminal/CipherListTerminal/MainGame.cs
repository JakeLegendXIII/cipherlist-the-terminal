using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CipherListTerminal.Entities;
using System;
using CipherListTerminal.Input;
using CipherListTerminal.Core;
using System.Diagnostics;
using System.IO;

namespace CipherListTerminal
{
	public class MainGame : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private RenderTarget2D _renderTarget;
		private Rectangle _renderDestination;
		private float _scale = 1f;

		private int _nativeWidth = 1280;
		private int _nativeHeight = 800;
		private bool _isResizing;
		bool _isFullscreen = false;
		bool _isBorderless = false;
		int _width = 0;
		int _height = 0;

		private const string SAVE_FILE_NAME = "Save.dat";

		public GameStates GameState;
		public GameStates PreviousGameState;
		public SaveState CurrentSaveState;

		string[] possibleValues = { "1C", "55", "BD", "FF", "E9", "1C", "55" };
		string[] possibleValuesExpanded = { "1C", "55", "BD", "FF", "E9", "7A", "1C", "55" };

		private SpriteFont _armadaFont;
		private SpriteFont _arialFont;
		private SpriteFont _farawayFont;

		private Texture2D _menuLogo;
		private Texture2D _backgroundTexture;
		private Texture2D _matrixUI;
		private Texture2D _bufferUI;
		private Texture2D _scoreUI;
		private Texture2D _keysUI;

		private TerminalBuffer _terminalBuffer;
		private PuzzleMatrix _matrix;
		private CipherList _targetList1;
		private CipherList _targetList2;
		private CipherList _targetList3;
		private ScoreBoard _scoreBoard;
		private Summary _summary;

		private Random _random = new Random();

		private const float _completedDelay = 1f;
		private float _remainingDelay = _completedDelay;

		private const float _singlePuzzleTimer = 60f;
		private float _remainingPuzzleTime = _singlePuzzleTimer;
		private const int _puzzleCount = 10;
		private int _remainingPuzzles = _puzzleCount;

		public MainGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			_graphics.PreferredBackBufferWidth = _nativeWidth;
			_graphics.PreferredBackBufferHeight = _nativeHeight;
			_graphics.ApplyChanges();

			Window.Title = "CipherList: The Terminal";
			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += OnClientSizeChanged;

			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			CurrentSaveState = LoadSaveState();
			base.Initialize();
			CalculateRenderDestination();
			GameState = GameStates.Menu;
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_renderTarget = new RenderTarget2D(GraphicsDevice, _nativeWidth, _nativeHeight);

			_armadaFont = Content.Load<SpriteFont>("Fonts/ArmadaBold");
			_arialFont = Content.Load<SpriteFont>("Fonts/Arial");
			_farawayFont = Content.Load<SpriteFont>("Fonts/Faraway");

			_menuLogo = Content.Load<Texture2D>("Sprites/RoughMenu");
			_backgroundTexture = Content.Load<Texture2D>("Sprites/RoughBG3");
			_matrixUI = Content.Load<Texture2D>("Sprites/MatrixUI");
			_bufferUI = Content.Load<Texture2D>("Sprites/BufferUI");
			_scoreUI = Content.Load<Texture2D>("Sprites/ScoreUI");
			_keysUI = Content.Load<Texture2D>("Sprites/KeysUI");
		}

		protected override void Update(GameTime gameTime)
		{
			InputManager.Update(_renderDestination, _scale);
			if (InputManager.IsKeyDown(Keys.F11))
			{
				// _graphics.ToggleFullScreen();
				ToggleFullscreen();
				// CalculateRenderDestination();
			}

			if (InputManager.IsKeyPressed(Keys.F8))
			{
				ResetSaveState();
			}

			if (GameState == GameStates.Menu)
			{
				if (InputManager.IsGamePadButtonPressed(Buttons.Back) || InputManager.IsKeyPressed(Keys.Escape))
					Exit();

				if (InputManager.IsKeyPressed(Keys.Enter))
				{
					
					SetupNewPuzzle();
					GameState = GameStates.FreePlay;
					PreviousGameState = GameStates.Menu;
					SetupScoreBoard();
				}

				if (InputManager.IsKeyPressed(Keys.R))
				{					
					SetupNewPuzzle();
					GameState = GameStates.SinglePuzzleTimed;
					PreviousGameState = GameStates.Menu;
					SetupScoreBoard();
				}
			}
			else if (GameState == GameStates.FreePlay)
			{
				if (InputManager.IsGamePadButtonPressed(Buttons.Back) || InputManager.IsKeyPressed(Keys.Escape))
				{
					CheckScore();
					GameState = GameStates.Summary;
					PreviousGameState = GameStates.FreePlay;
					SetupSummary();
				}

				_matrix.Update(gameTime);

				if (InputManager.IsKeyPressed(Keys.F5))
				{
					SetupNewPuzzle();
				}

				if (_terminalBuffer.IsCompleted)
				{
					_remainingDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;

					if (_remainingDelay <= 0)
					{
						_remainingDelay = _completedDelay;
						_terminalBuffer.IsCompleted = false;
						SetupNewPuzzle();
					}
				}
			}
			else if (GameState == GameStates.SinglePuzzleTimed)
			{
				if (InputManager.IsGamePadButtonPressed(Buttons.Back) || InputManager.IsKeyPressed(Keys.Escape))
				{
					CheckScore();
					GameState = GameStates.Summary;
					PreviousGameState = GameStates.SinglePuzzleTimed;
					SetupSummary();
				}

				_matrix.Update(gameTime);

				if (InputManager.IsKeyPressed(Keys.F5))
				{
					SetupNewPuzzle();
					_remainingPuzzles--;
				}

				_remainingPuzzleTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

				if (_remainingPuzzleTime <= 0)
				{
					_remainingPuzzleTime = _singlePuzzleTimer;
					SetupNewPuzzle();
					_remainingPuzzles--;
				}

				if (_terminalBuffer.IsCompleted)
				{
					_remainingDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;

					if (_remainingDelay <= 0)
					{
						_remainingDelay = _completedDelay;
						_remainingPuzzleTime = _singlePuzzleTimer;
						_terminalBuffer.IsCompleted = false;
						SetupNewPuzzle();
						_remainingPuzzles--;
					}
				}

				if (_remainingPuzzles <= 0)
				{
					CheckScore();
					SetupSummary();
					GameState = GameStates.Summary;
					PreviousGameState = GameStates.SinglePuzzleTimed;
					
					_remainingPuzzles = _puzzleCount;
				}
			}
			else if (GameState == GameStates.Summary)
			{
				_summary.Update(gameTime);

				if (InputManager.IsGamePadButtonPressed(Buttons.Back) || InputManager.IsKeyPressed(Keys.Escape))
				{
					GameState = GameStates.Menu;
					SetupNewPuzzle();
					SetupScoreBoard();
				}

				if (InputManager.IsKeyPressed(Keys.Enter))
				{
					GameState = PreviousGameState;
					PreviousGameState = GameStates.Summary;
				}
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			// Add RenterTarget2D to allow screen resizing
			GraphicsDevice.SetRenderTarget(_renderTarget);

			GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin(samplerState: SamplerState.PointClamp);

			_spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _nativeWidth, _nativeHeight), Color.White);

			if (GameState == GameStates.Menu)
			{
				_spriteBatch.Draw(_menuLogo, new Rectangle((_renderTarget.Width / 2) - 264, (_renderTarget.Height / 2) - 250, 500, 200), Color.White);
			}
			else if (GameState == GameStates.FreePlay || GameState == GameStates.SinglePuzzleTimed)
			{
				// _spriteBatch.DrawString(_font, "Scale: " + _scale.ToString(), new Vector2(600, 100), Color.White);
				_matrix.Draw(_spriteBatch, gameTime, _scale);
				_terminalBuffer.Draw(_spriteBatch, gameTime, _scale);

				_spriteBatch.Draw(_keysUI, new Vector2(580, 215), null,
									Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				_spriteBatch.DrawString(_armadaFont, "Target Keys:", new Vector2(590, 225), Color.White);


				_targetList1.Draw(_spriteBatch, gameTime, _scale);
				_targetList2.Draw(_spriteBatch, gameTime, _scale);
				_targetList3.Draw(_spriteBatch, gameTime, _scale);

				_scoreBoard.Draw(_spriteBatch, gameTime, _scale);
				if (GameState == GameStates.SinglePuzzleTimed)
				{
					_spriteBatch.DrawString(_armadaFont, "Puzzle Time Remaining: " + _remainingPuzzleTime.ToString(), new Vector2(650, 65), Color.White);
				}

			}
			else if (GameState == GameStates.Summary)
			{
				_summary.Draw(_spriteBatch, gameTime, _scale);
			}

			_spriteBatch.End();

			GraphicsDevice.SetRenderTarget(null);

			_spriteBatch.Begin(samplerState: SamplerState.PointClamp);
			_spriteBatch.Draw(_renderTarget, _renderDestination, Color.White);
			_spriteBatch.End();

			base.Draw(gameTime);
		}

		private void OnClientSizeChanged(object sender, EventArgs e)
		{
			if (!_isResizing && Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0)
			{				
				_isResizing = true;

				CalculateRenderDestination();

				_isResizing = false;
			}
		}

		private void CalculateRenderDestination()
		{
			Point size = GraphicsDevice.Viewport.Bounds.Size;

			float scaleX = (float)size.X / _renderTarget.Width;
			float scaleY = (float)size.Y / _renderTarget.Height;

			_scale = Math.Min(scaleX, scaleY);

			_renderDestination.Width = (int)(_renderTarget.Width * _scale);
			_renderDestination.Height = (int)(_renderTarget.Height * _scale);

			_renderDestination.X = (size.X - _renderDestination.Width) / 2;
			_renderDestination.Y = (size.Y - _renderDestination.Height) / 2;
		}

		private void SetupNewPuzzle()
		{
			_terminalBuffer = new TerminalBuffer(_armadaFont, _bufferUI);

			// Create the possible values for the Matrix and CipherLists
			int randomIndex = _random.Next(0, 99);
			string[] possibleValue;
			if (randomIndex > 80)
			{
				possibleValue = possibleValuesExpanded;
			}
			else
			{
				possibleValue = possibleValues;
			}

			// Create the starting Matrix
			_matrix = new PuzzleMatrix(_armadaFont, _matrixUI, possibleValue);

			_matrix.MatrixSelectionEvent += HandleSelectedMatrixEvent;

			// Create the target CipherLists using the possibleValues
			_targetList1 = new CipherList(_armadaFont, possibleValue, 3, 300, 1);
			_targetList2 = new CipherList(_armadaFont, possibleValue, 4, 450, 2);
			_targetList3 = new CipherList(_armadaFont, possibleValue, 5, 700, 3);
		}

		private void SetupScoreBoard()
		{
			_scoreBoard = new ScoreBoard(_armadaFont, _scoreUI);

			if (GameState == GameStates.FreePlay)
			{
				_scoreBoard.HighScore = CurrentSaveState.FreePlayHighScore;
			}
			else if (GameState == GameStates.SinglePuzzleTimed)
			{
				_scoreBoard.HighScore = CurrentSaveState.TimeTrialHighScore;
			}
		}

		private void SetupSummary()
		{
			_summary = new Summary(_matrixUI, _armadaFont);
			_summary.Score = _scoreBoard.Score;

			if (GameState == GameStates.FreePlay)
			{
				_summary.HighScore = CurrentSaveState.FreePlayHighScore;
				_summary.HighScoreDate = CurrentSaveState.FreePlayHighScoreDate;
			}
			else if (GameState == GameStates.SinglePuzzleTimed)
			{
				_summary.HighScore = CurrentSaveState.TimeTrialHighScore;
				_summary.HighScoreDate = CurrentSaveState.TimeTrialHighScoreDate;
			}
		}

		private void HandleSelectedMatrixEvent(string selectedValue)
		{
			_terminalBuffer.Text = ReplaceFirstOccurrence(_terminalBuffer.Text, "__", selectedValue);

			if (!_terminalBuffer.Text.StartsWith("__ __ __"))
			{
				if (!_terminalBuffer.Text.Contains("__"))
				{
					_terminalBuffer.IsCompleted = true;
				}

				if (!_targetList1.IsCompleted)
				{
					string cipher1Text = "";
					foreach (string cipher in _targetList1.CipherListValues)
					{
						cipher1Text += cipher + " ";
					}
					if (_terminalBuffer.Text.Contains(cipher1Text))
					{
						_targetList1.IsCompleted = true;
						_scoreBoard.Score += _targetList1.PointValue;
					}
				}

				if (!_targetList2.IsCompleted)
				{
					string cipher2Text = "";
					foreach (string cipher in _targetList2.CipherListValues)
					{
						cipher2Text += cipher + " ";
					}
					if (_terminalBuffer.Text.Contains(cipher2Text))
					{
						_targetList2.IsCompleted = true;
						_scoreBoard.Score += _targetList2.PointValue;
					}
				}

				if (!_targetList3.IsCompleted)
				{
					string cipher3Text = "";
					foreach (string cipher in _targetList3.CipherListValues)
					{
						cipher3Text += cipher + " ";
					}
					if (_terminalBuffer.Text.Contains(cipher3Text))
					{
						_targetList3.IsCompleted = true;
						_scoreBoard.Score += _targetList3.PointValue;
					}
				}
			}
		}

		private string ReplaceFirstOccurrence(string source, string find, string replace)
		{
			int place = source.IndexOf(find);
			if (place < 0)
			{
				// If the substring is not found, return the original string
				return source;
			}
			// Remove the substring from the original string and insert the new substring
			string result = source.Remove(place, find.Length).Insert(place, replace);
			return result;
		}

		private void CheckScore()
		{
			if (GameState == GameStates.FreePlay && _scoreBoard.Score > CurrentSaveState.FreePlayHighScore)
			{
				CurrentSaveState.FreePlayHighScore = _scoreBoard.Score;
				CurrentSaveState.FreePlayHighScoreDate = DateTime.Now;

				SaveGame();
			}
			else if (GameState == GameStates.SinglePuzzleTimed && _scoreBoard.Score > CurrentSaveState.TimeTrialHighScore)
			{
				CurrentSaveState.TimeTrialHighScore = _scoreBoard.Score;
				CurrentSaveState.TimeTrialHighScoreDate = DateTime.Now;

				SaveGame();
			}

		}

		public void SaveGame()
		{
			try
			{
				using (StreamWriter writer = new StreamWriter(SAVE_FILE_NAME))
				{
					string firstColumn = CurrentSaveState.FreePlayHighScore.ToString().PadRight(20);
					string secondColumn = CurrentSaveState.FreePlayHighScoreDate.ToString("MM/dd/yyyy").PadRight(10);
					string thirdColumn = CurrentSaveState.TimeTrialHighScore.ToString().PadRight(20);
					string fourthColumn = CurrentSaveState.TimeTrialHighScoreDate.ToString("MM/dd/yyyy").PadRight(10);

					writer.WriteLine($"{firstColumn}{secondColumn}{thirdColumn}{fourthColumn}");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("An error occurred while saving the game: " + ex.Message);
			}
		}

		private SaveState LoadSaveState()
		{
			SaveState saveState = new SaveState();

			try
			{
				using (StreamReader reader = new StreamReader(SAVE_FILE_NAME))
				{
					string line = reader.ReadLine();
					if (line != null)
					{
						saveState.FreePlayHighScore = int.Parse(line.Substring(0, 20).Trim());
						saveState.FreePlayHighScoreDate = DateTime.Parse(line.Substring(20, 10).Trim());
						saveState.TimeTrialHighScore = int.Parse(line.Substring(30, 20).Trim());
						saveState.TimeTrialHighScoreDate = DateTime.Parse(line.Substring(50, 10).Trim());
					}
				}

				return saveState;
			}
			catch (Exception ex)
			{
				Debug.WriteLine("An error occurred while loading the game: " + ex.Message);
				return saveState;
			}
		}

		private void ResetSaveState()
		{
			_scoreBoard.HighScore = 0;
			CurrentSaveState.FreePlayHighScore = 0;
			CurrentSaveState.FreePlayHighScoreDate = default(DateTime);

			SaveGame();
		}


		// Learn MonoGame how-to Fullscreen and Borderless code
		public void ToggleFullscreen()
		{
			bool oldIsFullscreen = _isFullscreen;

			if (_isBorderless)
			{
				_isBorderless = false;
			}
			else
			{
				_isFullscreen = !_isFullscreen;
			}

			ApplyFullscreenChange(oldIsFullscreen);
		}
		public void ToggleBorderless()
		{
			bool oldIsFullscreen = _isFullscreen;

			_isBorderless = !_isBorderless;
			_isFullscreen = _isBorderless;

			ApplyFullscreenChange(oldIsFullscreen);
		}

		private void ApplyFullscreenChange(bool oldIsFullscreen)
		{
			if (_isFullscreen)
			{
				if (oldIsFullscreen)
				{
					ApplyHardwareMode();
				}
				else
				{
					SetFullscreen();
				}
			}
			else
			{
				UnsetFullscreen();
			}
		}
		private void ApplyHardwareMode()
		{
			_graphics.HardwareModeSwitch = !_isBorderless;
			_graphics.ApplyChanges();
		}
		private void SetFullscreen()
		{
			_width = Window.ClientBounds.Width;
			_height = Window.ClientBounds.Height;

			_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			_graphics.HardwareModeSwitch = !_isBorderless;

			_graphics.IsFullScreen = true;
			_graphics.ApplyChanges();
		}
		private void UnsetFullscreen()
		{
			_graphics.PreferredBackBufferWidth = _width;
			_graphics.PreferredBackBufferHeight = _height;
			_graphics.IsFullScreen = false;
			_graphics.ApplyChanges();
		}
	}
}
