using BowlingBall.Contract;
using BowlingBall.Models;
using System;
using System.Linq;

namespace BowlingBall
{
	public class Game : IGame
	{
		private readonly IScoreBoardManager _boardManager;
		public Game(IScoreBoardManager boardManager)
		{
			_boardManager = boardManager;
		}

		/// <summary>
		/// Add number of pins knocked down in each roll, prepare bowling frame for each
		/// </summary>
		/// <param name="pins"></param>
		/// <param name="frameIndex"></param>
		public void Roll(string pins, int frameIndex)
		{
			var bowlingFrame = new Frame();
			var throws = pins.Split(',');

			if (throws.ElementAtOrDefault(0) != null)
				bowlingFrame.AddThrow(Convert.ToInt16(throws[0]), frameIndex);

			if (throws.ElementAtOrDefault(1) != null)
				bowlingFrame.AddThrow(Convert.ToInt16(throws[1]), frameIndex);

			if (throws.ElementAtOrDefault(2) != null)
				bowlingFrame.AddThrow(Convert.ToInt16(throws[2]), frameIndex);

			_boardManager.AddBowlingFrame(bowlingFrame, frameIndex);
		}

		/// <summary>
		/// Get score
		/// </summary>
		/// <returns></returns>
		public int GetScore()
		{
			_boardManager.CalculateScore();

			_boardManager.PrintScore();

			return _boardManager.GetTotalScore();
		}
	}
}
