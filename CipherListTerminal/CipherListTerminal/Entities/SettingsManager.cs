using CipherListTerminal.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CipherListTerminal.Entities
{
	internal class SettingsManager : IGameEntity
	{
		private Texture2D _settingsUI;
		private SpriteFont _armadaFont;

		private int _settingsUIPositionX = 260;
		private int _settingsUIPositionY = 85;

        public SettingsManager(Texture2D summaryUI, SpriteFont armadaFont)
        {
			_settingsUI = summaryUI;
            _armadaFont = armadaFont;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			spriteBatch.Draw(_settingsUI, new Vector2(_settingsUIPositionX, _settingsUIPositionY), null, Color.White, 
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, "Settings", new Vector2(270, 95), Color.White,
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}

		public void Update(GameTime gameTime, InputStates inputState)
		{
			
		}
	}
}
