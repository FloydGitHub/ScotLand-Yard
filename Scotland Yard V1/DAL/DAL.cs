using Scotland_Yard_V1.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Scotland_Yard_V1.DAL
{
    internal class DAL
    {
        string connectionString = "Data Source=FLOYDSCHOOL; Initial Catalog=ScotlandyardV1; Integrated Security=True";
        public List<Player> players { get; set; } = new List<Player>();
        public List<PlayingPiece> playingPieces { get; set; } = new List<PlayingPiece>();
        public List<Game> games { get; set; } = new List<Game>();
        public List<Round> rounds { get; set; } = new List<Round>();
        public List<Turn> turns { get; set; } = new List<Turn>();

        //Get
        public void DataRetrievalPlayers()
        {
            players.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "Select * from Player";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Customer ophalen uit database
                                int id = reader.GetInt32(0);
                                string Name = reader.GetString(1);

                                Player player = new Player(id, Name);
                                players.Add(player);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Er is een fout opgedreden met het ophalen van de klanten uit de database + {ex.Message}");
                        Console.ReadLine();
                    }
                }
            }
        }

        public void DataRetrievalGamesStats()
        {
            try
            {
                DataRetrievalPlayingPiecesStats();
                games.Clear();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "Select * from Game";
                    using (SqlDataReader reader = new SqlCommand(query, connection).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            int? winnerId = reader.IsDBNull(1) ? null : (int?)reader.GetInt32(1);
                            String date = reader.GetString(2);

                            PlayingPiece winner = playingPieces.FirstOrDefault(p => p.Id == winnerId);
                            Game game = new Game(id, winner, date);
                            games.Add(game);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Er is een fout opgetreden met het ophalen van de games uit de database: {ex.Message}");
                Console.ReadLine();
            }
        }


        public void DataRetrievalPlayingPiecesStats()
        {
            try
            {
                playingPieces.Clear();
                DataRetrievalPlayers();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "Select * from PlayingPiece";
                    using (SqlDataReader reader = new SqlCommand(query, connection).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string? name = reader.GetString(1);
                            int playerId = reader.GetInt32(5);
                            string type = reader.GetString(7);

                            Player player = players.FirstOrDefault(p => p.Id == playerId);

                            if (type == "Agent")
                            {
                                PlayingPiece playingPiece = new Agent(id, name, player);
                                playingPieces.Add(playingPiece);
                            }
                            else if (type == "MisterX")
                            {
                                PlayingPiece playingPiece = new MisterX(id, name, player);
                                playingPieces.Add(playingPiece);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Er is een fout opgetreden met het ophalen van de playing pieces uit de database: {ex.Message}");
                Console.ReadLine();
            }
        }



        public void DataRetrievalGamesToContinu()
        {
            try
            {
                games.Clear();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
            SELECT Game.id, Game.date
            FROM Game
            INNER JOIN Round ON Game.id = Round.gameId
            WHERE Game.WinnerId IS NULL;
        ";
                    using (SqlDataReader reader = new SqlCommand(query, connection).ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string date = reader.GetString(1);
                            Game game = new Game(id, null, date);
                            games.Add(game);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Er is een fout opgetreden met het ophalen van de games uit de database: {ex.Message}");
                Console.ReadLine();
            }
        }
        public void DataRetrievalPlayingPiecesContinu(int gameId, List<Location> board)
        {
            try
            {
                playingPieces.Clear();
                DataRetrievalPlayers();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * From PlayingPiece where GameId = @gameId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@gameId", gameId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string name = reader.GetString(1);
                                int taxiTicketAmount = reader.GetInt32(2);
                                int busTicketAmount = reader.GetInt32(3);
                                int playerId = reader.GetInt32(5);
                                int? locationId = reader.IsDBNull(6) ? null : (int?)reader.GetInt32(6);
                                string type = reader.GetString(7);

                                Player player = players.FirstOrDefault(p => p.Id == playerId);
                                Location location = board.FirstOrDefault(l => l.Id == locationId);

                                if (type == "Agent")
                                {
                                    PlayingPiece playingPiece = new Agent(id, name, location, player, taxiTicketAmount, busTicketAmount);
                                    playingPieces.Add(playingPiece);
                                }
                                else if (type == "MisterX")
                                {
                                    PlayingPiece playingPiece = new MisterX(id, name, location, player, taxiTicketAmount, busTicketAmount);
                                    playingPieces.Add(playingPiece);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Er is een fout opgetreden met het ophalen van de playing pieces uit de database: {ex.Message}");
                Console.ReadLine();
            }
        }

        public void DataRetrievalRounds(int gameId)
        {
            try
            {
                rounds.Clear();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * From Round where GameId = @gameId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@gameId", gameId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                int roundCount = reader.GetInt32(1);

                                Round round = new Round(id, roundCount);
                                rounds.Add(round);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Er is een fout opgetreden met het ophalen van de rounds uit de database: {ex.Message}");
                Console.ReadLine();
            }
        }

        public void DataRetrievalTurns(int id)
        {
            try 
            {
                turns.Clear();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * From Turn where RoundId = @roundId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@roundId", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int turnId = reader.GetInt32(0);
                                string vehicle = reader.GetString(2);

                                Turn turn = new Turn(turnId, vehicle);
                                turns.Add(turn);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Er is een fout opgetreden met het ophalen van de turns uit de database: {ex.Message}");
                Console.ReadLine();
            }
        }








        //Create
        public void CreatePlayer(Player player)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Player(Name) " +
                       "VALUES (@Name)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", player.Name);

                        command.ExecuteNonQuery();
                    }
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Er is een fout opgedreden met het toevoegen van de player aan de database + {ex.Message}");
                Console.ReadLine();
            }

        }
        public int CreateGame(Game game) 
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Game(WinnerId, Date) VALUES (@WinnerId, @Date); SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@WinnerId", DBNull.Value);
                        command.Parameters.AddWithValue("@Date", game.Date);
                        int gameId = Convert.ToInt32(command.ExecuteScalar());
                        return gameId;
                    }
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Er is een fout opgedreden met het toevoegen van de game aan de database + {ex.Message}");
                Console.ReadLine();
                return 0;
            }
        }

        public int CreateAgent(PlayingPiece playingPiece, int gameId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO PlayingPiece(PieceName, TaxiTicketAmount, BusTicketAmount, GameId, PlayerId, LocationId, Type) " +
                        "VALUES (@PieceName, @TaxiTicketAmount, @BusTicketAmount, @GameId, @PlayerId, @LocationId, @Type) SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PieceName", playingPiece.Name);
                        command.Parameters.AddWithValue("@TaxiTicketAmount", playingPiece.TaxiTicketAmount);
                        command.Parameters.AddWithValue("@BusTicketAmount", playingPiece.BusTicketAmount);
                        command.Parameters.AddWithValue("@GameId", gameId);
                        command.Parameters.AddWithValue("@PlayerId", playingPiece.Player.Id);
                        command.Parameters.AddWithValue("@LocationId", DBNull.Value);
                        command.Parameters.AddWithValue("@Type", "Agent");

                        int newId = Convert.ToInt32(command.ExecuteScalar());
                        return newId;
                    }
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine($"Er is een fout opgedreden met het toevoegen van de playingpiece aan de database + {ex.Message}");
                Console.ReadLine();
                return 0;
            }
        }

        public int CreateMisterX(PlayingPiece playingPiece, int gameId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO PlayingPiece(PieceName, TaxiTicketAmount, BusTicketAmount, GameId, PlayerId, LocationId, Type) " +
                                   "VALUES (@PieceName, @TaxiTicketAmount, @BusTicketAmount, @GameId, @PlayerId, @LocationId, @Type) SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PieceName", playingPiece.Name);
                        command.Parameters.AddWithValue("@TaxiTicketAmount", playingPiece.TaxiTicketAmount);
                        command.Parameters.AddWithValue("@BusTicketAmount", playingPiece.BusTicketAmount);
                        command.Parameters.AddWithValue("@GameId", gameId);
                        command.Parameters.AddWithValue("@PlayerId", playingPiece.Player.Id);
                        command.Parameters.AddWithValue("@LocationId", DBNull.Value);
                        command.Parameters.AddWithValue("@Type", "MisterX");

                        int newId = Convert.ToInt32(command.ExecuteScalar());
                        return newId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Er is een fout opgetreden met het toevoegen van de playing piece aan de database: {ex.Message}");
                Console.ReadLine();
                return 0;
            }
        }

        public int CreateRound(Round round, int gameId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Round(RoundCount, GameId) " +
                        "VALUES (@RoundCount, @GameId) SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RoundCount", round.RoundCount);
                        command.Parameters.AddWithValue("@GameId", gameId);

                        int newId = Convert.ToInt32(command.ExecuteScalar());
                        return newId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Er is een fout opgetreden met het toevoegen van de round aan de database: {ex.Message}");
                Console.ReadLine();
                return 0;
            }
        }

        public int CreateTurn(Turn turn, int roundId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Turn(PlayingPieceId, Vehicle, RoundId) " +
                        "VALUES (@PlayingPieceId, @Vehicle, @RoundId) SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PlayingPieceId", turn.PlayingPiece.Id);
                        command.Parameters.AddWithValue("@Vehicle", turn.Vehicle);
                        command.Parameters.AddWithValue("@RoundId", roundId);

                        int newId = Convert.ToInt32(command.ExecuteScalar());
                        return newId;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Er is een fout opgetreden met het toevoegen van de turn aan de database: {ex.Message}");
                Console.ReadLine();
                return 0;
            }
        }

        // CRUd: Update

        public void UpdateGame(Game game)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Game SET WinnerId = @WinnerId WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@WinnerId", game.Winner.Id);
                        command.Parameters.AddWithValue("@Id", game.Id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Er is een fout opgetreden met het updaten van de game in de database: {ex.Message}");
                Console.ReadLine();
            }
        }

        public void UpdatePlayingPiece(PlayingPiece playingPiece)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE PlayingPiece SET LocationId = @LocationId, TaxiTicketAmount = @TaxiTicketAmount, BusTicketAmount = @BusTicketAmount WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LocationId", playingPiece.Location.Id);
                        command.Parameters.AddWithValue("@TaxiTicketAmount", playingPiece.TaxiTicketAmount);
                        command.Parameters.AddWithValue("@BusTicketAmount", playingPiece.BusTicketAmount);
                        command.Parameters.AddWithValue("@Id", playingPiece.Id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Er is een fout opgetreden met het updaten van de playing piece in de database: {ex.Message}");
                Console.ReadLine();
            }
        }

    }
}
