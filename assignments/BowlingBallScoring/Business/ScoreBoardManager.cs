using BowlingBallScoring.Contract;

namespace BowlingBallScoring.Business
{
	/// <summary>
	/// Manages scoreBoard for bowling game
	/// </summary>
	public class ScoreBoardManager : IScoreBoardManager
	{
		public BowlingFrame[] BowlingFrames { get; private set; }

		public ScoreBoardManager(int totalFrames = 10)
		{
			BowlingFrames = new BowlingFrame[totalFrames];
		}

		/// <summary>
		/// Add bowling frame to ScoreBoard
		/// </summary>
		/// <param name="bowlingFrame"></param>
		/// <param name="index"></param>
		public void AddBowlingFrame(BowlingFrame bowlingFrame, int index)
		{
			BowlingFrames[index] = bowlingFrame;
		}

		/// <summary>
		/// Get total score against each bowling frame
		/// </summary>
		public void CalculateScore()
		{
			for (int i = 0; i < BowlingFrames.Length; i++)
			{
				CalculateScoreForBowlingFrame(BowlingFrames[i], i);
			}
		}

		/// <summary>
		/// Calculate score for given bowling frame
		/// </summary>
		/// <param name="bowlingFrame"></param>
		/// <param name="index"></param>
		public void CalculateScoreForBowlingFrame(BowlingFrame bowlingFrame, int index)
		{
			var currentScore = bowlingFrame.CalculateBasicScore();
			// Strike
			if (bowlingFrame.IsStrike() && bowlingFrame.nextFrame != null)
			{
				currentScore += GetStrikePoint(bowlingFrame.nextFrame);
			}
			// Spare
			else if (bowlingFrame.IsSpare() && bowlingFrame.nextFrame != null)
			{
				currentScore += GetSparePoint(bowlingFrame.nextFrame);
			}

			// Get previous bowling frame score
			if (bowlingFrame.previousFrame != null)
			{
				currentScore += bowlingFrame.previousFrame.score;
			}

			// Update score against bowling frame
			bowlingFrame.score = currentScore;
		}

		/// <summary>
		/// Caculate and return strike points by checking subsequent bowling frame
		/// </summary>
		/// <param name="bowlingFrame"></param>
		/// <returns></returns>
		private int GetStrikePoint(BowlingFrame bowlingFrame)
		{
			// Consecutive strikes
			if (bowlingFrame.IsStrike() && bowlingFrame.nextFrame != null)
			{
				return 10 + bowlingFrame.nextFrame.throw1;
			}
			else
			{
				return bowlingFrame.throw1 + bowlingFrame.throw2;
			}
		}

		/// <summary>
		/// Returns first throw of subsequent bowling frame  
		/// </summary>
		/// <param name="bowlingFrame"></param>
		/// <returns></returns>
		private int GetSparePoint(BowlingFrame bowlingFrame)
		{
			return bowlingFrame.throw1;
		}
	}
}
