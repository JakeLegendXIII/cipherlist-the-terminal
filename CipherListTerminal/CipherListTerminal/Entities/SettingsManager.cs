using CipherListTerminal.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CipherListTerminal.Entities
{
	internal class SettingsManager : IGameEntity
	{
		private Texture2D _settingsUI;
		private SpriteFont _armadaFont;

		private int _summaryUIPositionX = 260;
		private int _summaryUIPositionY = 85;

        public SettingsManager(Texture2D summaryUI, SpriteFont armadaFont)
        {
			_settingsUI = summaryUI;
            _armadaFont = armadaFont;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			
		}

		public void Update(GameTime gameTime, InputStates inputState)
		{
			
		}
	}
}
