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
		string[] possibleValues = { "1C", "55", "BD", "FF", "E9" };

		private SpriteFont _font;
		private PuzzleMatrix _matrix;

		public MainGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
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
			GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin();

			_matrix.Draw(_spriteBatch, gameTime);

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
