using BowlingBall.Contract;
using BowlingBall.Models;
using System;

namespace BowlingBall
{
	/// <summary>
	/// Manage frames and score for bowling game
	/// </summary>
	public class ScoreBoardManager : IScoreBoardManager
	{
		private readonly IBowlingFrames<Frame> bowlingFrames;

		public ScoreBoardManager(IBowlingFrames<Frame> bowlingFrames)
		{
			this.bowlingFrames = bowlingFrames;
		}

		/// <summary>
		/// Add bowling frame to ScoreBoard
		/// </summary>
		/// <param name="bowlingFrame"></param>
		/// <param name="index"></param>
		public void AddBowlingFrame(Frame frame, int index)
		{
			bowlingFrames.AddLast(frame);
		}

		/// <summary>
		/// Get total score against each bowling frame
		/// </summary>
		public void CalculateScore()
		{
			for (int i = 0; i < bowlingFrames.GetSize(); i++)
			{
				CalculateScoreForBowlingFrame(bowlingFrames.GetValue(i), i);
			}
		}

		/// <summary>
		/// Calculate score for given bowling frame
		/// </summary>
		/// <param name="bowlingFrame"></param>
		/// <param name="index"></param>
		public void CalculateScoreForBowlingFrame(Frame frame, int index)
		{
			var currentScore = frame.CalculateBasicScore();

			var nextFrame = bowlingFrames.GetValue(index + 1);
			var prevFrame = bowlingFrames.GetValue(index - 1);

			// Strike
			if (frame.IsStrike() && nextFrame != null)
			{
				currentScore += GetStrikePoint(nextFrame, index + 1);
			}
			// Spare
			else if (frame.IsSpare() && nextFrame != null)
			{
				currentScore += GetSparePoint(nextFrame);
			}

			// Get previous bowling frame score
			if (prevFrame != null)
			{
				currentScore += prevFrame.Score;
			}

			// Update score against bowling frame
			frame.Score = currentScore;
		}

		/// <summary>
		/// Print score for each frame
		/// </summary>
		public void PrintScore()
		{
			for (int j = 0; j < bowlingFrames.GetSize(); j++)
			{
				Console.Write($"{bowlingFrames.GetValue(j).Score} ");
			}
		}

		/// <summary>
		/// Get final score
		/// </summary>
		/// <returns></returns>
		public int GetTotalScore()
		{
			return bowlingFrames.GetValue(bowlingFrames.GetSize() - 1).Score;
		}

		/// <summary>
		/// Caculate and return strike points by checking subsequent bowling frame
		/// </summary>
		/// <param name="bowlingFrame"></param>
		/// <returns></returns>
		private int GetStrikePoint(Frame frame, int index)
		{
			var nextFrame = bowlingFrames.GetValue(index + 1);
			// Consecutive strikes
			if (frame.IsStrike() && nextFrame != null)
			{
				return 10 + nextFrame.Throw1;
			}
			else
			{
				return frame.Throw1 + frame.Throw2;
			}
		}

		/// <summary>
		/// Returns first throw of subsequent bowling frame  
		/// </summary>
		/// <param name="bowlingFrame"></param>
		/// <returns></returns>
		private int GetSparePoint(Frame frame)
		{
			return frame.Throw1;
		}
	}
}
