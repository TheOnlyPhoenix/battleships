using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SinkShips
{
    class Program
    {
        static void Main()
        {
            int mapWidth = 8;
            int mapHeight = 8;

            string[,] playerMap = new string[mapWidth, mapHeight];
            string[,] computerMap = new string[mapWidth, mapHeight];
            bool[,] computerShip = new bool[mapWidth, mapHeight];
            bool[,] playerShot = new bool[mapWidth, mapHeight];
            bool[,] computerShot = new bool[mapWidth, mapHeight];
            Random rand = new Random();

            string winner = "";
            bool isWinner = false;
            bool anyWinner = false;

            bool testingMode = false;
            bool intelligence = false;
            bool inputCheck = false;
            bool skipShot = false;

            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ";
            int rememberPositionX = 0;
            int rememberPositionY = 0;

            Menu();

            // Methods
            void PlayTheGame()
            {

                SaveMap();
                PlaceShips();
                while (anyWinner == false)
                {
                    Console.Clear();
                    DrawMap();
                    MarkShot();
                    DrawMap();
                    CheckComputerShot();

                    if (HasPlayerWon())
                    {
                        Console.Clear();
                        anyWinner = true;
                        Console.WriteLine("You won!");
                        Console.WriteLine("Please enter your name:");
                        winner = Console.ReadLine();
                        isWinner = true;
                    }
                    else if (HasComputerWon())
                    {
                        Console.Clear();
                        anyWinner = true;
                        Console.WriteLine("You lost");
                        Console.ReadKey();
                    }
                }
            }

            void SaveMap()
            {
                // Method that saves the map's unchanged layout to arrays
                for (int y = 0; y < mapHeight; y++)
                {
                    for (int x = 0; x < mapWidth; x++)
                    {
                        playerMap[x, y] = "O";
                        computerMap[x, y] = "O";
                        playerShot[x, y] = false;
                        computerShot[x, y] = false;
                    }
                }
            }

            void PlaceShips()
            {
                // Lets the user place ships on the playfield and updates dynamically, showing previously placed ships
                int x;
                int y;

                Console.Clear();

                MiniMap();

                Console.WriteLine($"The board is {mapWidth}x{mapHeight} big");
                Thread.Sleep(500);
                Console.WriteLine("Single Ship");
                int[] positionReceive = PositionParse();
                x = positionReceive[0] - 1;
                y = positionReceive[1] - 1;
                playerMap[x, y] = "X";

                Console.Clear();

                MiniMap();

                Console.WriteLine("1st Double Ship (Horizontal)");
                positionReceive = PositionParseXPlusOne();
                x = positionReceive[0] - 1;
                y = positionReceive[1] - 1;
                while (playerMap[x, y] == "X" || playerMap[x + 1, y] == "X" || x + 1 >= mapWidth)
                {
                    Console.WriteLine("Another ship exists in this location, please choose again");
                    Thread.Sleep(1000);
                    positionReceive = PositionParseXPlusOne();
                    x = positionReceive[0] - 1;
                    y = positionReceive[1] - 1;
                }
                playerMap[x, y] = "X";
                playerMap[x + 1, y] = "X";

                Console.Clear();

                MiniMap();

                Console.WriteLine("2nd Double Ship (Vertical)");
                positionReceive = PositionParseYPlusOne();
                x = positionReceive[0] - 1;
                y = positionReceive[1] - 1;
                while (playerMap[x, y] == "X" || playerMap[x, y + 1] == "X")
                {
                    Console.WriteLine("Another ship exists in this location, please choose again");
                    Thread.Sleep(1000);
                    positionReceive = PositionParseYPlusOne();
                    x = positionReceive[0] - 1;
                    y = positionReceive[1] - 1;
                }
                playerMap[x, y] = "X";
                playerMap[x, y + 1] = "X";

                Console.Clear();

                MiniMap();

                Console.WriteLine("1st Triple Ship (Horizontal)");
                positionReceive = PositionParseXPlusTwo();
                x = positionReceive[0] - 1;
                y = positionReceive[1] - 1;
                while (playerMap[x, y] == "X" || playerMap[x + 1, y] == "X" || playerMap[x + 2, y] == "X" || x + 1 >= mapWidth || x + 2 >= mapWidth)
                {
                    Console.WriteLine("Another ship exists in this location, please choose again");
                    Thread.Sleep(1000);
                    positionReceive = PositionParseXPlusTwo();
                    x = positionReceive[0] - 1;
                    y = positionReceive[1] - 1;
                }
                playerMap[x, y] = "X";
                playerMap[x + 1, y] = "X";
                playerMap[x + 2, y] = "X";

                Console.Clear();

                MiniMap();

                Console.WriteLine("2nd Triple Ship (Vertical)");
                positionReceive = PositionParseYPlusTwo();
                x = positionReceive[0] - 1;
                y = positionReceive[1] - 1;
                while (playerMap[x, y] == "X" || playerMap[x, y + 1] == "X" || playerMap[x, y + 2] == "X" || y + 1 >= mapHeight || y + 2 >= mapHeight)
                {
                    Console.WriteLine("Another ship exists in this location, please choose again");
                    Thread.Sleep(1000);
                    positionReceive = PositionParseYPlusTwo();
                    x = positionReceive[0] - 1;
                    y = positionReceive[1] - 1;
                }
                playerMap[x, y] = "X";
                playerMap[x, y + 1] = "X";
                playerMap[x, y + 2] = "X";

                // Single ship
                x = rand.Next(mapWidth);
                y = rand.Next(mapHeight);
                computerMap[x, y] = "X";

                //   Double ship X
                x = rand.Next(mapWidth - 1);
                y = rand.Next(mapHeight);
                while (computerMap[x, y] == "X" || computerMap[x + 1, y] == "X")
                {
                    x = rand.Next(mapWidth - 1);
                    y = rand.Next(mapHeight);
                }
                computerMap[x, y] = "X";
                computerMap[x + 1, y] = "X";

                // Double Ship Y
                x = rand.Next(mapWidth);
                y = rand.Next(mapHeight - 1);
                while (computerMap[x, y] == "X" || computerMap[x, y + 1] == "X")
                {
                    x = rand.Next(mapWidth);
                    y = rand.Next(mapHeight - 1);
                }
                computerMap[x, y] = "X";
                computerMap[x, y + 1] = "X";

                // Triple Ship X
                x = rand.Next(mapWidth - 2);
                y = rand.Next(mapHeight);
                while (computerMap[x, y] == "X" || computerMap[x + 1, y] == "X" || computerMap[x + 2, y] == "X")
                {
                    x = rand.Next(mapWidth - 2);
                    y = rand.Next(mapHeight);
                }
                computerMap[x, y] = "X";
                computerMap[x + 1, y] = "X";
                computerMap[x + 2, y] = "X";

                // Triple Ship Y
                x = rand.Next(mapWidth);
                y = rand.Next(mapHeight - 2);
                while (computerMap[x, y] == "X" || computerMap[x, y + 1] == "X" || computerMap[x, y + 2] == "X")
                {
                    x = rand.Next(mapWidth);
                    y = rand.Next(mapHeight - 2);
                }
                computerMap[x, y] = "X";
                computerMap[x, y + 1] = "X";
                computerMap[x, y + 2] = "X";


            }


            // Draws the computer's and player's maps
            void DrawMap()
            {
                // Draws the maps next to each other
                Console.Write("Player");
                for (int p = 0; p < mapWidth; p++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine("Computer");
                if (testingMode == false)
                {
                    Console.WriteLine($"  {alphabet.Substring(0, mapWidth)}      {alphabet.Substring(0, mapWidth)}");
                    for (int y = 0; y < mapHeight; y++)
                    {
                        Console.Write($"{y + 1} ");
                        for (int x = 0; x < mapWidth; x++)
                        {
                            if (computerShot[x, y] == true)
                            {
                                // Red shows shot, regardless of hit or miss
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(playerMap[x, y]);
                            }
                            else if (playerMap[x, y] == "X")
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write(playerMap[x, y]);
                            }
                            else
                            {
                                Console.Write(playerMap[x, y]);
                            }

                            // Reset colours
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        Console.Write("    ");
                        Console.Write($"{y + 1} ");
                        for (int x = 0; x < mapWidth; x++)
                        {

                            if (playerShot[x, y] == true && computerMap[x, y] == "X")
                            {
                                // Green shows a hit
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(computerMap[x, y]);
                            }
                            else if (playerShot[x, y] == true)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("O");
                            }
                            else
                            {
                                Console.Write("-");
                            }

                            // Reset colours
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        Console.WriteLine();
                    }
                }
                // "Cheat" mode that shows the computer's ships
                else if (testingMode == true)
                {
                    Console.WriteLine($"  {alphabet.Substring(0, mapWidth)}      {alphabet.Substring(0, mapWidth)}");
                    for (int y = 0; y < mapHeight; y++)
                    {
                        Console.Write($"{y + 1} ");
                        for (int x = 0; x < mapWidth; x++)
                        {
                            if (computerShot[x, y] == true)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(playerMap[x, y]);
                            }
                            else if (playerMap[x, y] == "X")
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write(playerMap[x, y]);
                            }
                            else
                            {
                                Console.Write(playerMap[x, y]);
                            }

                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        Console.Write("    ");
                        Console.Write($"{y + 1} ");
                        for (int x = 0; x < mapWidth; x++)
                        {
                            if (playerShot[x, y] == false && computerMap[x, y] == "X")
                            {
                                Console.Write(computerMap[x, y]);
                            }
                            else if (playerShot[x, y] == true && computerMap[x, y] == "X")
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(computerMap[x, y]);
                            }
                            else if (playerShot[x, y] == true)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("O");
                            }
                            else
                            {
                                Console.Write("-");
                            }

                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        Console.WriteLine();
                    }
                }

            }

            void MarkShot()
            {
                int[] shot = CombinedShot();
                int x = shot[0] - 1;
                int y = shot[1] - 1;

                playerShot[x, y] = true;

            }

            int[] CombinedShot()
            {
                int x = -9999;
                int y = -9999;
                int inputLength = 2;
                while (y < 0 || x < 0 || y > mapHeight || x > mapWidth || inputCheck == false)
                {
                    inputCheck = true;
                    Console.WriteLine("Enter the coordinate you would like to hit (AX)");
                    string position = Console.ReadLine().ToUpper();
                    while (position.Length != inputLength || !alphabet.Substring(0, mapWidth).Contains(position.Substring(0, 1)))
                    {
                        Console.WriteLine("Invalid position.");
                        Console.WriteLine("Enter the coordinate (AX)");
                        position = Console.ReadLine().ToUpper();
                    }
                    string xPosition = position.Substring(0, 1);
                    y = int.Parse(position.Substring(1, 1));
                    if (alphabet.Substring(0, mapWidth).Contains(xPosition) && (y <= mapHeight && y > 0))
                    {
                        x = alphabet.IndexOf(xPosition) + 1;
                    }
                    else
                    {
                        inputCheck = false;
                        Console.WriteLine("Invalid input. Try again");
                    }
                }
                int[] shotReturn = new int[2];
                shotReturn[0] = x;
                shotReturn[1] = y;
                return shotReturn;

            }


            bool HasPlayerWon()
            {
                // Checks if there are any unsunk ships on the computer's playing field
                for (int y = 0; y < mapHeight; y++)
                {
                    for (int x = 0; x < mapWidth; x++)
                    {
                        if (computerMap[x, y] == "X" && playerShot[x, y] == false)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            bool HasComputerWon()
            {
                // Checks if there are any unsunk ships on the player's playing field
                for (int y = 0; y < mapHeight; y++)
                {
                    for (int x = 0; x < mapWidth; x++)
                    {
                        if (playerMap[x, y] == "X" && computerShot[x, y] == false)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            void CheckComputerShot()
            {
                int x;
                int y;
                x = rand.Next(mapWidth);
                y = rand.Next(mapHeight);

                while (computerShot[x, y] == true)
                {
                    x = rand.Next(mapWidth);
                    y = rand.Next(mapHeight);
                }
                computerShot[x, y] = true;

                // Very bad attempt at trying to make the computer shoot at all non-diagonal ships surrounding the last hit.
                // I don't know what I'm doing wrong nor how to fix it. Commented the "broken" code out and right now it's just 
                // using a completely dumb AI.

                /* List<int> surroundingCoords = new List<int>();

                if (intelligence == false)
                {
                    x = rand.Next(mapWidth);
                    y = rand.Next(mapHeight);

                    while (computerShot[x, y] == true)
                    {
                        x = rand.Next(mapWidth);
                        y = rand.Next(mapHeight);
                    }
                    computerShot[x, y] = true;
                    if (playerMap[x, y] == "X")
                    {
                        rememberPositionX = x;
                        rememberPositionY = y;
                    }

                }
                
                while (surroundingCoords.Count < 1)
                {
                    
                    surroundingCoords.Add(1);
                    surroundingCoords.Add(2);
                    surroundingCoords.Add(3);
                    surroundingCoords.Add(4);
                }
                if (intelligence == true)
                {
                    x = rememberPositionX;
                    y = rememberPositionY;
                    if (surroundingCoords.Count == 4)
                    {
                        if (x + 1 <= mapWidth || computerShot[x + 1, y] == false)
                        {
                            x = rememberPositionX + 1;
                            computerShot[x, y] = true;
                        }
                        else
                        {
                            skipShot = true;
                        }
                    }
                    if (surroundingCoords.Count == 3 || skipShot == true)
                    {
                        skipShot = false;
                        if (y + 1 <= mapHeight || computerShot[x, y + 1] == false)
                        {
                            y = rememberPositionY + 1;
                            computerShot[x, y] = true;
                        }
                        else
                        {
                            skipShot = true;
                        }
                    }
                    if (surroundingCoords.Count == 2 || skipShot == true)
                    {
                        skipShot = false;
                        if (x - 1 <= mapWidth || computerShot[x - 1, y] == false)
                        {
                            x = rememberPositionX - 1;
                            computerShot[x, y] = true;
                        }
                        else
                        {
                            skipShot = true;
                        }
                    }
                    if (surroundingCoords.Count == 1 || skipShot == true)
                    {
                        skipShot = false;
                        if (y - 1 <= mapWidth || computerShot[x, y - 1] == false)
                        {
                            y = rememberPositionY - 1;
                            computerShot[x, y] = true;
                        }
                        else
                        {
                            skipShot = true;
                        }

                        if (surroundingCoords.Count > 1)
                        {
                            for (int i = surroundingCoords.Count; i > 1; i--)
                            {
                                surroundingCoords.RemoveAt(i - 1);
                            }
                        }
                    }

                    surroundingCoords.RemoveAt(surroundingCoords.Count - 1);
                }
                */
            }

            void Menu()
            {
                Console.Clear();
                string choice = "";
                while (choice != "5")
                {
                    Console.WriteLine("1. Play Battleships");
                    Console.WriteLine("2. Show latest winner");
                    Console.WriteLine("3. Settings");
                    Console.WriteLine("4. Game information");
                    Console.WriteLine("5. Exit");
                    choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            Console.Clear();
                            anyWinner = false;
                            PlayTheGame();
                            break;
                        case "2":
                            Console.Clear();
                            if (isWinner == true)
                            {
                                Console.WriteLine(winner);
                            }
                            else
                            {
                                Console.WriteLine("No previous winner");
                            }
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        case "3":
                            Console.Clear();
                            Console.WriteLine("1. Testing mode");
                            Console.WriteLine("2. Playfield size");
                            Console.WriteLine("3. Go back");
                            string settingChoice = Console.ReadLine();
                            switch (settingChoice)
                            {
                                case "1":
                                    // Option to enable the "cheat" mode which shows all the opponent's ships
                                    Console.Clear();
                                    Console.WriteLine("Do you want to enable testing? y/n");
                                    string answer = Console.ReadLine();
                                    if (answer == "y")
                                    {
                                        testingMode = true;
                                    }
                                    else if (answer == "n")
                                    {
                                        testingMode = false;
                                    }
                                    break;
                                case "2":
                                    // Allows the user to change the playfield size
                                    int x = -9999;
                                    int y = -9999;
                                    if (x < 2 || x > mapWidth || y < 2 || y > mapHeight)
                                    {
                                        Console.WriteLine("Enter playfield size 'AxB'");
                                        string size = Console.ReadLine();
                                        x = int.Parse(size.Substring(0, 1));
                                        y = int.Parse(size.Substring(2, 1));
                                        while (x < 2 || x > mapWidth || y < 2 || y > mapHeight || size.Length != 2)
                                        {
                                            Console.WriteLine("Invalid Size.");
                                            Thread.Sleep(1000);
                                            Console.WriteLine("Enter playfield size 'AxB'");
                                            size = Console.ReadLine();
                                            x = int.Parse(size.Substring(0, 1));
                                            y = int.Parse(size.Substring(2, 1));
                                        }

                                    }
                                    mapHeight = x;
                                    mapWidth = y;
                                    break;
                                case "3":
                                    break;
                            }
                            Console.Clear();
                            break;
                        case "4":
                            // Explains the game design
                            Console.Clear();
                            Console.WriteLine("Your placed ship is Blue and marked with an X. It will turn Red if hit by the computer.");
                            Console.WriteLine("If the computer misses, the white O on your map will turn Red.");
                            Console.WriteLine("The computer's map is marked with '-' by default. They will turn into Green X:s if you ");
                            Console.WriteLine("hit a ship, and red O:s if you miss.");
                            Console.WriteLine("Both players have 5 ships: 1 single length, 2 double length, 2 triple length. One of ");
                            Console.WriteLine("each longer ship is vertical, the other is horizontal.");

                            break;
                        case "5":
                            Console.WriteLine("Exiting...");
                            break;
                    }
                }
            }

            // The following five methods are extremely ugly, but it's the only solution I could come up with.
            // Their purpose is to check that the position is within the array for each of the five ships.  
            // The difference between the first method and the rest is that the rest have checks for their individual ships' positions.
            int[] PositionParse()
            {
                int yCoord = -9999;
                int xCoord = -9999;
                int inputLength = 2;

                while (yCoord < 0 || xCoord < 0 || yCoord > mapHeight || xCoord > mapWidth)
                {
                    Console.WriteLine("Enter the coordinate (AX)");
                    string position = Console.ReadLine().ToUpper();

                    while (position.Length != inputLength || !alphabet.Substring(0, mapWidth).Contains(position.Substring(0, 1)))
                    {
                        // A loop that confirms that the input is 2 symbols long and that it is correctly formatted (AX)
                        Console.WriteLine("Invalid position.");
                        Console.WriteLine("Enter the coordinate (AX)");
                        position = Console.ReadLine().ToUpper();
                    }
                    string xPosition = position.Substring(0, 1);
                    yCoord = int.Parse(position.Substring(1, 1));
                    xCoord = alphabet.IndexOf(xPosition) + 1;
                    if (yCoord < 0 || xCoord < 0 || yCoord > mapHeight || xCoord > mapWidth)
                    {
                        Console.WriteLine("Position out of bounds. Try again.");
                    }
                }

                int[] positionReturn = new int[2];
                positionReturn[0] = xCoord;
                positionReturn[1] = yCoord;
                return positionReturn;
            }

            int[] PositionParseXPlusOne()
            {
                int yCoord = -9999;
                int xCoord = -9999;
                int inputLength = 2;

                while (yCoord < 0 || xCoord < 0 || yCoord > mapHeight || xCoord > mapWidth || xCoord + 1 > mapHeight)
                {
                    Console.WriteLine("Enter the coordinate (AX)");
                    string position = Console.ReadLine().ToUpper();

                    while (position.Length != inputLength || !alphabet.Substring(0, mapWidth).Contains(position.Substring(0, 1)))
                    {
                        Console.WriteLine("Invalid position.");
                        Console.WriteLine("Enter the coordinate (AX)");
                        position = Console.ReadLine().ToUpper();
                    }
                    string xPosition = position.Substring(0, 1);
                    yCoord = int.Parse(position.Substring(1, 1));
                    xCoord = alphabet.IndexOf(xPosition) + 1;
                    if (yCoord < 0 || xCoord < 0 || yCoord > mapHeight || xCoord > mapWidth || xCoord + 1 > mapHeight)
                    {
                        Console.WriteLine("Position out of bounds. Try again.");
                    }
                }

                int[] positionReturn = new int[2];
                positionReturn[0] = xCoord;
                positionReturn[1] = yCoord;
                return positionReturn;
            }

            int[] PositionParseYPlusOne()
            {
                int yCoord = -9999;
                int xCoord = -9999;
                int inputLength = 2;

                while (yCoord < 0 || xCoord < 0 || yCoord > mapHeight || xCoord > mapWidth || yCoord + 1 > mapHeight)
                {
                    Console.WriteLine("Enter the coordinate (AX)");
                    string position = Console.ReadLine().ToUpper();

                    while (position.Length != inputLength || !alphabet.Substring(0, mapWidth).Contains(position.Substring(0, 1)))
                    {
                        Console.WriteLine("Invalid position.");
                        Console.WriteLine("Enter the coordinate (AX)");
                        position = Console.ReadLine().ToUpper();
                    }
                    string xPosition = position.Substring(0, 1);
                    yCoord = int.Parse(position.Substring(1, 1));
                    xCoord = alphabet.IndexOf(xPosition) + 1;
                    if (yCoord < 0 || xCoord < 0 || yCoord > mapHeight || xCoord > mapWidth || yCoord + 1 > mapHeight)
                    {
                        Console.WriteLine("Position out of bounds. Try again.");
                    }
                }

                int[] positionReturn = new int[2];
                positionReturn[0] = xCoord;
                positionReturn[1] = yCoord;
                return positionReturn;
            }

            int[] PositionParseXPlusTwo()
            {
                int yCoord = -9999;
                int xCoord = -9999;
                int inputLength = 2;

                while (yCoord < 0 || xCoord < 0 || yCoord > mapHeight || xCoord > mapWidth || xCoord + 1 > mapHeight || xCoord + 2 > mapHeight)
                {
                    Console.WriteLine("Enter the coordinate (AX)");
                    string position = Console.ReadLine().ToUpper();

                    while (position.Length != inputLength || !alphabet.Substring(0, mapWidth).Contains(position.Substring(0, 1)))
                    {
                        Console.WriteLine("Invalid position.");
                        Console.WriteLine("Enter the coordinate (AX)");
                        position = Console.ReadLine().ToUpper();
                    }
                    string xPosition = position.Substring(0, 1);
                    yCoord = int.Parse(position.Substring(1, 1));
                    xCoord = alphabet.IndexOf(xPosition) + 1;
                    if (yCoord < 0 || xCoord < 0 || yCoord > mapHeight || xCoord > mapWidth || xCoord + 1 > mapHeight || xCoord + 2 > mapHeight)
                    {
                        Console.WriteLine("Position out of bounds. Try again.");
                    }
                }

                int[] positionReturn = new int[2];
                positionReturn[0] = xCoord;
                positionReturn[1] = yCoord;
                return positionReturn;
            }

            int[] PositionParseYPlusTwo()
            {
                int yCoord = -9999;
                int xCoord = -9999;
                int inputLength = 2;

                while (yCoord < 0 || xCoord < 0 || yCoord > mapHeight || xCoord > mapWidth || yCoord + 1 > mapHeight || yCoord + 2 > mapHeight)
                {
                    Console.WriteLine("Enter the coordinate (AX)");
                    string position = Console.ReadLine().ToUpper();

                    while (position.Length != inputLength || !alphabet.Substring(0, mapWidth).Contains(position.Substring(0, 1)))
                    {
                        Console.WriteLine("Invalid position.");
                        Console.WriteLine("Enter the coordinate (AX)");
                        position = Console.ReadLine().ToUpper();
                    }
                    string xPosition = position.Substring(0, 1);
                    yCoord = int.Parse(position.Substring(1, 1));
                    xCoord = alphabet.IndexOf(xPosition) + 1;
                    if (yCoord < 0 || xCoord < 0 || yCoord > mapHeight || xCoord > mapWidth || yCoord + 1 > mapHeight || yCoord + 2 > mapHeight)
                    {
                        Console.WriteLine("Position out of bounds. Try again.");
                    }
                }

                int[] positionReturn = new int[2];
                positionReturn[0] = xCoord;
                positionReturn[1] = yCoord;
                return positionReturn;
            }

            void MiniMap()
            {
                // Map that is updated each time a new ship is placed. Only draws the Player's map
                Console.WriteLine($"  {alphabet.Substring(0, mapWidth)}");
                for (int p = 0; p < mapHeight; p++)
                {
                    Console.Write($"{p + 1} ");
                    for (int q = 0; q < mapWidth; q++)
                    {
                        if (computerShot[q, p] == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(playerMap[q, p]);
                        }
                        else if (playerMap[q, p] == "X")
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(playerMap[q, p]);
                        }
                        else
                        {
                            Console.Write(playerMap[q, p]);
                        }

                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    Console.WriteLine();
                }
            }
    }
    }
}