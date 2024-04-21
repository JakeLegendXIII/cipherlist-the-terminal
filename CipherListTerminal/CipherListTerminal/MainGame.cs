using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CipherListTerminal.Entities;
using System;
using CipherListTerminal.Input;

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

		string[] possibleValues = { "1C", "55", "BD", "FF", "E9", "1C", "55" };

		private SpriteFont _font;
		private Texture2D _backgroundTexture;
		private Texture2D _matrixUI;
		private Texture2D _bufferUI;

		private TerminalBuffer _terminalBuffer;
		private PuzzleMatrix _matrix;
		private CipherList _targetList1;
		private CipherList _targetList2;
		private CipherList _targetList3;
		private ScoreBoard _scoreBoard;

		private const float _completedDelay = 1f;
		private float _remainingDelay = _completedDelay;

		public MainGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			_graphics.PreferredBackBufferWidth = _nativeWidth;
			_graphics.PreferredBackBufferHeight = _nativeHeight;
			_graphics.ApplyChanges();


			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += OnClientSizeChanged;

			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			base.Initialize();
			CalculateRenderDestination();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_renderTarget = new RenderTarget2D(GraphicsDevice, _nativeWidth, _nativeHeight);

			_font = Content.Load<SpriteFont>("TestFont");
			_backgroundTexture = Content.Load<Texture2D>("Sprites/RoughBG2");
			_matrixUI = Content.Load<Texture2D>("Sprites/MatrixUI");
			_bufferUI = Content.Load<Texture2D>("Sprites/BufferUI");

			_scoreBoard = new ScoreBoard(_font);
			SetupNewPuzzle();			
		}

		private void SetupNewPuzzle()
		{
			_terminalBuffer = new TerminalBuffer(_font, _bufferUI);

			// Create the starting Matrix
			_matrix = new PuzzleMatrix(_font, _matrixUI, possibleValues);

			_matrix.MatrixSelectionEvent += HandleSelectedMatrixEvent;

			// Create the target CipherLists using the possibleValues
			_targetList1 = new CipherList(_font, possibleValues, 3, 300, 1);
			_targetList2 = new CipherList(_font, possibleValues, 4, 450, 2);
			_targetList3 = new CipherList(_font, possibleValues, 5, 700, 3);			
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
			
			InputManager.Update(_renderDestination, _scale);
			_matrix.Update(gameTime);

			if (InputManager.IsKeyPressed(Keys.F5))
			{
				SetupNewPuzzle();
			}

			if (InputManager.IsKeyPressed(Keys.F11))
			{
				_graphics.ToggleFullScreen();
				// CalculateRenderDestination();
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

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			// Add RenterTarget2D to allow screen resizing
			GraphicsDevice.SetRenderTarget(_renderTarget);

			GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin(samplerState: SamplerState.PointClamp);

			_spriteBatch.Draw(_backgroundTexture, new Rectangle(0, 0, _nativeWidth, _nativeHeight), Color.White);

			// _spriteBatch.DrawString(_font, "Scale: " + _scale.ToString(), new Vector2(600, 100), Color.White);
			_matrix.Draw(_spriteBatch, gameTime, _scale);
			_terminalBuffer.Draw(_spriteBatch, gameTime, _scale);
			_targetList1.Draw(_spriteBatch, gameTime, _scale);
			_targetList2.Draw(_spriteBatch, gameTime, _scale);
			_targetList3.Draw(_spriteBatch, gameTime, _scale);

			_scoreBoard.Draw(_spriteBatch, gameTime, _scale);

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
	}
}
