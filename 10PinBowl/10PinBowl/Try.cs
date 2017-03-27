using System;
/*
Here’s a quick overview of the rules of 10-pin bowling.

1. Frames. A bowling game is made up of ten frames. A player rolls the ball down the lane; if he knocks down all the pins, he gets a strike, marked as an X.
If not, the knocked down pins are removed and the player gets one more chance to hit the pins. If he knocks down all the pins on the second roll of he frame, he gets a spare, marked as a slash.
2. Scoring. If the player doesn't bowl a strike or spare, her score for a frame is the number of pins she marks down; this is called an open frame. '
If she bowls a spare, the score for the frame is ten plus the number of pins knocked down on the next roll. For a strike, the score 10 plus the number of pins knocked down on the next two rolls.
3. Final frame. If a player bowls a strike in the tenth frame, he gets two additional roll; for a spare, he gets one additional roll. 
The highest possible score in bowling is 300, which occurs when a player bowls twelve strikes in a row.

E.g.

In: 1
Out: 0

In: 2
Out: 3

In: 10
Out: 3

In: 10
Out: 3

In: 10
Out: 33

In: 8
Out: 61

In: 1
Out: 89

*/
namespace _10PinBowl
{
	public class Try
	{
		static void Main(string[] args)
		{
			do
			{
				PlayTenPinBowling();
			}
			while (PlayTenPinBowling());
		}
		public static bool PlayTenPinBowling()
		{
			Console.Write("Please Enter the Roll values: \n");
			int frame = 0, frameMax = 9;
			int input1, input2;
			LinkedList Bowling = new LinkedList();
			for (frame = 0; frame <= frameMax; frame++)
			{
				if (frame == frameMax)
				{
					TakeLastInput(Bowling);
				}
				else
				{
					input1 = Convert.ToInt32(Console.ReadLine());
					if (input1 >= 0 && input1 < 10)
					{
						input2 = Convert.ToInt32(Console.ReadLine());
						if (input2 >= 0 && input2 < 10)
						{
							if (input1 + input2 > 10)
							{
								Console.WriteLine("Invalid INPUT!");
								break;
							}
							Bowling.AddLast(input1, input2);
						}
						else if (input2 == 10)
						{
							if (input1 + input2 > 10)
							{
								Console.WriteLine("Invalid INPUT!");
								break;
							}
							Bowling.AddLast(0, input2);
						}
						else
							Console.WriteLine("Invalid INPUT!");
					}
					else if (input1 == 10)
					{
						Bowling.AddLast(input1, 0);
					}
					else
						Console.WriteLine("Invalid INPUT!");
					Bowling.PrintAllNodes();
				}
			}
			Console.WriteLine("Press any key to stop...");
			Console.ReadKey();
			while (true) // Continue asking until a correct answer is given.
			{
				Console.Write("Do you want to play again [Y/N]?");
				string answer = Console.ReadLine().ToUpper();
				if (answer == "Y")
					return true;
				if (answer == "N")
					return false;
			}
		}

		public static void TakeLastInput(LinkedList Bowling)
		{
			int input1, input2;
			int finalScore = 0;
			input1 = Convert.ToInt32(Console.ReadLine());
			if (input1 >= 0 && input1 < 10)
			{
				input2 = Convert.ToInt32(Console.ReadLine());
				if (input2 >= 0 && input2 < 10)
				{
					if (input1 + input2 > 10)
					{
						Console.WriteLine("Invalid INPUT!");
					}
					Bowling.AddLast(input1, input2);
					Bowling.AddLast(0, 0);
					Bowling.AddLast(0, 0);
					finalScore = Bowling.GetLast().score;
					if (input1 + input2 == 10)
					{
						input1 = Convert.ToInt32(Console.ReadLine());
						Bowling.AddLast(input1, 0, true);
						finalScore += input1;
					}
				}
				else
					Console.WriteLine("Invalid INPUT!");
			}
			else if (input1 == 10)
			{
				Bowling.AddLast(input1, 0);
				Bowling.AddLast(0, 0);
				Bowling.AddLast(0, 0);
				finalScore = Bowling.GetLast().score;
				input1 = Convert.ToInt32(Console.ReadLine());
				if (input1 >= 0 && input1 <= 10)
				{
					input2 = Convert.ToInt32(Console.ReadLine());
					if (input2 >= 0 && input2 <= 10)
					{
						Bowling.AddLast(input1, input2, true);
						finalScore += input1 + input2 + (Bowling.IsStrike(Bowling.GetLast().prev.prev.prev) ? 10 : 0);
					}
					else
						Console.WriteLine("Invalid INPUT!");
				}
			}
			else
				Console.WriteLine("Invalid INPUT!");

			Bowling.PrintAllNodes();
			Console.WriteLine("Final Score = " + finalScore);
		}
	}
}

