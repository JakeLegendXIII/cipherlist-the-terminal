using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CipherListTerminal.Core;

namespace CipherListTerminal.Entities
{
	internal interface IGameEntity
	{
		void Update(GameTime gameTime, InputStates inputState);
		void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale);
	}
}
