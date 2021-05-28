using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Model
{
    public class User
    {
        public enum UserTypes
        {
            BOOKMAKER = 1,
            BETTOR = 2
        }
        public enum SetResuls
        {
            WRONG_ID = 1,
            WRONG_NICK,
            WRONG_PASS,
            CORRECT
        }

        private class StringPatterns
        {
            public static string NicknamePattern = @"^[a-zA-Z0-9@_]+$";
            public static string PasswordPattern = @"^[a-zA-Z0-9_]+$";

            public static string OneLetter = @"[a-zA-Z]";
            public static string OneDigit = @"\d";
        }

        public int? Id { private set; get; }
        public string Nickname { private set; get; }
        public string Password { private set; get; }
        public UserTypes UserType { get; }
        public uint Coins { private set; get; }

        public User(int id, string nick, string pass, UserTypes userType, uint coins)
        {
            if (isIdCorrect(id)) Id = id;
            if (isNicknameCorrect(nick)) Nickname = nick;
            if (isPasswordCorrect(pass)) Password = pass;
            UserType = userType;
            Coins = 0;
        }

        private bool isIdCorrect(int id)
        {
            if (id > 0 && id <= int.MaxValue) return true;
            return false;
        }
        private bool isNicknameCorrect(string str)
        {
            if (str.Length < 4 || str.Length > 21) return false;
            if (!Regex.IsMatch(str, StringPatterns.NicknamePattern)) return false;
            return true;
        }
        private bool isPasswordCorrect(string str)
        {
            if (str.Length < 4 || str.Length > 21) return false;
            if (!Regex.IsMatch(str, StringPatterns.PasswordPattern)) return false;
            if (!Regex.IsMatch(str, StringPatterns.OneLetter, RegexOptions.IgnoreCase)) return false;
            if (!Regex.IsMatch(str, StringPatterns.OneDigit, RegexOptions.IgnoreCase)) return false;
            return true;
        }

        public bool IsNull()
        {
            return (Id.HasValue && Nickname != null && Password != null) ? false : true;
        }
        public SetResuls GetWrongField()
        {
            if (IsNull())
            {
                if (!Id.HasValue) return SetResuls.WRONG_ID;
                if (Nickname == null) return SetResuls.WRONG_NICK;
                if (Password == null) return SetResuls.WRONG_PASS;
            }
            return SetResuls.CORRECT;
        }

        public bool SetNickname(string str)
        {
            if (isNicknameCorrect(str)) { Nickname = str; return true; }
            return false;
        }
        public bool SetPassword(string str)
        {
            if (isPasswordCorrect(str)) { Password = str; return true; }
            return false;
        }

        public bool IsBettor()
        {
            return UserType == UserTypes.BETTOR;
        }
        public bool IsBookmaker()
        {
            return UserType == UserTypes.BOOKMAKER;
        }

        public bool SetCoins(uint coins)
        {
            if (IsBookmaker()) { Coins = coins; return true; }
            return false;
        }
    }
}
