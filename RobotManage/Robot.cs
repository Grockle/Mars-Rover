using System;
using System.Linq;

namespace RobotManage
{
    public class Robot
    {
        private Position RobotCurrentPosition { get; set; }

        public Robot(Position position)
        {
            RobotCurrentPosition = position;
        }

        public Position GetCurrentPosition()
        {
            return RobotCurrentPosition;
        }

        public void FindLastLocation(string moveCommands)
        {
            var steps = moveCommands.ToCharArray();
            foreach (var step in steps)
            {
                switch (step)
                {
                    case 'R':
                        TurnRight();
                        break;
                    case 'L':
                        TurnLeft();
                        break;
                    case 'M':
                        Move();
                        break;
                }
            }
        }

        private void TurnRight()
        {
            var index = Array.FindIndex(Constants.Directions, c => c.Equals(RobotCurrentPosition.Direction));
            var lastIndex = Constants.Directions.Length - 1;
            RobotCurrentPosition.Direction = index == lastIndex ? Constants.Directions.First() : Constants.Directions[index + 1];
        }

        private void TurnLeft()
        {
            var index = Array.FindIndex(Constants.Directions, c => c.Equals(RobotCurrentPosition.Direction));
            RobotCurrentPosition.Direction = index == 0 ? Constants.Directions.Last() : Constants.Directions[index - 1];
        }

        private void Move()
        {
            switch (RobotCurrentPosition.Direction)
            {
                case 'N':
                    Up();
                    break;
                case 'E':
                    Right();
                    break;
                case 'S':
                    Down();
                    break;
                case 'W':
                    Left();
                    break;
            }
        }

        private void Up()
        {
            RobotCurrentPosition.YCoordinate++;
        }

        private void Down()
        {
            RobotCurrentPosition.YCoordinate--;
        }

        private void Left()
        {
            RobotCurrentPosition.XCoordinate--;
        }

        private void Right()
        {
            RobotCurrentPosition.XCoordinate++;
        }
    }
}
