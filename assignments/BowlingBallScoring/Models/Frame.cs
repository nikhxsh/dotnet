namespace BowlingBall.Models
{
	/// <summary>
	/// Bowling frame to add throws and sum-up score
	/// </summary>
	public class Frame
	{
		public int Throw1 { get; private set; } = -1;
		public int Throw2 { get; private set; } = -1;
		public int Throw3 { get; private set; } = -1;
		public int Score { get; set; }

		/// <summary>
		/// Adds number of pins knowked down against each throw
		/// </summary>
		/// <param name="pinsKnowkedDown"></param>
		public void AddThrow(int pinsKnowkedDown, int frameIndex)
		{
			if (!AreRollsCompleted(frameIndex))
			{
				if (Throw1 == -1)
					Throw1 = pinsKnowkedDown;
				else if (Throw2 == -1)
					Throw2 = pinsKnowkedDown;
				else
					Throw3 = pinsKnowkedDown;
			}
		}

		/// <summary>
		/// Check if all throws are completed against each roll
		/// </summary>
		/// <returns></returns>
		private bool AreRollsCompleted(int frameIndex)
		{
			if (frameIndex < 9)
			{
				if (Throw1 == 10)
					return true;
				else
					return Throw2 != -1;
			}
			else
			{
				if (Throw1 == 10)
					return Throw3 != -1;
				else if (Throw1 + Throw2 == 10)
					return Throw3 != -1;
				else
					return Throw2 != -1;
			}
		}

		/// <summary>
		/// Check if its strike
		/// </summary>
		/// <returns></returns>
		public bool IsStrike()
		{
			return Throw1 == 10;
		}

		/// <summary>
		/// Check if its spare
		/// </summary>
		/// <returns></returns>
		public bool IsSpare()
		{
			return Throw1 + Throw2 == 10;
		}

		/// <summary>
		/// Calaculate basic score with bonus points
		/// </summary>
		/// <returns></returns>
		public int CalculateBasicScore()
		{
			var basicScore = 0;
			if (Throw1 != -1)
				basicScore += Throw1;
			if (Throw2 != -1)
				basicScore += Throw2;
			if (Throw3 != -1)
				basicScore += Throw3;
			return basicScore;
		}
	}
}
