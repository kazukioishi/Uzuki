﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Uzuki._2ch.Parser
{
    class ThreadListParser
    {
        public static List<BBSThread> ParseThread(String text)
        {
            String[] Row = text.Split(new string[] { "\n", Environment.NewLine }, StringSplitOptions.None);
            List<BBSThread> threadList = new List<BBSThread>();
            int counter = 0;
            foreach (String line in Row)
            {
                if (line == "") break;
                BBSThread thread = new BBSThread();
                String[] LineSplit = line.Split(new string[] { "<>"}, StringSplitOptions.None);
                thread.DAT = LineSplit[0];
                thread.UnixTime = long.Parse(Regex.Replace(LineSplit[0],@"(\.dat|\.cgi)",""));
                thread.createdAt = _2ch.Tools.UnixTime.FromUnixTime(thread.UnixTime);
                MatchCollection mc = Regex.Matches(LineSplit[1],@"(.*.)\((\d+)\)");
                thread.ResCount = int.Parse(mc[0].Groups[2].Value);
                thread.Title = System.Web.HttpUtility.HtmlDecode(mc[0].Groups[1].Value);
                thread.Number = counter;
                counter++;
                threadList.Add(thread);
            }
            return threadList;
        }
    }
}
