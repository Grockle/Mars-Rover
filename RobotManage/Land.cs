﻿using System;
using System.Collections.Generic;

namespace RobotManage
{
    public class Land
    {
        private int RightCoordinate { get; set; }
        private int UpperCoordinate { get; set; }

        public Dictionary<Robot, string> RobotsAndDirectionCommands = new Dictionary<Robot, string>();

        public Land(int xLength, int yLength)
        {
            RightCoordinate = xLength;
            UpperCoordinate = yLength;
        }

        public void AddRobotAndDirection(Robot robot, string command)
        {
            RobotsAndDirectionCommands.Add(robot, command);
        }

        public void MoveRobots()
        {
            var robotIndex = 1;
            foreach (var robotsAndDirectionCommand in RobotsAndDirectionCommands)
            {
                var robot = robotsAndDirectionCommand.Key;
                var command = robotsAndDirectionCommand.Value;
                robot.FindLastLocation(command);
                WriteRobotCurrentDirection(robot.GetCurrentPosition(), robotIndex);
                robotIndex++;
            }
        }

        public void WriteRobotCurrentDirection(Position robotCurrentPosition, int robotIndex)
        {
            if (IsOnOuterSpace(robotCurrentPosition))
            {
                Console.WriteLine($"{robotIndex}. Robot on empty space");
            }
            else
            {
                Console.WriteLine($"{robotCurrentPosition.XCoordinate} {robotCurrentPosition.YCoordinate} {robotCurrentPosition.Direction}");
            }
        }

        public bool IsOnOuterSpace(Position position)
        {
            return (position.XCoordinate < 0 || position.XCoordinate > RightCoordinate) ||
                   (position.YCoordinate < 0 || position.YCoordinate > UpperCoordinate);
        }
    }
}
