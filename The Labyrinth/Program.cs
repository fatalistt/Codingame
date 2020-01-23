using System;
using System.Collections.Generic;
using Path = System.Collections.Generic.IReadOnlyCollection<The_Labyrinth.Player.Point>;

namespace The_Labyrinth
{
	class Player
	{
		public struct Point
		{
			public readonly int x;
			public readonly int y;

			public Point(int x, int y)
			{
				this.x = x;
				this.y = y;
			}

			public override string ToString() => $"({x},{y})";
		}

		private enum Cell
		{
			Unknown,
			Wall,
			Hollow,
			StartingPosition,
			ControlRoom
		}

		private static int _height;
		private static int _width;
		private static int _alarmTime;
		private static Cell[,] _maze;
		private static Point _startPosition;
		private static Point _targetPosition;
		private static Point _currentPosition;
		private static Func<string> _currentAction = FindPath;
		private static Path _pathFromCurrentToTarget;
		private static Path _pathFromTargetToStart;
		private static IEnumerator<Point> _currentPathEnumerator;

		static void Main()
		{
			ReadConsts();

			while (true)
			{
				ReadCurrentPosition();
				ReadMaze();
				var moveWasMaded = MakeMove(_currentAction);
				if (!moveWasMaded)
					return;
			}
		}

		private static void ReadConsts()
		{
			var inputs = Console.ReadLine().Split(' ');
			_height = int.Parse(inputs[0]); // R, number of rows.
			_width = int.Parse(inputs[1]); // C, number of columns.
			_alarmTime = int.Parse(inputs[2]); // A, number of rounds between the time the alarm countdown is activated and the time the alarm goes off.
			_maze = new Cell[_height, _width];
		}

		private static void ReadCurrentPosition()
		{
			var inputs = Console.ReadLine().Split(' ');
			var y = int.Parse(inputs[0]); // row where Kirk is located.
			var x = int.Parse(inputs[1]); // column where Kirk is located.
			_currentPosition = new Point(x, y);
		}

		private static void ReadMaze()
		{
			for (var i = 0; i < _height; i++)
				ReadMazeRow(i);
		}

		private static void ReadMazeRow(int rowNumber)
		{
			var row = Console.ReadLine(); // C of the characters in '#.TC?' (i.e. one line of the ASCII maze).
			if (rowNumber < _currentPosition.y - 2 || rowNumber > _currentPosition.y + 2)
				return;
			for (var i = -2; i <= 2; ++i)
			{
				var colNumber = _currentPosition.x + i;
				_maze[rowNumber, colNumber] = CharToCell(row[colNumber]);
				if (_maze[rowNumber, colNumber] == Cell.ControlRoom)
					_targetPosition = new Point(colNumber, rowNumber);
				else if (_maze[rowNumber, colNumber] == Cell.StartingPosition)
					_startPosition = new Point(colNumber, rowNumber);
			}
		}

		private static Cell CharToCell(char c)
		{
			switch (c)
			{
				case '#':
					return Cell.Wall;
				case '.':
					return Cell.Hollow;
				case 'T':
					return Cell.StartingPosition;
				case 'C':
					return Cell.ControlRoom;
				case '?':
					return Cell.Unknown;
				default:
					throw new ArgumentOutOfRangeException("char", c, null);
			}
		}

		private static bool MakeMove(Func<string> action)
		{
			if (ReferenceEquals(action, null))
				throw new ArgumentNullException(nameof(action));

			var direction = action();
			if (string.Equals(direction, "STOP", StringComparison.InvariantCulture))
				return false;
			Console.WriteLine(direction); // Kirk's next move (UP DOWN LEFT or RIGHT).
			return true;
		}

		private static string FindPath()
		{
			Path pathFromCurrentToTarget = TryFindPathFromTo(_currentPosition, _targetPosition);
			if (!ReferenceEquals(pathFromCurrentToTarget, null))
			{
				Path pathFromTargetToStart = TryFindPathFromTo(_targetPosition, _startPosition);
				if (!ReferenceEquals(pathFromTargetToStart, null) && pathFromTargetToStart.Count <= _alarmTime)
				{
					_pathFromCurrentToTarget = pathFromCurrentToTarget;
					_pathFromTargetToStart = pathFromTargetToStart;
					_currentAction = GoToTarget;
					return GoToTarget();
				}
			}
			//move to explore more
			throw new NotImplementedException();
		}

		private static string GoToTarget()
		{
			if (_currentPosition.x == _targetPosition.x && _currentPosition.y == _targetPosition.y)
			{
				_currentPathEnumerator?.Dispose();
				_currentPathEnumerator = null;
				_currentAction = GoToStart;
				return GoToStart();
			}
			_currentPathEnumerator = _currentPathEnumerator ?? _pathFromCurrentToTarget.GetEnumerator();
			if (_currentPathEnumerator.MoveNext())
				return GetDirectionFromTo(_currentPosition, _currentPathEnumerator.Current);
			else
				throw new Exception("Path to target is not completed.");
		}

		private static string GoToStart()
		{
			if (_currentPosition.x == _targetPosition.x && _currentPosition.y == _targetPosition.y)
			{
				_currentPathEnumerator?.Dispose();
				_currentPathEnumerator = null;
				return "STOP";
			}
			_currentPathEnumerator = _currentPathEnumerator ?? _pathFromTargetToStart.GetEnumerator();
			if (_currentPathEnumerator.MoveNext())
				return GetDirectionFromTo(_currentPosition, _currentPathEnumerator.Current);
			else
				throw new Exception("Path to start is not completed.");
		}

		private static string GetDirectionFromTo(Point from, Point to)
		{
			if (to.x > from.x)
				return "RIGHT";
			else if (to.x < from.x)
				return "LEFT";
			else if (to.y > from.y)
				return "UP";
			else if (to.y < from.y)
				return "DOWN";
			else
				throw new InvalidOperationException($"from {from} and {to} are the same point!");
		}

		private static Path TryFindPathFromTo(Point from, Point to)
		{
			throw new NotImplementedException();
		}
	}
}