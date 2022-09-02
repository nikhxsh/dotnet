namespace BowlingBallScoring.Contract
{
	public interface IGame
	{
		void AddThrowsBowlingFrame(string[] rolls);
		int CalculateScore();
	}
}
