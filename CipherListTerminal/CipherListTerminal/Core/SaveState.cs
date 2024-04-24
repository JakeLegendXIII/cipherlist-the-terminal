using System;

namespace CipherListTerminal.Core
{
	[Serializable]
	public class SaveState
	{
		public int FreePlayHighScore { get; set; }
		public DateTime FreePlayHighScoreDate { get; set; }
	}
}
