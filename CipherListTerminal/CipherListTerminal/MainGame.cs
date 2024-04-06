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

		string[] possibleValues = { "1C", "55", "BD", "FF", "E9" };

		private SpriteFont _font;
		private TerminalBuffer _terminalBuffer;
		private PuzzleMatrix _matrix;
		private CipherList _targetList1;
		private CipherList _targetList2;
		private CipherList _targetList3;


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
			// TODO: Add your initialization logic here

			base.Initialize();
			CalculateRenderDestination();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_renderTarget = new RenderTarget2D(GraphicsDevice, _nativeWidth, _nativeHeight);

			_font = Content.Load<SpriteFont>("TestFont");

			_terminalBuffer = new TerminalBuffer(_font);

			// Create the starting Matrix
			_matrix = new PuzzleMatrix(_font, possibleValues);

			// Create the target CipherLists using the possibleValues
			_targetList1 = new CipherList(_font, possibleValues, 3, 300, 1);
			_targetList2 = new CipherList(_font, possibleValues, 4, 450, 2);
			_targetList3 = new CipherList(_font, possibleValues, 5, 700, 3);
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// TODO: Add your update logic here
			InputManager.Update(_renderDestination);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			// Add RenterTarget2D to allow screen resizing
			GraphicsDevice.SetRenderTarget(_renderTarget);

			GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin(samplerState: SamplerState.PointClamp);

			_matrix.Draw(_spriteBatch, gameTime, _renderDestination, _scale);
			_terminalBuffer.Draw(_spriteBatch, gameTime);
			_targetList1.Draw(_spriteBatch, gameTime);
			_targetList2.Draw(_spriteBatch, gameTime);
			_targetList3.Draw(_spriteBatch, gameTime);

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
	}
}
