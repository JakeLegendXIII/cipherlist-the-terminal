using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CipherListTerminal.Entities;
using System;
using CipherListTerminal.Input;
using CipherListTerminal.Core;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using CipherListTerminal.Sound;
using System.Collections.Generic;
using CipherListTerminal.Data;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

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

		private const string SETTINGS_FILE_NAME = "settings.json";

		public GameStates GameState;
		public GameStates PreviousGameState;
		public SettingsData SettingsData;
		public SettingsData DefaultSettingsData;
		public InputStates CurrentInputState;

		string[] possibleValues = { "1C", "55", "BD", "FF", "E9", "1C", "55" };
		string[] possibleValuesExpanded = { "1C", "55", "BD", "FF", "E9", "JK", "1C", "55" };

		private SpriteFont _armadaFont;
		private SpriteFont _farawayFont;

		private Texture2D _backgroundTexture;
		private Texture2D _spriteSheet;

		// Music
		private SoundEffect _losingLight;
		private SoundEffect _around;
		private SoundEffect _neonThump;

		//SFX
		private SoundEffect _flickingASwitch;
		private SoundEffect _buttonPress;
		private SoundEffect _positiveBlip;
		private SoundEffect _uiWrong;
		private SoundEffect _drop;

		private Effect _effect;

		private MainMenu _mainMenu;
		private TerminalBuffer _terminalBuffer;
		private PuzzleMatrix _matrix;
		private CipherList _targetList1;
		private CipherList _targetList2;
		private CipherList _targetList3;
		private ScoreBoard _scoreBoard;
		private Summary _summary;
		private InputStateIndicator _inputStateIndicator;
		private SettingsManager _settingsManager;
		private ButtonManager _buttonManager;
		private SoundManager _soundManager;

		private Random _random = new Random();

		private const float _completedDelay = 1f;
		private float _remainingDelay = _completedDelay;

		private const float _singlePuzzleTimer = 60f;
		private float _remainingPuzzleTime = _singlePuzzleTimer;
		private const int _puzzleCount = 10;
		private int _remainingPuzzles = _puzzleCount;
		private const float _timeTrialTimer = 300f;
		private float _remainingTime = _timeTrialTimer;
		private int _completedPuzzles = 0;

		public MainGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			LoadSettingsFile();

			if (SettingsData.settings.fullScreen)
			{
				_isFullscreen = SettingsData.settings.fullScreen;
				// SetFullscreen();
				_width = _nativeWidth;     // Window.ClientBounds.Width;
				_height = _nativeHeight;  // Window.ClientBounds.Height;

				_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
				_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
				_graphics.HardwareModeSwitch = !_isBorderless;

				_graphics.IsFullScreen = true;
				_graphics.ApplyChanges();
			}
			else
			{
				UnsetFullscreen();			
			}

			Window.Title = "CipherList: The Terminal";
			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += OnClientSizeChanged;

			IsMouseVisible = true;

			_renderTarget = new RenderTarget2D(GraphicsDevice, _nativeWidth, _nativeHeight);

			_effect = Content.Load<Effect>("Shaders/crt-lottes-mg");
			_effect.Parameters["brightboost"].SetValue(1.25f);
		}

		protected override void Initialize()
		{
			base.Initialize();

			CalculateRenderDestination();

			_mainMenu = new MainMenu(_spriteSheet, _armadaFont, _farawayFont, _buttonPress, _flickingASwitch);
			_mainMenu.MenuButtonSelectionEvent += OnMenuButtonSelection;
			GameState = GameStates.Menu;

			GamePadState gps = GamePad.GetState(PlayerIndex.One);
			if (gps.IsConnected)
				CurrentInputState = InputStates.GamePad;
			else
				CurrentInputState = InputStates.MouseKeyboard;

			_inputStateIndicator = new InputStateIndicator(_armadaFont, CurrentInputState, _spriteSheet);
			_buttonManager = new ButtonManager(_spriteSheet, _armadaFont, CurrentInputState, GameState, _buttonPress);

			DefaultSettingsData = CreateDefaultSettings();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_armadaFont = Content.Load<SpriteFont>("Fonts/ArmadaBold16");
			_farawayFont = Content.Load<SpriteFont>("Fonts/Faraway16");

			_backgroundTexture = Content.Load<Texture2D>("Sprites/Background");
			_spriteSheet = Content.Load<Texture2D>("Sprites/spritesheetfinal");

			_flickingASwitch = Content.Load<SoundEffect>("SFX/flickingaswitch");
			_buttonPress = Content.Load<SoundEffect>("SFX/buttonpress");
			_positiveBlip = Content.Load<SoundEffect>("SFX/positiveblip");
			_uiWrong = Content.Load<SoundEffect>("SFX/uiwrong");
			_drop = Content.Load<SoundEffect>("SFX/rolanddrop");

			_losingLight = Content.Load<SoundEffect>("Music/LosingLight");
			_around = Content.Load<SoundEffect>("Music/Around");
			_neonThump = Content.Load<SoundEffect>("Music/Neon_Thump");

			_soundManager = new SoundManager();
			var losingLight = _losingLight.CreateInstance();
			losingLight.Volume = 0.5f;
			var around = _around.CreateInstance();
			around.Volume = 0.5f;
			var neonThump = _neonThump.CreateInstance();
			neonThump.Volume = 0.4f;			
			_soundManager.SetSoundtrack(new List<SoundEffectInstance>() { losingLight, around, neonThump });

			_settingsManager = new SettingsManager(_spriteSheet, _armadaFont);
		}

		protected override void Update(GameTime gameTime)
		{
			if (SettingsData.settings.music)
				_soundManager.PlaySoundtrack();
			else
				_soundManager.StopSoundtrack();

			InputManager.Update(_renderDestination, _scale);

			if (InputManager.IsKeyPressed(Keys.F10) || InputManager.IsGamePadButtonPressed(Buttons.LeftTrigger))
			{
				ToggleInputState();
			}

			if (InputManager.PreviousGamePadConnected() && !InputManager.IsGamePadConnected() && CurrentInputState == InputStates.GamePad)
			{
				ToggleInputState();
			}

			_inputStateIndicator.Update(gameTime, CurrentInputState);
			_buttonManager.Update(gameTime, CurrentInputState, GameState);

			if (GameState == GameStates.Menu)
			{
				_mainMenu.Update(gameTime, CurrentInputState);

				if (InputManager.IsGamePadButtonPressed(Buttons.Back) || InputManager.IsKeyPressed(Keys.Escape))
					Exit();

				if (InputManager.IsKeyPressed(Keys.F1))
				{
					SetupNewPuzzle();
					GameState = GameStates.FreePlay;
					PreviousGameState = GameStates.Menu;
					SetupScoreBoard();
				}

				if (InputManager.IsKeyPressed(Keys.F2))
				{
					SetupNewPuzzle();
					GameState = GameStates.SinglePuzzleTimed;
					PreviousGameState = GameStates.Menu;
					SetupScoreBoard();
				}

				if (InputManager.IsKeyPressed(Keys.F3))
				{
					SetupNewPuzzle();
					GameState = GameStates.TimeTrial;
					PreviousGameState = GameStates.Menu;
					SetupScoreBoard();
				}

				if (InputManager.IsKeyPressed(Keys.F4))
				{
					_settingsManager.SetSettingsData(SettingsData);
					GameState = GameStates.Settings;
					PreviousGameState = GameStates.Menu;					
				}
			}
			else if (GameState == GameStates.FreePlay)
			{
				if (InputManager.IsGamePadButtonPressed(Buttons.Back) || InputManager.IsKeyPressed(Keys.Escape))
				{
					CheckScore();
					SetupSummary();
					GameState = GameStates.Summary;
					PreviousGameState = GameStates.FreePlay;
				}

				_matrix.Update(gameTime, CurrentInputState);

				if (InputManager.IsKeyPressed(Keys.F5) || InputManager.IsGamePadButtonPressed(Buttons.RightTrigger))
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
					SetupSummary();
					GameState = GameStates.Summary;
					PreviousGameState = GameStates.SinglePuzzleTimed;
				}

				_matrix.Update(gameTime, CurrentInputState);

				if (InputManager.IsKeyPressed(Keys.F5) || InputManager.IsGamePadButtonPressed(Buttons.RightTrigger))
				{
					SetupNewPuzzle();
					_remainingPuzzles--;
					_remainingPuzzleTime = _singlePuzzleTimer;
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

					// _remainingPuzzles = _puzzleCount;
				}
			}
			else if (GameState == GameStates.TimeTrial)
			{
				if (InputManager.IsGamePadButtonPressed(Buttons.Back) || InputManager.IsKeyPressed(Keys.Escape))
				{
					CheckScore();
					SetupSummary();
					GameState = GameStates.Summary;
					PreviousGameState = GameStates.TimeTrial;
				}

				_matrix.Update(gameTime, CurrentInputState);

				if (InputManager.IsKeyPressed(Keys.F5) || InputManager.IsGamePadButtonPressed(Buttons.RightTrigger))
				{
					SetupNewPuzzle();
					_completedPuzzles++;
				}

				_remainingTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

				if (_terminalBuffer.IsCompleted)
				{
					_remainingDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;

					if (_remainingDelay <= 0)
					{
						_remainingDelay = _completedDelay;
						_terminalBuffer.IsCompleted = false;
						SetupNewPuzzle();
						_completedPuzzles++;
					}
				}

				if (_remainingTime <= 0)
				{
					CheckScore();
					SetupSummary();
					GameState = GameStates.Summary;
					PreviousGameState = GameStates.TimeTrial;

					_completedPuzzles = 0;
				}
			}
			else if (GameState == GameStates.Summary)
			{
				_summary.Update(gameTime, CurrentInputState);

				if (InputManager.IsGamePadButtonPressed(Buttons.Back) || InputManager.IsKeyPressed(Keys.Escape))
				{
					if (PreviousGameState == GameStates.SinglePuzzleTimed)
					{
						_remainingPuzzleTime = _singlePuzzleTimer;
						_remainingPuzzles = _puzzleCount;
					}

					if (PreviousGameState == GameStates.TimeTrial)
					{
						_remainingTime = _timeTrialTimer;
						_completedPuzzles = 0;
					}

					GameState = GameStates.Menu;
					PreviousGameState = GameStates.Summary;
					SetupNewPuzzle();
					SetupScoreBoard();
				}

				if (InputManager.IsKeyPressed(Keys.Enter) || InputManager.IsGamePadButtonPressed(Buttons.A))
				{					
					if (PreviousGameState == GameStates.TimeTrial && _remainingTime <= 0)
					{
						_remainingTime = _timeTrialTimer;
						_completedPuzzles = 0;

						SetupNewPuzzle();
						SetupScoreBoardPrevious();
					}

					if (PreviousGameState == GameStates.SinglePuzzleTimed && _remainingPuzzles <= 0)
					{
						_remainingPuzzleTime = _singlePuzzleTimer;
						_remainingPuzzles = _puzzleCount;

						SetupNewPuzzle();
						SetupScoreBoardPrevious();
					}

					GameState = PreviousGameState;
					PreviousGameState = GameStates.Summary;
				}
			}
			else if (GameState == GameStates.Settings)
			{
				_settingsManager.Update(gameTime, CurrentInputState);

				if (InputManager.IsGamePadButtonPressed(Buttons.Back) || InputManager.IsKeyPressed(Keys.Escape))
				{
					GameState = GameStates.Menu;
					PreviousGameState = GameStates.Summary;
				}

				if (InputManager.IsGamePadButtonPressed(Buttons.RightShoulder) || InputManager.IsKeyPressed(Keys.F7))
				{	
					if (SettingsData.settings.fullScreen != DefaultSettingsData.settings.fullScreen)
					{
						ToggleFullscreen();
					}
					SettingsData.settings.crtShader = DefaultSettingsData.settings.crtShader;
					SettingsData.settings.music = DefaultSettingsData.settings.music;
					SettingsData.settings.fullScreen = DefaultSettingsData.settings.fullScreen;

					_settingsManager.SetSettingsData(SettingsData);

					SaveGame();
				}

				if (InputManager.IsGamePadButtonPressed(Buttons.Y) || InputManager.IsKeyPressed(Keys.F8))
				{
					SettingsData.settings.music = !SettingsData.settings.music;

					_settingsManager.SetSettingsData(SettingsData);

					SaveGame();
				}

				if (InputManager.IsGamePadButtonPressed(Buttons.LeftShoulder) || InputManager.IsKeyPressed(Keys.F11))
				{
					SettingsData.settings.fullScreen = !SettingsData.settings.fullScreen;

					_settingsManager.SetSettingsData(SettingsData);

					SaveGame();
					ToggleFullscreen();
				}

                if (InputManager.IsGamePadButtonPressed(Buttons.X) || InputManager.IsKeyPressed(Keys.F12))
                {
                    SettingsData.settings.crtShader = !SettingsData.settings.crtShader;

					_settingsManager.SetSettingsData(SettingsData);

					SaveGame();
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
			_spriteBatch.End();

			if (SettingsData.settings.crtShader)
			{
				_spriteBatch.Begin(samplerState: SamplerState.PointClamp, effect: _effect);
				_spriteBatch.Draw(_backgroundTexture, new Rectangle(80, 62, 1123, 630), new Rectangle(80, 62, 1123, 630), Color.White);
				_spriteBatch.End();
			}

			_spriteBatch.Begin(samplerState: SamplerState.PointClamp);				

			if (GameState == GameStates.Menu)
			{
				_mainMenu.Draw(_spriteBatch, gameTime, _scale);
				_buttonManager.Draw(_spriteBatch, gameTime, _scale);
			}
			else if (GameState == GameStates.FreePlay || GameState == GameStates.SinglePuzzleTimed ||
				GameState == GameStates.TimeTrial)
			{				
				_matrix.Draw(_spriteBatch, gameTime, _scale);
				_terminalBuffer.Draw(_spriteBatch, gameTime, _scale);

				_spriteBatch.Draw(_spriteSheet, new Vector2(580, 215), new Rectangle(5, 115, 500, 250),
									Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				_spriteBatch.DrawString(_armadaFont, "Target Keys:", new Vector2(590, 225), Color.White);


				_targetList1.Draw(_spriteBatch, gameTime, _scale);
				_targetList2.Draw(_spriteBatch, gameTime, _scale);
				_targetList3.Draw(_spriteBatch, gameTime, _scale);

				_scoreBoard.Draw(_spriteBatch, gameTime, _scale);
				if (GameState == GameStates.SinglePuzzleTimed)
				{
					_spriteBatch.DrawString(_armadaFont, "Time Left: " + _remainingPuzzleTime.ToString("0.00"), new Vector2(600, 165), Color.White);
					_spriteBatch.DrawString(_armadaFont, "Puzzles Left: " + _remainingPuzzles.ToString() + "/" + _puzzleCount.ToString(), new Vector2(800, 165), Color.White);
				}
				if (GameState == GameStates.TimeTrial)
				{
					_spriteBatch.DrawString(_armadaFont, "Time Left: " + _remainingTime.ToString("0.00"), new Vector2(600, 165), Color.White);
					_spriteBatch.DrawString(_armadaFont, "Puzzles: " + _completedPuzzles.ToString(), new Vector2(830, 165), Color.White);
				}

				_buttonManager.Draw(_spriteBatch, gameTime, _scale);
			}
			else if (GameState == GameStates.Summary)
			{
				_summary.Draw(_spriteBatch, gameTime, _scale);
				_buttonManager.Draw(_spriteBatch, gameTime, _scale);
			}
			else if (GameState == GameStates.Settings)
			{
				_settingsManager.Draw(_spriteBatch, gameTime, _scale);
				_buttonManager.Draw(_spriteBatch, gameTime, _scale);
			}

			if (InputManager.IsGamePadConnected())
			{
				_inputStateIndicator.Draw(_spriteBatch, gameTime, _scale);
				// _spriteBatch.DrawString(_armadaFont, $" GamePadDisplayName: {InputManager.GetGamePadDisplayName()}", new Vector2(10, 10), Color.White);
			}

			_spriteBatch.DrawString(_armadaFont, "Scale: " + _scale.ToString(), new Vector2(10, 10), Color.White);
			//_spriteBatch.DrawString(_armadaFont, $" RenderDest.Width: {_renderDestination.Width} RenderDest.Height: {_renderDestination.Height}", new Vector2(10, 10), Color.White);
			//_spriteBatch.DrawString(_armadaFont, $" PreferredB.Width: {_graphics.PreferredBackBufferWidth} PreferredB.Height: {_graphics.PreferredBackBufferHeight}", new Vector2(10, 30), Color.White);

			_spriteBatch.End();

			GraphicsDevice.SetRenderTarget(null);

			_spriteBatch.Begin(samplerState: SamplerState.PointClamp);
			_spriteBatch.Draw(_renderTarget, _renderDestination, Color.White);
			_spriteBatch.End();

			base.Draw(gameTime);
		}

		private void SetupNewPuzzle()
		{
			_terminalBuffer = new TerminalBuffer(_armadaFont, _spriteSheet);

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
			_matrix = new PuzzleMatrix(_armadaFont, _spriteSheet, possibleValue, CurrentInputState, _flickingASwitch, _buttonPress, _uiWrong);

			_matrix.MatrixSelectionEvent += HandleSelectedMatrixEvent;

			// Create the target CipherLists using the possibleValues
			_targetList1 = new CipherList(_armadaFont, possibleValue, 3, 300, 1);
			_targetList2 = new CipherList(_armadaFont, possibleValue, 4, 450, 2);
			_targetList3 = new CipherList(_armadaFont, possibleValue, 5, 700, 3);
		}

		private void SetupScoreBoard()
		{
			_scoreBoard = new ScoreBoard(_armadaFont, _spriteSheet);

			if (GameState == GameStates.FreePlay)
			{
				_scoreBoard.HighScore = SettingsData.highScores.freePlay;
			}
			else if (GameState == GameStates.SinglePuzzleTimed)
			{
				_scoreBoard.HighScore = SettingsData.highScores.bestOf10Timed;
			}
			else if (GameState == GameStates.TimeTrial)
			{
				_scoreBoard.HighScore = SettingsData.highScores.timeTrial;
			}
		}

		private void SetupScoreBoardPrevious()
		{
			_scoreBoard = new ScoreBoard(_armadaFont, _spriteSheet);

			if (PreviousGameState == GameStates.FreePlay)
			{
				_scoreBoard.HighScore = SettingsData.highScores.freePlay;
			}
			else if (PreviousGameState == GameStates.SinglePuzzleTimed)
			{
				_scoreBoard.HighScore = SettingsData.highScores.bestOf10Timed;
			}
			else if (PreviousGameState == GameStates.TimeTrial)
			{
				_scoreBoard.HighScore = SettingsData.highScores.timeTrial;
			}
		}

		private void SetupSummary()
		{
			_summary = new Summary(_spriteSheet, _armadaFont);
			_summary.Score = _scoreBoard.Score;

			if (GameState == GameStates.FreePlay)
			{
				_summary.HighScore = SettingsData.highScores.freePlay;
				_summary.HighScoreDate = SettingsData.highScores.freePlayRecordDate;
			}
			else if (GameState == GameStates.SinglePuzzleTimed)
			{
				_summary.HighScore = SettingsData.highScores.bestOf10Timed;
				_summary.HighScoreDate = SettingsData.highScores.bestOf10TimedRecordDate;
			}
			else if (GameState == GameStates.TimeTrial)
			{
				_summary.HighScore = SettingsData.highScores.timeTrial;
				_summary.HighScoreDate = SettingsData.highScores.timeTrialRecordDate;
			}
		}

		private void OnMenuButtonSelection(GameStates newGameState)
		{
			if (newGameState == GameStates.SinglePuzzleTimed || newGameState == GameStates.FreePlay || newGameState == GameStates.TimeTrial)
			{
				SetupNewPuzzle();
				PreviousGameState = GameState;
				GameState = newGameState;
				SetupScoreBoard();
			}

			if (newGameState == GameStates.Settings)
			{
				_settingsManager.SetSettingsData(SettingsData);
				PreviousGameState = GameState;
				GameState = newGameState;				
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
					// _drop.Play();
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
						_positiveBlip.Play();
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
						_positiveBlip.Play();
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
						_positiveBlip.Play();
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
			if (GameState == GameStates.FreePlay && _scoreBoard.Score > SettingsData.highScores.freePlay)
			{
				SettingsData.highScores.freePlay = _scoreBoard.Score;
				SettingsData.highScores.freePlayRecordDate = DateTime.Now.ToString("yyyy-MM-dd");

				SaveGame();
			}
			else if (GameState == GameStates.SinglePuzzleTimed && _scoreBoard.Score > SettingsData.highScores.bestOf10Timed)
			{
				SettingsData.highScores.bestOf10Timed = _scoreBoard.Score;
				SettingsData.highScores.bestOf10TimedRecordDate = DateTime.Now.ToString("yyyy-MM-dd");

				SaveGame();
			}
			else if (GameState == GameStates.TimeTrial && _scoreBoard.Score > SettingsData.highScores.timeTrial)
			{
				SettingsData.highScores.timeTrial = _scoreBoard.Score;
				SettingsData.highScores.bestOf10TimedRecordDate = DateTime.Now.ToString("yyyy-MM-dd");

				SaveGame();
			}

		}

		public void SaveGame()
		{
			try
			{
				string defaultJsonData = JsonSerializer.Serialize(SettingsData, SourceGenerationContext.Default.SettingsData);

				File.WriteAllText(SETTINGS_FILE_NAME, defaultJsonData);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("An error occurred while saving the game: " + ex.Message);
			}
		}
		
		private void LoadSettingsFile()
		{
			try
			{
				if (File.Exists(SETTINGS_FILE_NAME))
				{
					string jsonData = File.ReadAllText(SETTINGS_FILE_NAME);

					SettingsData = JsonSerializer.Deserialize<SettingsData>(jsonData, SourceGenerationContext.Default.SettingsData);
				}
				else
				{
					SettingsData = CreateDefaultSettings();

					// Serialize the default object to JSON
					string defaultJsonData = JsonSerializer.Serialize(SettingsData, SourceGenerationContext.Default.SettingsData);

					// Write JSON to file
					File.WriteAllText(SETTINGS_FILE_NAME, defaultJsonData);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("An error occurred while loading the settings file: " + ex.Message);
			}
		}

		private SettingsData CreateDefaultSettings()
		{
			var settingsData = new SettingsData
			{
				highScores = new Highscores
				{
					freePlay = 0,
					freePlayRecordDate = DateTime.Now.ToString("yyyy-MM-dd"),
					bestOf10Timed = 0,
					bestOf10TimedRecordDate = DateTime.Now.ToString("yyyy-MM-dd"),
					timeTrial = 0,
					timeTrialRecordDate = DateTime.Now.ToString("yyyy-MM-dd")
				},
				settings = new Settings
				{
					fullScreen = false,
					crtShader = true,
					music = true
				}
			};

			return settingsData;
		}

		//private void ResetSaveState()
		//{
		//	_scoreBoard.HighScore = 0;
		//	//CurrentSaveState.FreePlayHighScore = 0;
		//	//CurrentSaveState.FreePlayHighScoreDate = default(DateTime);

		//	SaveGame();
		//}

		private void ToggleInputState()
		{
			if (CurrentInputState == InputStates.GamePad)
			{
				CurrentInputState = InputStates.MouseKeyboard;
			}
			else if (CurrentInputState == InputStates.MouseKeyboard)
			{
				if (InputManager.IsGamePadConnected())
					CurrentInputState = InputStates.GamePad;
				else
					CurrentInputState = InputStates.MouseKeyboard;
			}
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

			//_effect.Parameters["textureSize"]?.SetValue(new Vector2(size.X - 250, size.Y - 250));
			//_effect.Parameters["outputSize"]?.SetValue(new Vector2(size.X - 250, size.Y - 250));
			//_effect.Parameters["videoSize"]?.SetValue(new Vector2(size.X - 250, size.Y - 250));

			var texSize = new Vector2(_renderDestination.Width, _renderDestination.Height);
			_effect.Parameters["textureSize"]?.SetValue(texSize);
			_effect.Parameters["videoSize"]?.SetValue(texSize);
			var outSize = new Vector2(_nativeWidth, _nativeHeight);
			_effect.Parameters["outputSize"]?.SetValue(outSize);
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
			// _graphics.HardwareModeSwitch = !_isBorderless;
			_graphics.HardwareModeSwitch = false;

			_graphics.IsFullScreen = true;
			_graphics.ApplyChanges();
		}
		private void UnsetFullscreen()
		{
			// Reset to default resolution, but we do have the previous Width and Height just tweaky if somebody resized
			_graphics.PreferredBackBufferWidth = _nativeWidth;
			_graphics.PreferredBackBufferHeight = _nativeHeight;			

			_graphics.HardwareModeSwitch = false;
			_graphics.IsFullScreen = false;
			_graphics.ApplyChanges();
		}
	}
}
