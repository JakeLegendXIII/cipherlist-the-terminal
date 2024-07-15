namespace CipherListTerminal.Data
{
	public class SettingsData
	{
		public Highscores highScores { get; set; }
		public Settings settings { get; set; }
	}

	public class Highscores
	{
		public int freePlay { get; set; }
		public string freePlayRecordDate { get; set; }
		public int bestOf10Timed { get; set; }
		public string bestOf10TimedRecordDate { get; set; }
		public int timeTrial { get; set; }
		public string timeTrialRecordDate { get; set; }
	}

	public class Settings
	{
		public string displayMode { get; set; }
		public bool crtShader { get; set; }
		public bool music { get; set; }
	}
}
