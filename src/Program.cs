using System;
namespace Battleships
{
    class Program
    {
        static void Main()
        {
            int mapWidth =  8;
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

            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ";

            Menu();

            // Metoder
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
                // Metod som sparar kartans storlek i en array
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
                Console.WriteLine($"The board is {mapWidth}x{mapHeight} big");

                Thread.Sleep(1000);

                int x;
                int y;

                for (int i = 0; i < 2; i++)
                {
                    int[] positionReceive = PositionParse();
                    x = positionReceive[0] - 1;
                    y = positionReceive[1] - 1;
                    while (playerMap[x, y] == "X")
                    {
                        Console.WriteLine("Another ship exists in this location, please choose again");
                        Thread.Sleep(1000);
                        positionReceive = PositionParse();
                        x = positionReceive[0] - 1;
                        y = positionReceive[1] - 1;
                    }
                    playerMap[x, y] = "X";
                }

                int z = rand.Next(mapWidth);
                int v = rand.Next(mapHeight);
                computerMap[z, v] = "X";
                for (int i = 0; i < 1; i++)
                {
                    z = rand.Next(mapWidth);
                    v = rand.Next(mapHeight);
                    while (computerMap[z, v] == "X")
                    {
                        z = rand.Next(mapWidth);
                        v = rand.Next(mapHeight);
                    }
                    computerMap[z, v] = "X";
                }
            }


            // Metod som ritar ut spelarkartan och datorkartan
            void DrawMap()
            {
                // Loop som ritar ut spelarkartorna bredvid varandra
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
                                // Röd visar att det är en träff
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

                            // Återställ färgen
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        Console.Write("    ");
                        Console.Write($"{y + 1} ");
                        for (int x = 0; x < mapWidth; x++)
                        {

                            if (playerShot[x, y] == true && computerMap[x, y] == "X")
                            {
                                // grön visar att det är en träff
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

                            // Återställ färgen
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        Console.WriteLine();
                    }
                }
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
                                // Röd visar att det är en träff
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

                            // Återställ färgen
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
                                // Röd visar att det är en träff
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

                            // Återställ färgen
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        Console.WriteLine();
                    }
                }
                Console.WriteLine();
                // Loop som ritar ut datorkartan
                Console.WriteLine("Computer's map");
                // här ska vi sätta en if-sats som beror på ett menyval i början "Fusk/Test". alltså om man inte har fusk kör den första forloopen, om man har fusk så kör den en sats som ritar ut X för datorn

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
                    //if (yCoord < 0 || x < 0 || yCoord > mapHeight || x > mapWidth)
                    //{
                    //    Console.WriteLine("Position out of bounds. Try again.");
                    //}
                }
                int[] shotReturn = new int[2];
                shotReturn[0] = x;
                shotReturn[1] = y;
                return shotReturn;

            }



            int ReadInt()
            {
                // Läser in ett heltal inom arrayen
                int num;
                while (int.TryParse(Console.ReadLine(), out num) == false)
                {
                    Console.WriteLine("Not within playing field");
                }
                return num;
            }

            bool HasPlayerWon()
            {
                // Kollar om det finns några osänkta skepp
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
                //kollar om det finns några osänkta skepp
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
                int y = rand.Next(mapHeight);
                int x = rand.Next(mapWidth);
                //if (intelligence == false)
                //{
                    while (computerShot[x, y] == true)
                    {
                        y = rand.Next(mapHeight);
                        x = rand.Next(mapWidth);
                    }
                    computerShot[x, y] = true;
                //    if (playerMap[x, y] == "X")
                //    {
                //        intelligence = true;
                //    }
                //}
                //if (intelligence == true)
                //{
                //    if (playerMap[x, y] == "X")
                //    {

                //    }
                //}

            }

            void Menu()
            {
                Console.Clear();
                string choice = "";
                while (choice != "4")
                {
                    Console.WriteLine("1. Play Battleships");
                    Console.WriteLine("2. Show latest winner");
                    Console.WriteLine("3. Settings");
                    Console.WriteLine("4. Exit");
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
                                    int x = -9999;
                                    int y = -9999;
                                    if (x < 2 || x > mapWidth || y < 2 || y > mapHeight)
                                    {
                                        Console.WriteLine("Enter playfield size 'AxB'");
                                        string size = Console.ReadLine();
                                        x = int.Parse(size.Substring(0, 1));
                                        y = int.Parse(size.Substring(2, 1));
                                        while (x < 2 || x > mapWidth || y < 2 || y > mapHeight)
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
                            Console.WriteLine("Exiting...");
                            break;
                    }
                }
            }


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

        }
    }
}
