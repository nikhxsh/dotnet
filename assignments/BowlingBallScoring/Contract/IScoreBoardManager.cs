using BowlingBall.Models;

namespace BowlingBall.Contract
{
	public interface IScoreBoardManager
	{
		void AddBowlingFrame(Frame frame, int index);
		void CalculateScore();
		void CalculateScoreForBowlingFrame(Frame frame, int index);
		void PrintScore();
		int GetTotalScore();
	}
}
