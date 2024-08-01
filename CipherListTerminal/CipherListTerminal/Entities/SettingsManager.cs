using CipherListTerminal.Core;
using CipherListTerminal.Data;
using CipherListTerminal.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CipherListTerminal.Entities
{
	internal class SettingsManager : IGameEntity
	{
		private Texture2D _settingsUI;
		private SpriteFont _armadaFont;

		private int _settingsUIPositionX = 260;
		private int _settingsUIPositionY = 85;

		private int _highScoreUIPositionX = 650;
		private int _highScoreUIPositionY = 85;

		private SettingsData _settingsData;

		public SettingsManager(Texture2D summaryUI, SpriteFont armadaFont)
		{
			_settingsUI = summaryUI;
			_armadaFont = armadaFont;
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
		{
			// Settings			
			spriteBatch.Draw(_settingsUI, new Vector2(_settingsUIPositionX, _settingsUIPositionY), null, Color.White,
			0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, "Settings", new Vector2(270, 95), Color.White,
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, "CRT Shader", new Vector2(270, 155), Color.White,
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			if (_settingsData.settings.crtShader)
			{
				spriteBatch.DrawString(_armadaFont, "[ Enabled  ]", new Vector2(470, 155), Color.Green,
					0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.DrawString(_armadaFont, "[ Disabled ]", new Vector2(470, 155), Color.Red,
					0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}

			spriteBatch.DrawString(_armadaFont, "Music", new Vector2(270, 185), Color.White,
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			if (_settingsData.settings.music)
				{
				spriteBatch.DrawString(_armadaFont, "[ Enabled  ]", new Vector2(470, 185), Color.Green,
					0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.DrawString(_armadaFont, "[ Disabled ]", new Vector2(470, 185), Color.Red,
					0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}

			spriteBatch.DrawString(_armadaFont, "FullScreen", new Vector2(270, 215), Color.White,
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);


			spriteBatch.DrawString(_armadaFont, "(windowed mode recommended)", new Vector2(270, 235), Color.White,
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			if (_settingsData.settings.fullScreen)
				{
				spriteBatch.DrawString(_armadaFont, "[ Enabled  ]", new Vector2(470, 215), Color.Green,
					0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			else
			{
				spriteBatch.DrawString(_armadaFont, "[ Disabled ]", new Vector2(470, 215), Color.Red,
					0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}

			// High Score
			spriteBatch.Draw(_settingsUI, new Vector2(_highScoreUIPositionX, _highScoreUIPositionY), null, Color.White,
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, "High Scores", new Vector2(660, 95), Color.White,
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, "Free Play", new Vector2(660, 155), Color.White,
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, _settingsData.highScores.freePlay.ToString(), new Vector2(850, 155), Color.White,
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, _settingsData.highScores.freePlayRecordDate.ToString(), new Vector2(850, 185), Color.White,
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, "Best of 10", new Vector2(660, 215), Color.White,
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, _settingsData.highScores.bestOf10Timed.ToString(), new Vector2(850, 215), Color.White,
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, _settingsData.highScores.bestOf10TimedRecordDate.ToString(), new Vector2(850, 245), Color.White,
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, "TimeTrial", new Vector2(660, 275), Color.White,
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, _settingsData.highScores.timeTrial.ToString(), new Vector2(850, 275), Color.White,
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			spriteBatch.DrawString(_armadaFont, _settingsData.highScores.timeTrialRecordDate.ToString(), new Vector2(850, 305), Color.White,
				0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}

		public void Update(GameTime gameTime, InputStates inputState)
		{
			if (InputManager.IsKeyPressed(Keys.F5))
			{

			}
		}

		public void SetSettingsData(SettingsData settingsData)
		{
			_settingsData = settingsData;
		}
	}
}
