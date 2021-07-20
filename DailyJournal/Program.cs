using System;
using System.Collections.Generic;
using System.IO;
using ConsoleTables;
using Newtonsoft.Json;

namespace DailyJournal
{
    class Journal
    {
        public static string[] WeekD = new string[7];

        static void Main(string[] args)
        {
            var CurTime = DateTime.Now;
            var NewTime = DateTime.Now;
            var Actions = new Dictionary<string, string>();

            string[] Week = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            string WDay = CurTime.DayOfWeek.ToString(), SelectedDate = null;
            short WDayNum = Convert.ToInt16(Array.IndexOf(Week, WDay));

            if (File.Exists("djournal.json"))
            {
                Actions = JsonConvert.DeserializeObject<Dictionary<string, string>>
                    (File.ReadAllText("djournal.json"));
            }
            else
            {
                Actions = new Dictionary<string, string>();
                File.WriteAllText("djournal.json", JsonConvert.SerializeObject(Actions));
            }

            DefaultTable(CurTime, WDayNum);
            while (true)
            {
                string Ans = Console.ReadLine();
                switch (Ans) 
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
                    case "select":
                        Console.WriteLine("Enter the date (dd.mm.yyyy):");
                        SelectedDate = Console.ReadLine();
                        Console.WriteLine("Successfully selected!");
                        break;
                    case "add":
                        if (!(string.IsNullOrEmpty(SelectedDate)))
                        {
                            Console.WriteLine("Enter the time (hh:mm):");
                            string ActTime = Console.ReadLine();

                            Console.WriteLine("Enter a small descryption:");
                            string ActDesc = ActTime + " - " + Console.ReadLine();

                            if(Actions.ContainsKey(SelectedDate))
                            {
                                Actions[SelectedDate] += "\n" + ActDesc;
                            }
                            else
                            {
                                Actions.Add(SelectedDate, ActDesc);
                            }
                            

                            File.WriteAllText("djournal.json", JsonConvert.SerializeObject(Actions));
                            
                            Console.WriteLine("Successfully added!");
                        }
                        else
                            Console.WriteLine("First select the date!");
                        break;
                    case "show":
                        if (!(string.IsNullOrEmpty(SelectedDate)))
                        {
                            if (Actions.ContainsKey(SelectedDate))
                            {
                                var SelActions = new ConsoleTable(Actions[SelectedDate]);
                                SelActions.Write(Format.Alternative);
                            }
                            else
                            {
                                Console.WriteLine("There aren't any notes for selected date.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("First select the date!");
                        }
                        break;
                    case "help":
                        Console.WriteLine("add - adds a note on selected day");
                        Console.WriteLine("back - previous week");
                        Console.WriteLine("help - shows this message");
                        Console.WriteLine("next - next week");
                        Console.WriteLine("now - shows current week");
                        Console.WriteLine("refresh - refreshes the time and clears the console");
                        Console.WriteLine("select - selecting the day to add/remove notes");
                        Console.WriteLine("show - shows the list of notes for selected date");
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
