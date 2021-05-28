using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model
{
    /// <summary>
    /// Parameters can not be chagenged In the Parlance of Our Times
    /// </summary>
    public class BetBase
    {
        public enum BetTypes
        {
            [Description("Custom")]
            CUSTOM = 1,
            [Description("Home win")]
            W1,
            [Description("Draw")]
            X,
            [Description("Away win")]
            W2,
            [Description("Home not lose")]
            W1X,
            [Description("Away not lose")]
            W2X,
            [Description("Total more")]
            TOTAL_MORE,
            [Description("Total less")]
            TOTAL_LESS,
            [Description("Total home more")]
            TOTAL1_MORE,
            [Description("Total home less")]
            TOTAL1_LESS,
            [Description("Total away more")]
            TOTAL2_MORE,
            [Description("Total away less")]
            TOTAL2_LESS,
            [Description("Total 1 half more")]
            TOTAL_FIRTS_HALF_MORE,
            [Description("Total 1 half less")]
            TOTAL_FIRTS_HALF_LESS,
            [Description("Total 2 half more")]
            TOTAL_SECOND_HALF_MORE,
            [Description("Total 1 half less")]
            TOTAL_SECOND_HALF_LESS
        }

        private static Dictionary<BetTypes, string> betTypesDictionary = new Dictionary<BetTypes, string>
        {
            { BetTypes.W1, "{0} win" },
            { BetTypes.X, "Draw" },
            { BetTypes.W2, "{0} win" },
            { BetTypes.W1X, "{0} not lose" },
            { BetTypes.W2X, "{0} not lose" },
            { BetTypes.TOTAL_MORE, "General total {0} MORE" }, { BetTypes.TOTAL_LESS, "General total {0} LESS" },
            { BetTypes.TOTAL1_MORE, "Individual total {0} - {1} MORE" }, { BetTypes.TOTAL1_LESS, "Individual total {0} - {1} LESS" },
            { BetTypes.TOTAL2_MORE, "Individual total {0} - {1} MORE" }, { BetTypes.TOTAL2_LESS, "Individual total {0} - {1} LESS" },
            { BetTypes.TOTAL_FIRTS_HALF_MORE, "Total first half - {0} MORE" }, { BetTypes.TOTAL_FIRTS_HALF_LESS, "Total first half - {0} LESS" },
            { BetTypes.TOTAL_SECOND_HALF_MORE, "Total second half - {0} MORE" }, { BetTypes.TOTAL_SECOND_HALF_LESS, "Total second half - {0} LESS" },
            { BetTypes.CUSTOM, "{0}" },
        };

        public BetTypes BetType { private set; get; }
        public string Description { private set; get; }
        public double Goals { private set; get; }

        protected BetBase(BetTypes betType)
        {
            InitBase(betType);
            Description = betTypesDictionary[BetType];
        }
        protected BetBase(BetTypes betType, string teamNameOrDescription)
        {
            InitBase(betType);
            Description = String.Format(betTypesDictionary[BetType], teamNameOrDescription);
        }
        protected BetBase(BetTypes betType, double goals = 0.0)
        {
            InitBase(betType);
            Description = String.Format(betTypesDictionary[BetType], goals.ToString());
            Goals = goals;
        }
        protected BetBase(BetTypes betType, string teamName, double goals = 0.0)
        {
            InitBase(betType);
            Description = String.Format(betTypesDictionary[BetType], teamName, goals.ToString());
            Goals = goals;
        }
        private void InitBase(BetTypes betType)
        {
            BetType = betType;
            Description = "";
            Goals = -1.0;
        }
    }
}
