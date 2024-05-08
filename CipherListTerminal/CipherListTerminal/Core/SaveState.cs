using System;

namespace CipherListTerminal.Core
{
	public class SaveState
	{
		public int FreePlayHighScore { get; set; }
		public DateTime FreePlayHighScoreDate { get; set; }
		public int SinglePuzzleHighScore { get; set; }
		public DateTime SinglePuzzleHighScoreDate { get; set; }
		public int TimeTrialHighScore { get; set; }
		public DateTime TimeTrialHighScoreDate { get; set; }
	}
}
