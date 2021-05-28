SELECT Teams.Id, Teams.Name, Teams.Manager, Teams.Stadium
FROM Teams
INNER JOIN TournamentsTeams ON Teams.Id = TournamentsTeams.TeamId
WHERE TournamentsTeams.TournamentId = 10;