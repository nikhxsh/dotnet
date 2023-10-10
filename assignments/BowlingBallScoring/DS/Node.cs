namespace BowlingBall.DS
{
	class Node<T>
	{
		internal readonly T data;
		internal Node<T> next;
		internal Node<T> prev;
		public Node(T data)
		{
			this.data = data;
			next = default;
			prev = default;
		}
	}
}
