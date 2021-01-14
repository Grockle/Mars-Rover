using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RobotManage
{
    internal static class Program
    {
        private static string GetProjectPath()
        {
            var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            var appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath ?? string.Empty).Value;
            return appRoot;
        }

        private static List<string> ReadInput(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open);
            using var reader = new StreamReader(fileStream);
            string line;
            var lines = new List<string>();
            while ((line = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line.TrimEnd()))
                {
                    lines.Add(line.TrimEnd().ToUpper());
                }
            }
            
            return lines.ToList();
        }

        private static Position CalculatePositions(List<string> positionParameters, int index)
        {
            try
            {
                var xCoordinate = Int16.Parse(positionParameters[0]);
                var yCoordinate = Int16.Parse(positionParameters[1]);
                var direction = positionParameters[2][0];

                if (!Constants.Directions.Contains(direction) || positionParameters[2].Length > 1)
                {
                    Console.WriteLine("Invalid start direction. Use only N E S W");
                    throw new Exception();
                }
                
                return new Position{XCoordinate = xCoordinate, YCoordinate = yCoordinate, Direction = direction};
            }
            catch (Exception e)
            {
                Console.WriteLine($"invalid positions for {index}. robot");
                throw;
            }
        }


        private static void ControlCommandsIsValid(char[] commands, int index)
        {
            if (commands.Any(command => !Constants.CommandTypes.Contains(command)))
            {
                Console.WriteLine($"Invalid direction type for {index}. robot");
                throw new Exception();
            }
        }

        private static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        static void Main(string[] args)
        {
            var projectPath = GetProjectPath();
            var filePath = $"{projectPath}\\input.txt";
            
            if (File.Exists(filePath))
            {
                var lines = ReadInput(filePath);

                //line number of input list must be least 3 line and odd
                if (lines.Count < 3 || !IsOdd(lines.Count))
                {
                    Console.Error.WriteLine("missing line");
                    throw new Exception();
                }

                var landSizes = lines.First().Split(" ");

                if (landSizes.Length != 2)
                {
                    Console.WriteLine("invalid upper-right coordinates for mars land");
                    throw new Exception();
                }

                try
                {
                    var land = new Land(Int32.Parse(landSizes[0]), Int32.Parse(landSizes[1]));
                        
                    var robotIndex = 0;
                    for (var lineIndex = 1; lineIndex < lines.Count; lineIndex += 2)
                    {
                        robotIndex++;
                        var robotCurrentPosition = lines[lineIndex];
                        var directionCommand = lines[lineIndex + 1];

                        var positionParameters = robotCurrentPosition.Split(' ');

                        if (positionParameters.Length < 3)
                        {
                            Console.WriteLine($"invalid positions for {robotIndex}. robot");
                            throw new Exception();
                        }

                        var position = CalculatePositions(positionParameters.ToList(), robotIndex);
                        
                        //to control positon of robot in range
                        if (land.IsOnOuterSpace(position))
                        {
                            Console.WriteLine($"{robotIndex}. robot position not in range of land upper-right coordinates");
                            throw new Exception();
                        }
                        
                        var robot = new Robot(position);

                        ControlCommandsIsValid(directionCommand.ToCharArray(), robotIndex);

                        land.AddRobotAndDirection(robot, directionCommand);
                    }
                    land.MoveRobots();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

            }
            else
            {
                Console.WriteLine("File is not exist");
            }
            
        }
    }
}
