using System;
using System.Text.RegularExpressions;

namespace Model
{
    public class Team
    {
        public enum SetResuls
        {
            WRONG_ID = 1,
            WRONG_NAME,
            WRONG_SADIUM,
            WRONG_MANAGER,
            CORRECT
        }

        private class StringPatterns
        {
            // "-" считается диапазоном, если он посередине или не экранирован "\"
            public static string NamePattern = @"^[a-zA-Z0-9.\-#`,\ ]+$";
            public static string StadiumPattern = @"^[a-zA-Z0-9.\-,`\ ]+$";
            public static string ManagerPattern = @"^[a-zA-Z.\-`\ ]+$";
        }

        public int? Id { private set; get; }
        public string Name { private set; get; }
        public string Stadium { private set; get; }
        public string Manager { private set; get; }

        public Team() { }
        public Team(string name, string stad, string man, int id = 0)
        {
            if (isIdCorrect(id))        Id = id;
            if (isNameCorrect(name))    Name = name;
            if (isStadiumCorrect(stad)) Stadium = stad;
            if (isManagerCorrect(man))  Manager = man;
        }

        private bool isIdCorrect(int id)
        {
            if (id > 0 && id <= int.MaxValue) return true;
            return false;
        }
        private bool isNameCorrect(string str)
        {
            if (!Regex.IsMatch(str, StringPatterns.NamePattern) || str.Equals(String.Empty) || str.Length < 3 || str.Length > 50) return false;
            return true;
        }
        private bool isStadiumCorrect(string str)
        {
            if (str.Length == 0 || (Regex.IsMatch(str, StringPatterns.StadiumPattern) && !(str.Length > 50))) return true;
            return false;
        }
        private bool isManagerCorrect(string str)
        {
            if (str.Length == 0 || (Regex.IsMatch(str, StringPatterns.ManagerPattern) && !(str.Length > 50))) return true;
            return false;
        }

        public bool IsNull()
        {
            return (Id.HasValue && Name != null && Stadium != null && Manager != null) ? false : true;
        }
        public bool IsReadyToDB()
        {
            return (Name != null && Stadium != null && Manager != null) ? true : false;
        }
        public SetResuls GetWrongField()
        {
            if (IsNull())
            {
                if (!Id.HasValue) return SetResuls.WRONG_ID;
                if (Name == null) return SetResuls.WRONG_NAME;
                if (Stadium == null) return SetResuls.WRONG_SADIUM;
                if (Manager == null) return SetResuls.WRONG_MANAGER;
            }
            return SetResuls.CORRECT;
        }

        public bool SetName(string str)
        {
            if (isNameCorrect(str)) { Name = str; return true; }
            return false;
        }
        public bool SetStadium(string str)
        {
            if (isStadiumCorrect(str)) { Stadium = str; return true; }
            return false;
        }
        public bool SetManager(string str)
        {
            if (isManagerCorrect(str)) { Manager = str; return true; }
            return false;
        }
        public bool SetId(int id)
        {
            if (isIdCorrect(id)) { Id = id; return true; }
            else return false;
        }

    }
}
