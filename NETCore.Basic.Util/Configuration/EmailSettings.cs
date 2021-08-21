using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Util.Configuration
{
    public static class EmailSettings
    {
        public static string SMTPEmail { get; private set; }
        public static string SMTPPassword { get; private set; }
        public static int SMTPPort { get; private set; }
        public static string SMTPHostname { get; private set; }
    }
}
