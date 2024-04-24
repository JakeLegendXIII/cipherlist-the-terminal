using System;

namespace CipherListTerminal.Core
{
	[Serializable]
	public class SaveState
	{
		public int HighScore { get; set; }
		public DateTime HighScoreDate { get; set; }
	}
}
