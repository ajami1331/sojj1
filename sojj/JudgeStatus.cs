﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sojj
{
    internal enum JudgeStatus
    {
        STATUS_ACCEPTED = 1,
        STATUS_WRONG_ANSWER = 2,
        STATUS_TIME_LIMIT_EXCEEDED = 3,
        STATUS_MEMORY_LIMIT_EXCEEDED = 4,
        STATUS_RUNTIME_ERROR = 6,
        STATUS_COMPILE_ERROR = 7,
        STATUS_SYSTEM_ERROR = 8,
        STATUS_JUDGING = 20,
        STATUS_COMPILING = 21,
    }
}
