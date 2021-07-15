using System;
using ConsoleTables;

namespace DailyJournal
{
    class Journal
    {
        static void Main(string[] args)
        {
            var CurTime = DateTime.Now;
            var NewTime = DateTime.Now;
            string WDay = CurTime.DayOfWeek.ToString();
            string[] Week = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            short WDayNum = Convert.ToInt16(Array.IndexOf(Week, WDay));

            var table = new ConsoleTable(Week[0], Week[1], Week[2], Week[3], Week[4], Week[5], Week[6]);
            DefaultTable(CurTime, WDayNum);
            while (true)
            {
                string Ans = Console.ReadLine();
                switch(Ans)
                {
                    case "next":
                        NewTime = NewTime.AddDays(7);
                        DefaultTable(NewTime, WDayNum);
                        break;
                    case "back":
                        NewTime = NewTime.AddDays(-7);
                        DefaultTable(NewTime, WDayNum);
                        break;
                    case "now":
                        DefaultTable(DateTime.Now, WDayNum);
                        break;
                    case "refresh":
                        CurTime = DateTime.Now;
                        DefaultTable(DateTime.Now, WDayNum);
                        break;
                    default:
                        Console.WriteLine("Please, check the syntax!");
                        break;
                }
            }
        }

        static void DefaultTable(DateTime CurTime, short WDayNum)
        {
            Console.Clear();

            var NewTime = CurTime;
            Console.WriteLine("Current date is: " + String.Format("{0:dd.MM.yyyy}", DateTime.Now) + ", " + CurTime.DayOfWeek.ToString());
            Console.WriteLine();
            string[] WeekD = new string[7];
            for(int i = 0; i < 7; i++)
            {
                NewTime = CurTime.AddDays(i - WDayNum);
                WeekD[i] = String.Format("{0:dd.MM.yyyy}", NewTime);
            }
            var tableD = new ConsoleTable("Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday");
            tableD.AddRow(WeekD[0], WeekD[1], WeekD[2], WeekD[3], WeekD[4], WeekD[5], WeekD[6]);
            tableD.Write(Format.Alternative);
        }
    }
}
