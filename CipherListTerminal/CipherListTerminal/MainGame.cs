using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CipherListTerminal
{
	public class MainGame : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		string[,] matrix = new string[6, 6];
		string[] possibleValues = { "1C", "55", "BD", "FF", "E9" };

		private SpriteFont _font;
		private Random _random = new Random();

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

			// Initialize the matrix
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					int randomIndex = _random.Next(0, possibleValues.Length - 1);
					matrix[i, j] = possibleValues[randomIndex];
				}
			}
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

			// Draw the matrix
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					string text = matrix[i, j];
					Vector2 position = new Vector2(100 + j * 50, 100 + i * 50);
					_spriteBatch.DrawString(_font,text, position, Color.White);
				}
			}

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
