using BowlingBallScoring.Business;

namespace BowlingBallScoring.Contract
{
	public interface IScoreBoardManager
	{
		public BowlingFrame[] BowlingFrames { get; }
		void AddBowlingFrame(BowlingFrame bowlingFrame, int index);
		void CalculateScore();
		void CalculateScoreForBowlingFrame(BowlingFrame bowlingFrame, int index);
	}
}
