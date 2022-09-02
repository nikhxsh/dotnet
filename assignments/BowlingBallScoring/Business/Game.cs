using BowlingBallScoring.Contract;
using System;
using System.Linq;

namespace BowlingBallScoring.Business
{
	/// <summary>
	/// Manage bowling game
	/// </summary>
	public class Game : IGame
	{
		private readonly IScoreBoardManager _boardManager;
		public Game(IScoreBoardManager boardManager)
		{
			_boardManager = boardManager;
		}

		/// <summary>
		/// Adds throws to bowling frame
		/// </summary>
		/// <param name="rolls"></param>
		public void AddThrowsBowlingFrame(string[] rolls)
		{
			for (int i = 0; i < rolls.Length; i++)
			{
				var bowlingFrame = new BowlingFrame(i);
				var throws = rolls[i].Split(',');

				if (throws.ElementAtOrDefault(0) != null)
					bowlingFrame.AddThrow(Convert.ToInt16(throws[0]));

				if (throws.ElementAtOrDefault(1) != null)
					bowlingFrame.AddThrow(Convert.ToInt16(throws[1]));

				if (throws.ElementAtOrDefault(2) != null)
					bowlingFrame.AddThrow(Convert.ToInt16(throws[2]));

				_boardManager.AddBowlingFrame(bowlingFrame, i);
			}

			LinkBowlingFrames();
		}

		/// <summary>
		/// Links previous and next frame like linked list
		/// </summary>
		private void LinkBowlingFrames()
		{
			for (int j = 0; j < _boardManager.BowlingFrames.Length; j++)
			{
				var frame = _boardManager.BowlingFrames[j];
				if (j > 0)
					frame.AddPreviousFrame(_boardManager.BowlingFrames[j - 1]);
				if (j < _boardManager.BowlingFrames.Length - 1)
					frame.AddNextFrame(_boardManager.BowlingFrames[j + 1]);
			}
		}

		/// <summary>
		/// Calculate score and print as output
		/// </summary>
		public int CalculateScore()
		{
			_boardManager.CalculateScore();

			for (int j = 0; j < _boardManager.BowlingFrames.Length; j++)
			{
				Console.Write($"{_boardManager.BowlingFrames[j].score} ");
			}
			Console.WriteLine();

			return _boardManager.BowlingFrames[_boardManager.BowlingFrames.Length - 1].score;
		}
	}
}
