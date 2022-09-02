using BowlingBallScoring.Business;
using BowlingBallScoring.Contract;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BowlingBallScoring.Tests
{
	public class Tests
	{
		private IGame _game;

		[SetUp]
		public void Setup()
		{
			var serviceProvider = new ServiceCollection()
				.AddSingleton<IScoreBoardManager, ScoreBoardManager>()
				.AddSingleton<IGame, Game>()
				.BuildServiceProvider();
			_game = serviceProvider.GetService<IGame>();
		}

		[Test]
		[TestCase(new string[] { "10", "9,1", "5,5", "7,2", "10", "10", "10", "9,0", "8,2", "9,1,10" }, 187)]
		[TestCase(new string[] { "1,4", "4,5", "6,4", "5,5", "10", "0,1", "7,3", "6,4", "10", "2,8,6" }, 133)]
		[TestCase(new string[] { "6,1", "9,0", "8,2", "5,5", "8,0", "6,2", "9,1", "7,2", "8,2", "9,1,7" }, 127)]
		[TestCase(new string[] { "5,5", "8,2", "9,1", "7,3", "8,2", "6,4", "9,1", "7,3", "6,4", "4,5" }, 163)]
		[TestCase(new string[] { "10", "10", "10", "10", "10", "10", "10", "10", "10", "10,10,10" }, 300)]
		public void Check_For_Valid_Score(string[] rolls, int score)
		{
			// Arrange
			_game.AddThrowsBowlingFrame(rolls);

			// Act
			var finalScore = _game.CalculateScore();

			// Assert
			Assert.AreEqual(finalScore, score);
		}
	}
}