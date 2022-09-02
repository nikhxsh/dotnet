using BowlingBallScoring.Business;
using BowlingBallScoring.Contract;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace BowlingBallScoring
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Bowling ball scoring");

			//setup our DI
			var serviceProvider = new ServiceCollection()
				.AddSingleton<IScoreBoardManager, ScoreBoardManager>()
				.AddSingleton<IGame, Game>()
				.BuildServiceProvider();

			//Dummy data
			var input = new string[] { "10", "9,1", "5,5", "7,2", "10", "10", "10", "9,0", "8,2", "9,1,10" };

			var bowlingGame = serviceProvider.GetService<IGame>();

			bowlingGame.AddThrowsBowlingFrame(input);

			var finalScore = bowlingGame.CalculateScore();

			Console.WriteLine($"Final Score : {finalScore}");
		}
	}
}
