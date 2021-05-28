using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Model
{
    public class Tournament
    {
        public enum SetResuls
        {
            WRONG_ID = 1,
            WRONG_NAME,
            WRONG_ABBREV,
            CORRECT
        }
        public enum TournamentTypes
        {
            League = 1,
            PlayOff1,
            PlayOff2,
            None
        }

        private static string NamePattern = @"^[a-zA-Z0-9.\\-`,\ /]+$";
        private static string AbbreviationPattern = @"^[a-zA-Z.]+$";
        private static string AbbreviationNull = "LEAG. N";

        public int? Id { private set; get; }
        // unique name
        public string Name { private set; get; }
        public string Abbreviation { private set; get; }
        public TournamentTypes TournamentType { private set; get; }

        public Tournament() { }
        public Tournament(string name, TournamentTypes tournamentType, int id = 0)
        {
            if (isIdCorrect(id)) Id = id;
            if (isNameCorrect(name))
            {
                Name = name;
                Abbreviation = Name.Substring(0, 3).ToUpper(); if (!isAbbreviationCorrect(Abbreviation)) Abbreviation = AbbreviationNull;
            }
            TournamentType = tournamentType;
        }
        public Tournament(string name, string abbreviation, TournamentTypes tournamentType, int id = 0)
        {
            if (isIdCorrect(id)) Id = id;
            if (isNameCorrect(name)) Name = name;
            if (isAbbreviationCorrect(abbreviation)) Abbreviation = abbreviation;
            TournamentType = tournamentType;
        }

        private bool isIdCorrect(int id)
        {
            if (id > 0 && id <= int.MaxValue) return true;
            return false;
        }
        private bool isNameCorrect(string str)
        {
            if (!Regex.IsMatch(str, NamePattern) || str.Equals(String.Empty) || str.Length < 4 || str.Length > 40) return false;
            return true;
        }
        private bool isAbbreviationCorrect(string str)
        {
            if (!Regex.IsMatch(str, AbbreviationPattern) || str.Equals(String.Empty) || str.Length > 7) return false;
            return true;
        }

        public bool IsNull()
        {
            return (Id.HasValue && Name != null && Abbreviation != null) ? false : true;
        }
        public bool IsReadyToDB()
        {
            return (Name != null && Abbreviation != null && TournamentType != TournamentTypes.None) ? true : false;
        }
        public SetResuls GetWrongField()
        {
            if (IsNull())
            {
                if (!Id.HasValue) return SetResuls.WRONG_ID;
                if (Name == null) return SetResuls.WRONG_NAME;
                if (Abbreviation == null) return SetResuls.WRONG_ABBREV;
            }
            return SetResuls.CORRECT;
        }

        public bool SetName(string str)
        {
            if (isNameCorrect(str)) { Name = str; return true; }
            return false;
        }
        public bool SetAbbreviation(string str)
        {
            if (isAbbreviationCorrect(str)) { Abbreviation = str; return true; }
            return false;
        }
        /// <summary>
        /// Convert string into tournament type
        /// </summary>
        /// <param name="str">Only: league, playoff1, playoff2</param>
        /// <returns></returns>
        public bool SetType(string str)
        {
            if (String.Equals("league", str, StringComparison.OrdinalIgnoreCase)) { TournamentType = TournamentTypes.League; return true; }
            if (String.Equals("playoff1", str, StringComparison.OrdinalIgnoreCase)) { TournamentType = TournamentTypes.PlayOff1; return true; }
            if (String.Equals("playoff2", str, StringComparison.OrdinalIgnoreCase)) { TournamentType = TournamentTypes.PlayOff2; return true; }
            return false;
        }
        public bool SetId(int id)
        {
            if (isIdCorrect(id)) { Id = id; return true; }
            else return false;
        }
    }
}
