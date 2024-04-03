using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CipherListTerminal.Entities;
using System;

namespace CipherListTerminal
{
	public class MainGame : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		private int _nativeWidth = 1280;
		private int _nativeHeight = 800;

		string[] possibleValues = { "1C", "55", "BD", "FF", "E9" };

		private SpriteFont _font;
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
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_font = Content.Load<SpriteFont>("TestFont");

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

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			// Add RenterTarget2D to allow screen resizing
			
			GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin(samplerState: SamplerState.PointClamp);

			_matrix.Draw(_spriteBatch, gameTime);
			_targetList1.Draw(_spriteBatch, gameTime);
			_targetList2.Draw(_spriteBatch, gameTime);
			_targetList3.Draw(_spriteBatch, gameTime);

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
