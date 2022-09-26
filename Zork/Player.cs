namespace Zork
{
    internal class Player
    {
        private static Room CurrentRoom //property,  private static string GetCurrentRoom() is method. Functionally equivalent
        {
            get
            {
                return _world.Rooms[_location.Row, _location.Column];
            }
        }

        public int Score {get;}
        public int Moves { get; }

        public bool Move(Commands command)
        {
            Assert.IsTrue(IsDirection(command), "Invalid direction.");

            bool didMove = false;

            switch (command)
            {
                case Commands.NORTH when _location.Row < _world.Rooms.GetLength(0) - 1: // _rooms.GetLength(0) - number of rows, _rooms.GetLength(1) - number of columns
                    _location.Row++;
                    didMove = true;
                    break;

                case Commands.SOUTH when _location.Row > 0:
                    _location.Row--;
                    didMove = true;
                    break;

                case Commands.EAST when _location.Column < _world.Rooms.GetLength(1) - 1:
                    _location.Column++;
                    didMove = true;
                    break;

                case Commands.WEST when _location.Column > 0:
                    _location.Column--;
                    didMove = true;
                    break;

            }

            return didMove;
        }

        public Player(World world)
        {
            _world = world;
        }

        private World _world;
        private static (int Row, int Column) _location = (1, 1);

        private static bool IsDirection(Commands command) => _directions.Contains(command);
    }
}
