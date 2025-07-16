using System;

namespace DataStructures
{
    /*
    A Binary Heap is a complete binary tree (every level is filled except possibly the last, and all nodes are as far left as possible) that satisfies a specific heap order property.
       Min-Heap
            - Parent ≤ Children
            - The smallest element is at the root
            - It’s a priority queue where you always get the minimum value first
            - E.g.
                    2
                   / \
                  3   4
                 / \
                5   8
        Max-Heap
            - Parent ≥ Children
            - The largest element is at the root
            - It’s a priority queue where you always get the maximum value first
            - E.g.        
                    9
                   / \
                  7   6
                 / \
                3   1
        Applications of Min-Heap and Max-Heap
            Use Case                                Heap Type
            Priority Queue (shortest job)            Min-Heap
            Median in streaming data            Both (Min + Max Heap)
            K smallest/largest elements             Min or Max
            Scheduling / Task Queues                 Min-Heap
            Heap Sort                                Max-Heap
            Dijkstra’s algorithm                     Min-Heap
    */
    class HeapExample
    {
        public HeapExample()
        {
            int[] inputArray;
            Console.WriteLine("-------- Tree Structures --------");
            Console.WriteLine(" 1. Min Heap");
            Console.WriteLine(" 2. Max Heap");
            Console.WriteLine(" 3. Example of Max Heap");
            Console.WriteLine(" 4. Exit");
            Console.WriteLine("---------------------------------");
            int choice;
            do
            {
                choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        var minHeap = new MinHeap();
                        inputArray = new int[] { 35, 33, 42, 10, 14, 19, 27, 44, 26, 31 };

                        Console.Write("Input Array: ");
                        for (int i = 0; i < inputArray.Length; i++)
                        {
                            Console.Write($" {inputArray[i]} ");
                            minHeap.Insert(inputArray[i]);
                        }

                        Console.Write($"Peek() {minHeap.Peek()}"); //10
                        Console.Write($"Max  {minHeap.RemoveMin()}\n"); //10
                        Console.Write($"Peek() {minHeap.Peek()}"); //14
                        break;

                    case 2:
                        var maxHeap = new Heap();

                        inputArray = new int[] { 35, 33, 42, 10, 14, 19, 27, 44, 26, 31 };

                        Console.Write("Input Array: ");
                        for (int i = 0; i < inputArray.Length; i++)
                        {
                            Console.Write($" {inputArray[i]} ");
                            maxHeap.Insert(inputArray[i]);
                        }

                        Console.Write($"Peek() {maxHeap.Peek()}"); //44
                        Console.Write($"Max  {maxHeap.RemoveMax()}\n"); //44
                        Console.Write($"Peek() {maxHeap.Peek()}"); //42
                        break;

                    case 3:
                        var ticketsHeap = new Heap();

                        //Given ‘N’ win­dows where each win­dow con­tains cer­tain num­ber of tick­ets at each win­dow.
                        //Price of a ticket is equal to num­ber of tick­ets remain­ing at that win­dow. Write an algo­rithm
                        //to sell ‘k’ tick­ets from these win­dows in such a man­ner so that it gen­er­ates the max­i­mum revenue.
                        var numberOfTickets = new int[] { 5, 1, 7, 10, 11, 9 };
                        var k=4;

                        Console.Write("Tickets: ");
                        for (int i = 0; i < numberOfTickets.Length; i++)
                        {
                            Console.Write($" {numberOfTickets[i]} ");
                            maxHeap.Insert(numberOfTickets[i]);
                        }

                        var maxSell = 0;
                        for (int i = 0; i < k; i++)
                        {
                            maxSell += maxHeap.RemoveMax();
                        }
                        
                        Console.Write($"Max Sell with {k} tickets: {maxSell}");
                        break;
                }
            } while (choice != 4);
        }

        public class MinHeap
        {
            private List<int> heap = new();
        
            public int Count => heap.Count;
        
            public void Insert(int val)
            {
                heap.Add(val);
                HeapifyUp(Count - 1);
            }
        
            public int Peek()
            {
                if (Count == 0) throw new InvalidOperationException("Heap is empty.");
                return heap[0];
            }
        
            public int RemoveMin()
            {
                if (Count == 0) throw new InvalidOperationException("Heap is empty.");
                int min = heap[0];
                heap[0] = heap[^1];
                heap.RemoveAt(Count - 1);
                HeapifyDown(0);
                return min;
            }
        
            private void HeapifyUp(int i)
            {
                while (i > 0)
                {
                    int parent = (i - 1) / 2;
                    if (heap[i] >= heap[parent]) break;
                    (heap[i], heap[parent]) = (heap[parent], heap[i]);
                    i = parent;
                }
            }
        
            private void HeapifyDown(int i)
            {
                while (true)
                {
                    int left = 2 * i + 1;
                    int right = 2 * i + 2;
                    int smallest = i;
        
                    if (left < Count && heap[left] < heap[smallest]) smallest = left;
                    if (right < Count && heap[right] < heap[smallest]) smallest = right;
        
                    if (smallest == i) break;
        
                    (heap[i], heap[smallest]) = (heap[smallest], heap[i]);
                    i = smallest;
                }
            }
        }

        public class MaxHeap
        {
            private List<int> heap = new();
        
            public int Count => heap.Count;
        
            public void Insert(int val)
            {
                heap.Add(val);
                HeapifyUp(Count - 1);
            }
        
            public int Peek()
            {
                if (Count == 0) throw new InvalidOperationException("Heap is empty.");
                return heap[0];
            }
        
            public int RemoveMax()
            {
                if (Count == 0) throw new InvalidOperationException("Heap is empty.");
                int max = heap[0];
                heap[0] = heap[^1];
                heap.RemoveAt(Count - 1);
                HeapifyDown(0);
                return max;
            }
        
            private void HeapifyUp(int i)
            {
                while (i > 0)
                {
                    int parent = (i - 1) / 2;
                    if (heap[i] <= heap[parent]) break;
                    (heap[i], heap[parent]) = (heap[parent], heap[i]);
                    i = parent;
                }
            }
        
            private void HeapifyDown(int i)
            {
                while (true)
                {
                    int left = 2 * i + 1;
                    int right = 2 * i + 2;
                    int largest = i;
        
                    if (left < Count && heap[left] > heap[largest]) largest = left;
                    if (right < Count && heap[right] > heap[largest]) largest = right;
        
                    if (largest == i) break;
        
                    (heap[i], heap[largest]) = (heap[largest], heap[i]);
                    i = largest;
                }
            }
        }
    }
}
