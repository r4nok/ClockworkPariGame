using System;
using System.Collections.Generic;
using System.Text;

using System.Data.SqlClient;
using Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace DataLevel
{
    public class ClockworkDataBase
    {
        private static string ConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=Z:\Users\Sergey\Desktop\univ\Курсач\work\ClockworkPariGame\DataLevel\ClockworkDataBase.mdf;Integrated Security = True; Connect Timeout = 30";

        private SqlConnection sqlconnection;

        public ClockworkDataBase()
        {
            sqlconnection = new SqlConnection(ConnectionString);
        }

        private bool OpenConnection()
        {
            if (sqlconnection.State != System.Data.ConnectionState.Open)
            {
                sqlconnection.Open();
                return true;
            }
            return false;
        }
        private bool CloseConnection()
        {
            if (sqlconnection.State == System.Data.ConnectionState.Open)
            {
                sqlconnection.Close();
                return true;
            }
            return false;
        }

        private DataActionResults ExceptionHandler(SqlException ex)
        {
            // если в результате не добавили запись в БД
            CloseConnection();

            if (ex.Number == 2627) // Cannot insert duplicate key row in object error
            {
                // handle duplicate key error
                return DataActionResults.NOT_UNIQUE_EX;
            }

            return DataActionResults.GENERAL_EX;
        }

        public List<UserMessage> ReadUserMessages(int userId, int receiverId, out int error_rows)
        {
            error_rows = 0;

            List<UserMessage> messages = new List<UserMessage>();

            OpenConnection();

            string query = "SELECT * FROM UserMessages WHERE (Sender=" + userId + " AND Receiver=" + receiverId + ") OR (Sender=" + receiverId + " AND Receiver=" + userId + ");";

            SqlCommand command = new SqlCommand(query, sqlconnection);
            SqlDataReader result = command.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {

                    UserMessage message = new UserMessage(Convert.ToInt32(result["Sender"]), Convert.ToInt32(result["Receiver"]), 
                        Convert.ToString(result["Text"]), Convert.ToDateTime(result["Date"]), Convert.ToInt32(result["Id"]));

                    if (message.IsNull()) { error_rows++; continue; }

                    messages.Add(message);
                }
            }

            result.Close();
            CloseConnection();
            return messages;
        }
        public int InsertUserMessage(UserMessage message)
        {
            try
            {
                if (message.GetWrongField() != UserMessage.SetResuls.WRONG_ID) return -1; // поле уже добавлено в БД
                if (!message.IsReadyToDB()) return -1; // нужные поля не заполнены

                OpenConnection();

                string query = "INSERT INTO UserMessages (Sender, Receiver, Text, Date) output INSERTED.Id VALUES (@sender, @receiver, @text, @date)";
                SqlCommand command = new SqlCommand(query, sqlconnection);


                command.Parameters.AddWithValue("@sender",      message.Sender);
                command.Parameters.AddWithValue("@receiver",    message.Receiver);
                command.Parameters.AddWithValue("@text",        message.Text);
                command.Parameters.AddWithValue("@date",        message.Date);

                int messageId = (int)command.ExecuteScalar();

                CloseConnection();
                return messageId;
            }
            catch
            {
                // если в результате не добавили запись в БД
                CloseConnection();
                return (int)DataActionResults.GENERAL_EX;
            }
        }

        public List<Tournament> ReadTournaments(out int error_rows)
        {
            error_rows = 0;

            List<Tournament> tournaments = new List<Tournament>();

            OpenConnection();

            string query = "SELECT * FROM Tournaments;";

            SqlCommand command = new SqlCommand(query, sqlconnection);
            SqlDataReader result = command.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {

                    Tournament tournament = new Tournament(Convert.ToString(result["Name"]), Convert.ToString(result["Abbrev"]),
                        (Tournament.TournamentTypes)Convert.ToInt32(result["Type"]), Convert.ToInt32(result["Id"]));

                    if (tournament.IsNull()) { error_rows++; continue; }

                    tournaments.Add(tournament);
                }
            }

            result.Close();
            CloseConnection();
            return tournaments;
        }
        public DataActionResults InsertTournament(Tournament tournament)
        {
            try
            {
                if (tournament.GetWrongField() != Tournament.SetResuls.WRONG_ID) return DataActionResults.ALREADY_EXIST; // поле уже добавлено в БД
                if (!tournament.IsReadyToDB()) return DataActionResults.NOT_READY; // нужные поля не заполнены

                OpenConnection();

                string query = "INSERT INTO Tournaments (Name, Abbrev, Type) output INSERTED.Id VALUES (@name, @abbrev, @type)";
                SqlCommand command = new SqlCommand(query, sqlconnection);


                command.Parameters.AddWithValue("@name", tournament.Name);
                command.Parameters.AddWithValue("@abbrev", tournament.Abbreviation);
                command.Parameters.AddWithValue("@type", (int)tournament.TournamentType);

                int tournamentID = (int)command.ExecuteScalar();
                tournament.SetId(tournamentID);

                CloseConnection();
                return DataActionResults.ALL_RIGHT;
            }
            catch (SqlException ex)
            {
                return ExceptionHandler(ex);
            }
}
        public DataActionResults UpdateTournament(Tournament tournament)
        {
            try
            {
                OpenConnection();

                string query = "UPDATE Tournaments SET Name='" + tournament.Name + "', Abbrev='" + tournament.Abbreviation + "', Type='"
                    + (int)tournament.TournamentType + "' WHERE Id LIKE '" + tournament.Id + "' ;";

                SqlCommand command = new SqlCommand(query, sqlconnection);

                if (command.ExecuteNonQuery() != 1) return DataActionResults.MORE_THAN_1_ROW;

                CloseConnection();
                return DataActionResults.ALL_RIGHT;
            }
            catch (SqlException ex)
            {
                return ExceptionHandler(ex);
            }
        }
        private Tournament ReadTournament(int tournamentId)
        {
            OpenConnection();

            string query = "SELECT * FROM Tournaments WHERE Id = '" + tournamentId + "' ;";

            SqlCommand command = new SqlCommand(query, sqlconnection);
            SqlDataReader result = command.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {
                    Tournament tournament = new Tournament(Convert.ToString(result["Name"]), Convert.ToString(result["Abbrev"]),
                        (Tournament.TournamentTypes)Convert.ToInt32(result["Type"]), Convert.ToInt32(result["Id"]));

                    result.Close();
                    CloseConnection();

                    return tournament;
                }
            }

            result.Close();
            CloseConnection();
            return null;
        }

        public List<Team> ReadTeams(int tournamentId, out int error_rows)
        {
            error_rows = 0;

            List<Team> teams = new List<Team>();

            OpenConnection();

            string query = "SELECT Teams.Id, Teams.Name, Teams.Manager, Teams.Stadium" +
                " FROM Teams INNER JOIN TournamentsTeams ON Teams.Id = TournamentsTeams.TeamId WHERE TournamentsTeams.TournamentId = " + tournamentId  + " ;";

            SqlCommand command = new SqlCommand(query, sqlconnection);
            SqlDataReader result = command.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {

                    Team team = new Team(Convert.ToString(result["Name"]), Convert.ToString(result["Stadium"]),
                        Convert.ToString(result["Manager"]), Convert.ToInt32(result["Id"]));

                    if (team.IsNull()) { error_rows++; continue; }

                    teams.Add(team);
                }
            }

            result.Close();
            CloseConnection();
            return teams;
        }
        public DataActionResults InsertTeam(Team team)
        {
            try
            {
                if (team.GetWrongField() != Team.SetResuls.WRONG_ID) return DataActionResults.GENERAL_EX; // поле уже добавлено в БД
                if (!team.IsReadyToDB()) return DataActionResults.GENERAL_EX; // нужные поля не заполнены

                OpenConnection();

                string query = "INSERT INTO Teams (Name, Manager, Stadium) output INSERTED.Id VALUES (@name, @manager, @st)";
                SqlCommand command = new SqlCommand(query, sqlconnection);


                command.Parameters.AddWithValue("@name", team.Name);
                command.Parameters.AddWithValue("@manager", team.Manager);
                command.Parameters.AddWithValue("@st", team.Stadium);

                int teamID = (int)command.ExecuteScalar();
                team.SetId(teamID);

                CloseConnection();
                return DataActionResults.ALL_RIGHT;
            }
            catch (SqlException ex)
            {
                return ExceptionHandler(ex);
            }
        }
        public DataActionResults UpdateTeam(Team team)
        {
            try
            {
                OpenConnection();

                string query = "UPDATE Teams SET Name='" + team.Name + "', Manager='" + team.Manager + "', Stadium='"
                    + team.Stadium + "' WHERE Id LIKE '" + team.Id + "' ;";

                SqlCommand command = new SqlCommand(query, sqlconnection);

                if (command.ExecuteNonQuery() != 1) return DataActionResults.MORE_THAN_1_ROW;

                CloseConnection();
                return DataActionResults.ALL_RIGHT;
            }
            catch (SqlException ex)
            {
                return ExceptionHandler(ex);
            }
        }
        public DataActionResults ConnectTournamentTeam(Tournament tournament, Team team)
        {
            try
            {
                if (tournament.GetWrongField() != Tournament.SetResuls.CORRECT) return DataActionResults.GENERAL_EX;
                if (team.GetWrongField() != Team.SetResuls.CORRECT) return DataActionResults.GENERAL_EX;

                OpenConnection();

                string query = "INSERT INTO TournamentsTeams (TournamentId, TeamId) VALUES (@tournamentId, @teamId)";
                SqlCommand command = new SqlCommand(query, sqlconnection);


                command.Parameters.AddWithValue("@tournamentId", tournament.Id);
                command.Parameters.AddWithValue("@teamId", team.Id);

                command.ExecuteNonQuery();

                CloseConnection();
                return DataActionResults.ALL_RIGHT;
            }
            catch (SqlException ex)
            {
                return ExceptionHandler(ex);
            }
        }
        public DataActionResults RemoveTeam(Tournament tournament, Team team)
        {
            try
            {
                OpenConnection();

                string query = "DELETE FROM TournamentsTeams WHERE TournamentId='" + tournament.Id + "' AND TeamId='" + team.Id + "';";

                SqlCommand command = new SqlCommand(query, sqlconnection);

                if (command.ExecuteNonQuery() != 1) return DataActionResults.MORE_THAN_1_ROW;

                CloseConnection();
                return DataActionResults.ALL_RIGHT;
            }
            catch
            {
                CloseConnection();
                return DataActionResults.GENERAL_EX;
            }
        }
        public Team ReadTeam(string teamName)
        {
            OpenConnection();

            string query = "SELECT * FROM Teams WHERE Name = '" + teamName + "' ;";

            SqlCommand command = new SqlCommand(query, sqlconnection);
            SqlDataReader result = command.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {
                    Team team = new Team(Convert.ToString(result["Name"]), Convert.ToString(result["Stadium"]),
                        Convert.ToString(result["Manager"]), Convert.ToInt32(result["Id"]));

                    result.Close();
                    CloseConnection();

                    return team.IsNull() ? null : team;                    
                }
            }

            result.Close();
            CloseConnection();
            return null;
        }
        public Team ReadTeam(int teamId)
        {
            OpenConnection();

            string query = "SELECT * FROM Teams WHERE Id = '" + teamId + "' ;";

            SqlCommand command = new SqlCommand(query, sqlconnection);
            SqlDataReader result = command.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {
                    Team team = new Team(Convert.ToString(result["Name"]), Convert.ToString(result["Stadium"]),
                        Convert.ToString(result["Manager"]), Convert.ToInt32(result["Id"]));

                    result.Close();
                    CloseConnection();

                    return team.IsNull() ? null : team;
                }
            }

            result.Close();
            CloseConnection();
            return null;
        }

        public DataActionResults InsertMatch(Match match, Tournament tournament)
        {
            try
            {
                if (match.Home.GetWrongField() != Team.SetResuls.CORRECT) return DataActionResults.GENERAL_EX;
                if (match.Away.GetWrongField() != Team.SetResuls.CORRECT) return DataActionResults.GENERAL_EX;

                if (!IsTeamInTournament((int)match.Home.Id, (int)tournament.Id) || !IsTeamInTournament((int)match.Away.Id, (int)tournament.Id)) return DataActionResults.NOT_IN_TOURNAMENT;

                OpenConnection();

                string query = "INSERT INTO Matches (Home, Away, TournamentId, Type) output INSERTED.Id VALUES (@home, @away, @tourId, @type)";
                SqlCommand command = new SqlCommand(query, sqlconnection);


                command.Parameters.AddWithValue("@home", match.Home.Id);
                command.Parameters.AddWithValue("@away", match.Away.Id);
                command.Parameters.AddWithValue("@tourId", tournament.Id);
                command.Parameters.AddWithValue("@type", Match.MatchType.Upcoming);

                match.SetId((int)command.ExecuteScalar());

                CloseConnection();
                return DataActionResults.ALL_RIGHT;
            }
            catch (SqlException ex)
            {
                return ExceptionHandler(ex);
            }
        }
        public ObservableCollection<Match> ReadMatches()
        {
            ObservableCollection<Match> matches = new ObservableCollection<Match>();

            OpenConnection();

            string query = "SELECT * FROM Matches;";

            SqlCommand command = new SqlCommand(query, sqlconnection);
            SqlDataReader result = command.ExecuteReader();

            List<List<int>> matchTeamsTour = new List<List<int>>();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    List<int> ttt = new List<int>();

                    ttt.Add(Convert.ToInt32(result["Home"]));
                    ttt.Add(Convert.ToInt32(result["Away"]));
                    ttt.Add(Convert.ToInt32(result["TournamentId"]));
                    ttt.Add(Convert.ToInt32(result["Id"]));

                    ttt.Add(Convert.ToInt32(result["Type"]));
                    ttt.Add(Convert.ToInt32(Convert.ToBoolean(result["Visibility"])));

                    matchTeamsTour.Add(ttt);
                }
            }

            result.Close();

            foreach (var el in matchTeamsTour)
            {
                Team home = ReadTeam(el[0]);
                Team away = ReadTeam(el[1]);
                Tournament tournament = ReadTournament(el[2]);
                bool visibility = el[5] == 1 ? true : false;

                if (home != null && away != null && tournament != null)
                {
                    matches.Add(new Match(home, away, tournament, ReadMatchEvents(el[3], true), ReadMatchEvents(el[3], false), el[3], (Match.MatchType)el[4], visibility));
                }
                ReadBets(matches.Last());
            }

            CloseConnection();
            return matches;
        }
        public DataActionResults UpdateMatch(Match match, Match.MatchType matchType)
        {
            try
            {
                if (match.Id == null) return DataActionResults.GENERAL_EX;

                OpenConnection();

                string query = "UPDATE Matches SET Type='" + (int)matchType + "' WHERE Id LIKE '" + match.Id + "';";

                SqlCommand command = new SqlCommand(query, sqlconnection);

                if (command.ExecuteNonQuery() != 1) return DataActionResults.MORE_THAN_1_ROW;

                CloseConnection();
                return DataActionResults.ALL_RIGHT;
            }
            catch (SqlException ex)
            {
                return ExceptionHandler(ex);
            }
        }
        public DataActionResults UpdateMatch(Match match, bool isVisible)
        {
            try
            {
                if (match.Id == null) return DataActionResults.GENERAL_EX;

                OpenConnection();

                string query = "UPDATE Matches SET Visibility='" + isVisible + "' WHERE Id LIKE '" + match.Id + "';";

                SqlCommand command = new SqlCommand(query, sqlconnection);

                if (command.ExecuteNonQuery() != 1) return DataActionResults.MORE_THAN_1_ROW;

                CloseConnection();
                return DataActionResults.ALL_RIGHT;
            }
            catch (SqlException ex)
            {
                return ExceptionHandler(ex);
            }
        }
        public DataActionResults RemoveMatch(int? matchId)
        {
            try
            {
                if (matchId == null) return DataActionResults.GENERAL_EX;

                OpenConnection();

                string query = "DELETE FROM Matches WHERE Matches.Id='" + matchId + "';";

                SqlCommand command = new SqlCommand(query, sqlconnection);

                if (command.ExecuteNonQuery() != 1) return DataActionResults.MORE_THAN_1_ROW;

                CloseConnection();
                return DataActionResults.ALL_RIGHT;
            }
            catch
            {
                CloseConnection();
                return DataActionResults.GENERAL_EX;
            }
        }

        public ObservableCollection<Events> ReadMatchEvents(int matchId, bool isHome)
        {
            ObservableCollection<Events> eventsList = new ObservableCollection<Events>();
            OpenConnection();

            string query = "SELECT * FROM MatchEvents WHERE MatchId='" + matchId +"' AND IsHome='" + isHome + "';";

            SqlCommand command = new SqlCommand(query, sqlconnection);
            SqlDataReader result = command.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Events matchEvent = new Events((Events.EventTypes)Convert.ToInt32(result["TypeId"]),
                        Convert.ToInt32(result["Minute"]), Convert.ToInt32(result["Id"]));
                    eventsList.Add(matchEvent);
                }
            }

            result.Close();
            CloseConnection();
            return eventsList;
        }
        public DataActionResults InsertMatchEvent(Events matchEvent, int matchId, bool IsHome)
        {
            try
            {
                OpenConnection();

                string query = "INSERT INTO MatchEvents (MatchId, TypeId, Minute, IsHome) output INSERTED.Id VALUES (@match, @type, @minute, @ishome)";
                SqlCommand command = new SqlCommand(query, sqlconnection);


                command.Parameters.AddWithValue("@match", matchId);
                command.Parameters.AddWithValue("@type", (int)matchEvent.Type);
                command.Parameters.AddWithValue("@minute", matchEvent.Minute);
                command.Parameters.AddWithValue("@ishome", IsHome);

                matchEvent.SetId((int)command.ExecuteScalar());

                CloseConnection();
                return DataActionResults.ALL_RIGHT;
            }
            catch (SqlException ex)
            {
                return ExceptionHandler(ex);
            }
        }
        public DataActionResults RemoveMatchEvent(int? eventId)
        {
            try
            {
                if (eventId == null) return DataActionResults.GENERAL_EX;

                OpenConnection();

                string query = "DELETE FROM MatchEvents WHERE Id='" + eventId + "';";

                SqlCommand command = new SqlCommand(query, sqlconnection);

                if (command.ExecuteNonQuery() != 1) return DataActionResults.MORE_THAN_1_ROW;

                CloseConnection();
                return DataActionResults.ALL_RIGHT;
            }
            catch
            {
                CloseConnection();
                return DataActionResults.GENERAL_EX;
            }
        }
        public DataActionResults UpdateMatchEvent(int? eventId, int minute)
        {
            try
            {
                if (eventId == null) return DataActionResults.GENERAL_EX;

                OpenConnection();

                string query = "UPDATE MatchEvents SET Minute='" + minute + "' WHERE Id LIKE '" + eventId + "';";

                SqlCommand command = new SqlCommand(query, sqlconnection);

                if (command.ExecuteNonQuery() != 1) return DataActionResults.MORE_THAN_1_ROW;

                CloseConnection();
                return DataActionResults.ALL_RIGHT;
            }
            catch (SqlException ex)
            {
                return ExceptionHandler(ex);
            }
        }

        public void ReadBets(Match match)
        {
            BetsList betsList = new BetsList();
            OpenConnection();

            string query = "SELECT * FROM Bets WHERE MatchId='" + match.Id + "';";

            SqlCommand command = new SqlCommand(query, sqlconnection);
            SqlDataReader result = command.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    betsList.AddBet((BetBase.BetTypes)Convert.ToInt32(result["Type"]), Math.Round(Convert.ToDouble(result["Odd"]),2), match,
                        Convert.ToString(result["Description"]), Math.Round(Convert.ToDouble(result["Grade"]), 2));
                    betsList.Bets.Last().SetId(Convert.ToInt32(result["Id"]));

                    Bet.OutcomeType betOutcome = (Bet.OutcomeType)Convert.ToInt32(result["Outcome"]);
                    if (betOutcome == Bet.OutcomeType.Win) { betsList.Bets.Last().SetWin(); continue; }
                    if (betOutcome == Bet.OutcomeType.Refund) { betsList.Bets.Last().SetRefund(); continue; }
                    if (betOutcome == Bet.OutcomeType.Lost) { betsList.Bets.Last().SetLost(); continue; }
                }
            }

            match.BetList = betsList;

            result.Close();
            CloseConnection();
        }
        public DataActionResults InsertBet(Bet bet, Match match)
        {
            try
            {
                if (match == null || bet == null || match.IsNull()) return DataActionResults.GENERAL_EX;

                OpenConnection();

                string query = "INSERT INTO Bets (MatchId, Type, Description, Odd, Outcome, Grade) output INSERTED.Id VALUES (@match, @type, @descr, @odd, @outcome, @grade)";
                SqlCommand command = new SqlCommand(query, sqlconnection);

                command.Parameters.AddWithValue("@match", match.Id);
                command.Parameters.AddWithValue("@type", (int)bet.BetType);
                command.Parameters.AddWithValue("@descr", bet.Description);
                command.Parameters.AddWithValue("@odd", bet.Odd);
                command.Parameters.AddWithValue("@outcome", (int)Bet.OutcomeType.Sleep);
                command.Parameters.AddWithValue("@grade", bet.Goals);

                bet.SetId((int)command.ExecuteScalar());

                CloseConnection();
                return DataActionResults.ALL_RIGHT;
            }
            catch (SqlException ex)
            {
                return ExceptionHandler(ex);
            }
        }
        public DataActionResults RemoveBet(int betId)
        {
            try
            {
                OpenConnection();

                string query = "DELETE FROM Bets WHERE Id='" + betId + "';";

                SqlCommand command = new SqlCommand(query, sqlconnection);

                if (command.ExecuteNonQuery() != 1) return DataActionResults.MORE_THAN_1_ROW;

                CloseConnection();
                return DataActionResults.ALL_RIGHT;
            }
            catch
            {
                CloseConnection();
                return DataActionResults.GENERAL_EX;
            }
        }

        public int GetUserId(string nick)
        {
            int id = -1;
            OpenConnection();

            string query = "SELECT Id FROM Users WHERE Nickname LIKE '" + nick + "';";

            SqlCommand command = new SqlCommand(query, sqlconnection);
            SqlDataReader result = command.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {

                    id = Convert.ToInt32(result["Id"]);
                }
            }

            result.Close();
            CloseConnection();
            return id;
        }

        private bool IsTeamInTournament(int TeamId, int TournamentId)
        {
            try
            {
                OpenConnection();

                string query = "SELECT * FROM TournamentsTeams WHERE TournamentId ='" + TournamentId + "' AND TeamId='" + TeamId + "';";

                SqlCommand command = new SqlCommand(query, sqlconnection);
                SqlDataReader result = command.ExecuteReader();

                if (result.HasRows) { CloseConnection(); return true;} else { CloseConnection(); return false; }
            }
            catch (SqlException ex)
            {
                CloseConnection(); return false;
            }
        }
    }
}
