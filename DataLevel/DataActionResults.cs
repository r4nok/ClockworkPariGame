using System;
using System.Collections.Generic;
using System.Text;

namespace DataLevel
{
    public enum DataActionResults
    {
        ALL_RIGHT = 1,
        GENERAL_EX,
        NOT_UNIQUE_EX, // Violation of UNIQUE KEY constraint
        NOT_READY,
        ALREADY_EXIST,
        MORE_THAN_1_ROW,
        NOT_IN_TOURNAMENT,
    }
}
