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

		public ButtonManager()
		{
			// Pre-load the various buttons for each game mode need to figure out handling positions
			_buttons = new List<Button>();
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
