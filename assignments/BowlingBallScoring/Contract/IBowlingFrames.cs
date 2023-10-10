namespace BowlingBall.Contract
{
	public interface IBowlingFrames<T>
	{
		void AddLast(T frame);
		int GetSize();
		T GetValue(int index);
	}
}