public class Node
{
	public Node next;
	public Node prev;
	public int roll1;
	public int roll2;
	public int score;
}

public class LinkedList
{
	private Node head;

	public void PrintAllNodes()
	{
		Node current = head;
		while (current != null)
		{
			Console.WriteLine(current.roll1 + ":" + current.roll2 + "=" + current.score);
			current = current.next;
		}
	}

	public Node GetLast()
	{
		Node current = head;
		while (current.next != null)
		{
			current = current.next;
		}
		return current.prev;
	}

	public void AddLast(int currentRoll1, int currentRoll2, bool last = false)
	{
		if (head == null)
		{
			head = new Node();
			head.roll1 = currentRoll1;
			head.roll2 = currentRoll2;
			head.next = null;
			head.prev = null;
			if (IsStrike(head))
				head.score = 0;
			else if (IsSpare(head))
				head.score = 0;
			else
				head.score = TotalNode(head);
		}
		else
		{
			Node addLast = new Node();
			addLast.roll1 = currentRoll1;
			addLast.roll2 = currentRoll2;

			Node current = head;
			while (current.next != null)
			{
				current = current.next;
			}
			current.next = addLast;
			addLast.prev = current;
			if(!last)
				AdjustScores(addLast);
		}
	}

	public void AdjustScores(Node current)
	{
		if (current.prev.prev != null)
		{
			if (IsStrike(current.prev.prev) && IsStrike(current.prev))
			{
				current.prev.prev.score += 10 + 10 + (!IsStrike(current) ? current.roll1 : 10);
				if (IsStrike(current))
				{
					current.prev.score = current.prev.prev.score;
					current.score = current.prev.prev.score;
				}
				else
				{
					current.prev.score = current.prev.prev.score + 10 + TotalNode(current);
					current.score = current.prev.score;
				}
			}
			else if (!IsStrike(current.prev.prev) && IsStrike(current.prev))
			{
				if (IsStrike(current))
				{
					current.score = current.prev.score;
				}
				else
				{
					current.prev.score += 10 + TotalNode(current);
					current.score = current.prev.score;
				}
			}
			else if (!IsStrike(current.prev.prev) && !IsStrike(current.prev))
			{
				if (IsSpare(current.prev))
				{
					current.prev.score += 10 + (!IsStrike(current) ? current.roll1 : TotalNode(current));
					current.score = current.prev.score;
				}
				else
				{
					if (IsStrike(current) || IsSpare(current))
					{
						current.prev.score += TotalNode(current.prev);
						current.score = current.prev.score;
					}
					else
					{
						current.prev.score += TotalNode(current.prev);
						current.score = current.prev.score;
					}
				}
			}
			else // (IsStrike(current.prev.prev) && !IsStrike(current.prev))
			{
				if (IsSpare(current.prev))
				{
					current.prev.score += 10 + current.roll1;
					current.score = current.prev.score;
				}
				else
				{
					if (IsStrike(current) || IsSpare(current))
					{
						current.prev.score += TotalNode(current.prev);
						current.score = current.prev.score;
					}
					else
					{
						current.prev.score += TotalNode(current.prev);
						current.score = current.prev.score;
					}
				}
			}
		}
		else
		{
			if (IsStrike(current.prev))
			{
				if (IsStrike(current))
				{
					current.score = current.prev.score;
				}
				else
				{
					current.prev.score += 10 + TotalNode(current);
					current.score = current.prev.score;
				}
			}
			else if (IsSpare(current.prev))
			{
				if (IsStrike(current))
				{
					current.prev.score += 10 + 10;
					current.score = current.prev.score;
				}
				else
				{
					current.prev.score += 10 + current.roll1;
					current.score = current.prev.score;
				}
			}
			else
			{
				if (IsStrike(current) || IsSpare(current))
				{
					current.score = current.prev.score;
				}
				else
				{
					current.score = current.prev.score;
				}
			}
		}
	}

	public bool IsStrike(Node current)
	{
		return (current.roll1 == 10);
	}

	public bool IsSpare(Node current)
	{
		return ((current.roll1 + current.roll2) == 10);
	}

	public int TotalNode(Node current)
	{
		return (current.roll1 + current.roll2);
	}
}


