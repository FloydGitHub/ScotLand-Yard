using Scotland_Yard_V1.Classes;
using System;

namespace Scotland_Yard_V1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool ProgramRunning = true;
            while (ProgramRunning)
            {
                int MainMenuChoice = MainMenu();
                switch (MainMenuChoice)
                {
                    case 1:
                        List<Location> board = Location.MakeBoard();
                        // create pieces use case
                        Game game = new Game();
                        game.DalCreateGame();

                        List<PlayingPiece> playingPieces = CreatePieces(game.Id);
                        game.GivePlayingPieces(playingPieces);
                        List<Location> availableLocations = new List<Location>(board);
                        // decide starting location usecase
                        foreach (PlayingPiece playingPiece in playingPieces)
                        {
                            Console.WriteLine($"Speler {playingPiece.Player.Name} kies een locatie voor {playingPiece.Name}");
                            ChooseLocation(availableLocations, playingPiece);
                        }

                        PlayGame(board, game);

                        break;
                    case 2:
                        ViewResults();
                        break;
                    case 3:
                        board = Location.MakeBoard();
                        ContinuGame(board);
                        break;
                    case 4:
                        Console.WriteLine("Programma wordt gesloten");
                        ProgramRunning = false;
                        break;
                    default:
                        Console.WriteLine("Ongeldige invoer, probeer opnieuw");
                        Console.WriteLine();
                        break;
                }
            }





            int MainMenu()
            {
                try
                {
                    Console.WriteLine("Welkom bij Scotland Yard!");
                    Console.WriteLine("---------------------------");
                    Console.WriteLine("1. Nieuw spel");
                    Console.WriteLine("2. Resultaten bekijken");
                    Console.WriteLine("3. Spel hervatten");
                    Console.WriteLine("4. Afsluiten");
                    Console.WriteLine("Kies een optie: ");
                    int MainMenuChoice = int.Parse(Console.ReadLine() ?? string.Empty);
                    Console.Clear();
                    return MainMenuChoice;
                }
                catch (Exception)
                {
                    Console.Clear();
                    return 0;
                }

            }

            List<PlayingPiece> CreatePieces(int gameId)
            {
                List<PlayingPiece> playingPieces = new List<PlayingPiece>();

                Console.WriteLine("Met hoeveel agenten wil je spelen? (2 of 3)");
                int amountOfPlayers = int.Parse(Console.ReadLine() ?? string.Empty);
                while (amountOfPlayers != 2 && amountOfPlayers != 3)
                {
                    Console.WriteLine("Ongeldige invoer, probeer opnieuw");
                    Console.WriteLine("Met hoeveel agenten wil je spelen? (2 of 3)");
                    amountOfPlayers = int.Parse(Console.ReadLine() ?? string.Empty);

                }
                Console.Clear();
                
                for (int i = 1; i <= amountOfPlayers; i++)
                {
                    Console.WriteLine($"Geef het speelfiguur agent {i} een naam");
                    string agentName = Console.ReadLine() ?? string.Empty;
                    Console.WriteLine("--------------------------------------------");
                    int indexProtentialAgentPlayers = 1;
                    foreach (Player èxistingPlayer in Player.GetPlayers())
                    {
                        Console.WriteLine($"{indexProtentialAgentPlayers}. {èxistingPlayer.Name}");
                        indexProtentialAgentPlayers++;
                    }
                    Console.WriteLine($"Voer het nummer in van de speler die agent {i} speelt. Als je een nieuwe speler bent, voer -1 in.");
                    bool validInput = false;
                    int playerIndex = int.Parse(Console.ReadLine() ?? string.Empty);
                    Console.Clear();

                    Player player = null;
                    if (playerIndex == -1)
                    {
                        Console.WriteLine("Geef de naam van de nieuwe speler");
                        string playerName = Console.ReadLine() ?? string.Empty;
                        while (Player.GetPlayers().Any(existingEmployee => existingEmployee.Name == playerName))
                        {
                            Console.WriteLine("Een speler met deze naam bestaat al. Voer een andere naam in:");
                            playerName = Console.ReadLine() ?? string.Empty;
                        }
                        player = new Player(playerName);
                        player.DalCreatePlayer();
                        player = Player.GetPlayers().FirstOrDefault(p => p.Name == playerName);
                    }

                    else if (playerIndex > 0 && playerIndex <= Player.GetPlayers().Count)
                    {
                        player = Player.GetPlayers().ElementAt(playerIndex - 1);
                    }

                    Agent agent = new Agent(0, agentName, player);
                    agent.DalCreatePiece(gameId);
                    Console.Clear();
                    playingPieces.Add(agent);

                }



                Console.WriteLine("Geef Mister X een naam");
                string misterXName = Console.ReadLine() ?? string.Empty;
                int indexProtentialXPlayers = 1;
                foreach (Player èxistingPlayer in Player.GetPlayers())
                {
                    Console.WriteLine($"{indexProtentialXPlayers}. {èxistingPlayer.Name}");
                    indexProtentialXPlayers++;
                }
                Console.WriteLine($"Voer het nummer in van de speler die Mister X speelt. Als je een nieuwe speler bent, voer -1 in.");
                int xPlayerIndex = int.Parse(Console.ReadLine() ?? string.Empty);
                Console.Clear();

                Player xPlayer = null;
                if (xPlayerIndex == -1)
                {
                    Console.WriteLine("Geef de naam van de nieuwe speler");
                    string playerName = Console.ReadLine() ?? string.Empty;
                    while (Player.GetPlayers().Any(existingEmployee => existingEmployee.Name == playerName))
                    {
                        Console.WriteLine("Een speler met deze naam bestaat al. Voer een andere naam in:");
                        playerName = Console.ReadLine() ?? string.Empty;
                    }
                    xPlayer = new Player(playerName);
                    xPlayer.DalCreatePlayer();
                    xPlayer = Player.GetPlayers().FirstOrDefault(p => p.Name == playerName);
                }

                else if (xPlayerIndex > 0 && xPlayerIndex <= Player.GetPlayers().Count)
                {
                    xPlayer = Player.GetPlayers().ElementAt(xPlayerIndex - 1);
                }

                MisterX misterX = new MisterX(0, misterXName, xPlayer); 
                misterX.DalCreatePiece(gameId);
                playingPieces.Add(misterX);
                Console.Clear();

                return playingPieces;
            }

            void ChooseLocation(List<Location> availableLocations, PlayingPiece playingPiece)
            {
                Random random = new Random();
                int index1 = random.Next(availableLocations.Count);
                int index2 = random.Next(availableLocations.Count);
                while (index2 == index1)
                {
                    index2 = random.Next(availableLocations.Count);
                }

                Location location1 = availableLocations[index1];
                Location location2 = availableLocations[index2];

                Console.WriteLine($"Kies een locatie:");
                Console.WriteLine($"1. Vak {location1.Id}");
                Console.WriteLine($"2. Vak {location2.Id}");
                int choice = int.Parse(Console.ReadLine() ?? string.Empty);
                while (choice != 1 && choice != 2)
                {
                    Console.WriteLine("Ongeldige invoer, probeer opnieuw. Kies een locatie: 1 of 2");
                    choice = int.Parse(Console.ReadLine() ?? string.Empty);
                }
                if (choice == 1)
                {
                    availableLocations.Remove(location1);
                    playingPiece.GiveLocation(location1);
                    Console.Clear();
                }
                else
                {
                    availableLocations.Remove(location2);
                    playingPiece.GiveLocation(location2);
                    Console.Clear();
                }
            }
            void ViewResults()
            {
                double notFinnished = 0;
                double MisterXWon = 0;
                double AgentWon = 0;
                foreach (Game game in Game.GetGameStats())
                {
                    game.VieuwResult();
                    if (game.Winner == null)
                    {
                        notFinnished++;
                    }
                    else if (game.Winner is MisterX)
                    {
                        MisterXWon++;
                    }
                    else if (game.Winner is Agent)
                    {
                        AgentWon++;
                    }
                }
                
                try
                {
                    Console.WriteLine($"Aantal procent die nog niet zijn afgerond: {Math.Floor(notFinnished/ (notFinnished + MisterXWon + AgentWon) * 100)}%");
                    if (MisterXWon == 0)
                    {
                        Console.WriteLine("Er zijn nog geen spellen gewonnen door Mister X");
                    }
                    else
                    {
                        Console.WriteLine($"Aantal procent die Mister X heeft gewonnen: {Math.Floor(MisterXWon / (notFinnished + MisterXWon + AgentWon) * 100)}%");
                    }
                    if (AgentWon == 0)
                    {
                        Console.WriteLine("Er zijn nog geen spellen gewonnen door de agenten");
                    }
                    else
                    {
                        Console.WriteLine($"Aantal procent die de agenten hebben gewonnen: {Math.Floor(AgentWon/ (notFinnished + MisterXWon + AgentWon) * 100)}%");
                    }
                    Console.ReadLine();
                    Console.Clear();
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("Er zijn nog geen spellen te vinden in de database");
                    Console.WriteLine("De GetGames() moet nog geimplimenteerd worden. dus deze functie is nog niet helemaal af.");
                    Console.ReadLine();
                    Console.Clear();
                }

            }
            void PlayGame(List<Location> board, Game game)
            {
                Location? reaveledXLocation = null;
                bool misterXArrested = false;
                int i = 1;

                while (i < 11 && !misterXArrested)
                {
                    MisterX misterX = null;
                    Round round = new Round(i);
                    int roundreveal = round.RoundCount % 2;
                    Turn xTurn = null;

                    foreach (PlayingPiece playingPiece in game.PlayingPieces)
                    {
                        if (playingPiece is MisterX)
                        {
                            misterX = (MisterX)playingPiece;
                            xTurn = playingPiece.PlayTurn(board, round, playingPiece.Location);
                            round.AddTurn(xTurn);
                            misterXArrested = PlayingPiece.CheckCatch(game.PlayingPieces, game);
                            if (misterXArrested)
                            {
                                return;
                            }
                        }
                    }

                    foreach (PlayingPiece playingPiece in game.PlayingPieces)
                    {
                        if (playingPiece is Agent)
                        {
                            Turn turn = playingPiece.PlayTurn(board, round, playingPiece.Location, reaveledXLocation, xTurn, misterX);
                            round.AddTurn(turn);
                            misterXArrested = PlayingPiece.CheckCatch(game.PlayingPieces, game);
                            if (misterXArrested)
                            {
                                return;
                            }
                        }
                    }

                    Console.Clear();
                    if (i != 10)
                    {
                        Console.WriteLine("Do you wanna stop and safe the game?");
                        Console.WriteLine("1. No");
                        Console.WriteLine("2. Yes");
                        int safeGame = int.Parse(Console.ReadLine() ?? string.Empty);
                        if (safeGame == 2)
                        {
                            foreach (PlayingPiece playingPiece in game.PlayingPieces)
                            {
                                playingPiece.UpdatePlayingPiece();
                            }
                            round.DalCreateRound(game.Id);
                            xTurn.DalCreateTurn(round.Id);
                            Console.Clear();
                            Console.WriteLine("Game is opgeslagen");
                            Console.ReadLine();
                            return;
                        }
                    }

                    if (roundreveal == 0)
                    {
                        reaveledXLocation = game.PlayingPieces.FirstOrDefault(p => p is MisterX)?.Location;
                        Console.WriteLine($"Mister X bevind zich nu op {reaveledXLocation.Id}");
                        Console.ReadLine();
                    }

                    game.AddRound(round);
                    i++;
                }

                Console.Clear();
                Console.WriteLine("10 rondes zijn verstreken.");
                Console.WriteLine($"Mister X is ontsnatpt");
                Console.ReadLine();
                foreach (PlayingPiece playingPiece in game.PlayingPieces)
                {
                    if (playingPiece is MisterX)
                    {
                        game.DalGiveWinner(playingPiece);
                    }
                }

            }
            void ContinuGame(List<Location> board)
            {
                List <Game> gamesToContinu = Game.GetGamesToContinu();
                if (gamesToContinu.Count == 0)
                {
                    Console.WriteLine("Er zijn geen spellen om te hervatten");
                    Console.ReadLine();
                    Console.Clear();
                    return;
                }
                Console.WriteLine("Kies een spel om te hervatten:");
                List<int> showedGamesIds = new List<int>();
                int Index = 1;
                foreach (Game protentialGame in gamesToContinu)
                {
                    if (!showedGamesIds.Contains(protentialGame.Id))
                    {
                        showedGamesIds.Add(protentialGame.Id);
                        Console.WriteLine($"{Index}. Spel gestart op: {protentialGame.Date}, GameId {protentialGame.Id}");
                        Index++;
                    }
                }
                int gameChoise = int.Parse(Console.ReadLine() ?? string.Empty);
                while (gameChoise < 1 || gameChoise > gamesToContinu.Count)
                {
                    Console.WriteLine("Ongeldige invoer, probeer opnieuw");
                    gameChoise = int.Parse(Console.ReadLine() ?? string.Empty);
                }
                Console.Clear();
                Game game = gamesToContinu[gameChoise - 1];

                List<PlayingPiece> playingPieces = PlayingPiece.GetPlayingPiecesContinu(game.Id, board);
                game.GivePlayingPieces(playingPieces);
                Round round = Round.GetLastRound(game.Id);
                Turn xTurn = Turn.GetLastXTurn(round.Id);
                MisterX misterX = (MisterX)playingPieces.FirstOrDefault(p => p is MisterX);
                Location? reaveledXLocation = misterX.Location;
                bool misterXArrested = false;
                int i = round.RoundCount+1;

                while (i < 11 && !misterXArrested)
                {
                    misterX = null;
                    round = new Round(i);
                    int roundreveal = round.RoundCount % 2;
                    xTurn = null;

                    foreach (PlayingPiece playingPiece in game.PlayingPieces)
                    {
                        if (playingPiece is MisterX)
                        {
                            misterX = (MisterX)playingPiece;
                            xTurn = playingPiece.PlayTurn(board, round, playingPiece.Location);
                            round.AddTurn(xTurn);
                            misterXArrested = PlayingPiece.CheckCatch(game.PlayingPieces, game);
                            if (misterXArrested)
                            {
                                return;
                            }
                        }
                    }

                    foreach (PlayingPiece playingPiece in game.PlayingPieces)
                    {
                        if (playingPiece is Agent)
                        {
                            Turn turn = playingPiece.PlayTurn(board, round, playingPiece.Location, reaveledXLocation, xTurn, misterX);
                            round.AddTurn(turn);
                            misterXArrested = PlayingPiece.CheckCatch(game.PlayingPieces, game);
                            if (misterXArrested)
                            {
                                return;
                            }
                        }
                    }

                    Console.Clear();
                    if (i != 10)
                    {
                        Console.WriteLine("Do you wanna stop and safe the game?");
                        Console.WriteLine("1. No");
                        Console.WriteLine("2. Yes");
                        int safeGame = int.Parse(Console.ReadLine() ?? string.Empty);
                        if (safeGame == 2)
                        {
                            foreach (PlayingPiece playingPiece in game.PlayingPieces)
                            {
                                playingPiece.UpdatePlayingPiece();
                            }
                            round.DalCreateRound(game.Id);
                            xTurn.DalCreateTurn(round.Id);
                            Console.Clear();
                            Console.WriteLine("Game is opgeslagen");
                            Console.ReadLine();
                            return;
                        }
                    }

                    if (roundreveal == 0)
                    {
                        reaveledXLocation = game.PlayingPieces.FirstOrDefault(p => p is MisterX)?.Location;
                        Console.WriteLine($"Mister X bevind zich nu op {reaveledXLocation.Id}");
                        Console.ReadLine();
                    }

                    game.AddRound(round);
                    i++;
                }

                Console.Clear();
                Console.WriteLine("10 rondes zijn verstreken.");
                Console.WriteLine($"Mister X is ontsnatpt");
                Console.ReadLine();
                foreach (PlayingPiece playingPiece in game.PlayingPieces)
                {
                    if (playingPiece is MisterX)
                    {
                        game.DalGiveWinner(playingPiece);
                    }
                }


            }

        }

    }
}
