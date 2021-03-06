using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConsoleTables;
using Newtonsoft.Json;

namespace DailyJournal
{
    class Journal
    {
        public static string[] WeekD = new string[7];
        public static string SelectedDate = null;

        static void Main(string[] args)
        {
            DateTime CurTime = DateTime.Now;
            DateTime NewTime = DateTime.Now;
            Dictionary<string, List<string>> Actions = new Dictionary<string, List<string>>();

            string[] Week = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            string WDay = CurTime.DayOfWeek.ToString();
            short WDayNum = Convert.ToInt16(Array.IndexOf(Week, WDay));

            if (File.Exists("djournal.json"))
            {
                Actions = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>
                    (File.ReadAllText("djournal.json")); // reading Actions from JSON if it exists
            }
            else
            {
                Actions = new Dictionary<string, List<string>>();
                File.WriteAllText("djournal.json", JsonConvert.SerializeObject(Actions)); // writing down a JSON file if it does not exist
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
                        WDay = CurTime.DayOfWeek.ToString();
                        WDayNum = Convert.ToInt16(Array.IndexOf(Week, WDay));
                        DefaultTable(DateTime.Now, WDayNum);
                        break;
                    case "select":
                        Console.WriteLine("Enter the date (dd.mm.yyyy):");
                        SelectedDate = Console.ReadLine();
                        DefaultTable(DateTime.Now, WDayNum);
                        if (SelectedDate.Length == 10
                            && SelectedDate[2] == '.'
                            && SelectedDate[5] == '.')
                        {
                            Console.WriteLine("Successfully selected!");
                        }
                        else
                        {
                            Console.WriteLine("Date wasn't selected: Incorrect date.");
                        }
                        break;
                    case "add":
                        if (!(string.IsNullOrEmpty(SelectedDate)))
                        {
                            Console.WriteLine("Enter the time (hh:mm):");
                            string ActTime = Console.ReadLine();

                            Console.WriteLine("Enter a small descryption:");
                            string ActDesc = ActTime + " - " + Console.ReadLine();

                            DefaultTable(DateTime.Now, WDayNum);

                            if (ActTime[2] == ':')
                            {
                                if(Actions.ContainsKey(SelectedDate))
                                {
                                    Actions[SelectedDate].Add(ActDesc);
                                }
                                else
                                {
                                    List<string> SelectedDateActions = new List<string>();
                                    SelectedDateActions.Add(ActDesc);
                                    Actions.Add(SelectedDate, SelectedDateActions);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Note wasn't saved: Incorrect time.");
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
                                DefaultTable(CurTime, WDayNum);
                                Console.WriteLine();
                                var SelActions = new ConsoleTable("Notes");
                                for(int i = 0; i < Actions[SelectedDate].Count(); i++)
                                {
                                    SelActions.AddRow(Actions[SelectedDate][i]);
                                }

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
            if (!(string.IsNullOrEmpty(SelectedDate)))
            {
                Console.WriteLine("Selected date is: " + SelectedDate);
            }
            else
            {
                Console.WriteLine("No currently selected date.");
            }
            Console.WriteLine();

            for(int i = 0; i < 7; i++) // parsing dates of days of week
            {
                NewTime = CurTime.AddDays(i - WDayNum);
                WeekD[i] = String.Format("{0:dd.MM.yyyy}", NewTime);
            }

            var tableD = new ConsoleTable("Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday");
            tableD.AddRow(WeekD[0], WeekD[1], WeekD[2], WeekD[3], WeekD[4], WeekD[5], WeekD[6]); // building seccond row with dates

            tableD.Write(Format.Alternative);
        }
    }
}
