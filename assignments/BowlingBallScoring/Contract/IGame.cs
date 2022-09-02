using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingBallScoring.Contract
{
	internal interface IGame
	{
		void AddThrowsBowlingFrame(string[] rolls);
		int CalculateScore();
	}
}
