namespace BowlingBall.Contract
{
	public interface IGame
	{
		void Roll(string pins, int frame);
		int GetScore();
	}
}
