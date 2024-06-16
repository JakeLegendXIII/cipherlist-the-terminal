using CipherListTerminal.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CipherListTerminal.Entities
{
	internal class ButtonManager : IGameEntity
	{
		private InputStates _inputState;
		private GameStates _gameState;
		private List<Button> _buttons;

		private Texture2D _buttonUI;

		public ButtonManager()
		{
			// Pre-load the various buttons for each game mode need to figure out handling positions
			_buttons = [
				new Button(_buttonUI, "Back", "ESC", "Back"),
				new Button(_buttonUI, "Next Puzzle", "F5", "RT"),
				new Button(_buttonUI, "Switch Input", "F10", "LT"),
				new Button(_buttonUI, "Continue", "Enter", "A"),
				new Button(_buttonUI, "Full Screen", "F11", "Y"),
			];
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			
		}

		public void Update(GameTime gameTime, InputStates inputState) {}

		public void Update(GameTime gameTime, InputStates inputState, GameStates gameState)
		{
			_inputState = inputState;
			_gameState = gameState;
		}
	}
}
