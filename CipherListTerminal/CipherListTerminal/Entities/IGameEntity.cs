using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CipherListTerminal.Entities
{
	internal interface IGameEntity
	{
		void Update(GameTime gameTime);
		void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale);
	}
}
