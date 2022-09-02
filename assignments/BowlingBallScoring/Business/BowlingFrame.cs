namespace BowlingBallScoring.Business
{
	/// <summary>
	/// Create frame for each turn, adds previous and next frame for reference
	/// </summary>
	public class BowlingFrame
	{
		public int frameNumber;
		public int throw1 { get; private set; } = -1;
		public int throw2 { get; private set; } = -1;
		public int throw3 { get; private set; } = -1;
		public int score { get; set; }

		public BowlingFrame previousFrame = null;
		public BowlingFrame nextFrame = null;

		public BowlingFrame(int frameNumber)
		{
			this.frameNumber = frameNumber;
		}

		/// <summary>
		/// Link previous frame to traverse
		/// </summary>
		/// <param name="frame"></param>
		public void AddPreviousFrame(BowlingFrame frame)
		{
			this.previousFrame = frame;
		}

		/// <summary>
		/// Link next frame to traverse
		/// </summary>
		/// <param name="frame"></param>
		public void AddNextFrame(BowlingFrame frame)
		{
			this.nextFrame = frame;
		}

		/// <summary>
		/// Adds number of pins knowked down against each throw
		/// </summary>
		/// <param name="pinsKnowkedDown"></param>
		public void AddThrow(int pinsKnowkedDown)
		{
			if (!AreRollsCompleted())
			{
				if (throw1 == -1)
					throw1 = pinsKnowkedDown;
				else if (throw2 == -1)
					throw2 = pinsKnowkedDown;
				else
					throw3 = pinsKnowkedDown;
			}
		}

		/// <summary>
		/// Check if all throws are completed against each roll
		/// </summary>
		/// <returns></returns>
		private bool AreRollsCompleted()
		{
			if (frameNumber < 9)
			{
				if (throw1 == 10)
					return true;
				else
					return throw2 != -1;
			}
			else
			{
				if (throw1 == 10)
					return throw3 != -1;
				else if (throw1 + throw2 == 10)
					return throw3 != -1;
				else
					return throw2 != -1;
			}
		}

		/// <summary>
		/// Check if its strike
		/// </summary>
		/// <returns></returns>
		public bool IsStrike()
		{
			return throw1 == 10;
		}

		/// <summary>
		/// Check if its spare
		/// </summary>
		/// <returns></returns>
		public bool IsSpare()
		{
			return throw1 + throw2 == 10;
		}

		/// <summary>
		/// Calaculate basic score with bonus points
		/// </summary>
		/// <returns></returns>
		public int CalculateBasicScore()
		{
			var basicScore = 0;
			if (throw1 != -1)
				basicScore += throw1;
			if (throw2 != -1)
				basicScore += throw2;
			if (throw3 != -1)
				basicScore += throw3;
			return basicScore;
		}
	}
}
