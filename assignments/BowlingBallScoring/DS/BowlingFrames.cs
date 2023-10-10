using BowlingBall.Contract;

namespace BowlingBall.DS
{
	/// <summary>
	/// Create frame for each turn, adds previous and next frame for reference
	/// </summary>
	public class BowlingFrames<T> : IBowlingFrames<T>
	{
		private Node<T> head;
		private Node<T> first;

		public BowlingFrames()
		{
			head = null;
			first = null;
		}

		/// <summary>
		/// Link next frame to traverse
		/// </summary>
		/// <param name="frame"></param>
		public void AddLast(T frame)
		{
			var node = new Node<T>(frame);
			if (head == null && first == null)
			{
				head = node;
				first = node;
			}
			else
			{
				head.next = node;
				node.prev = head;
				head = node;
			}
		}

		/// <summary>
		/// Get current linked list size
		/// </summary>
		/// <returns></returns>
		public int GetSize()
		{
			if (head == null)
				return 0;

			var currentNode = first;
			var size = 0;
			do
			{
				size++;
				currentNode = currentNode.next;
			} while (currentNode != null);
			return size;
		}

		/// <summary>
		/// Get node value by index
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public T GetValue(int index)
		{
			if (head == null)
				return default;

			var currentNode = first;
			T value = default;
			var i = 0;
			do
			{
				if (i == index)
				{
					value = currentNode.data;
					break;
				}
				else
				{
					i++;
					currentNode = currentNode.next;
				}
			} while (currentNode != null);
			return value;
		}
	}
}
