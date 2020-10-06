using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePhoneBook
{
    class Program
    {
        static void Main(string[] args)
        {
            PhoneBookManager manager = PhoneBookManager.CreateManagerInstance();

            manager.ReadToFile();

            while (true)
            {
                try
                {
                    int choice;
                    while (true)
                    {
                        manager.ShowMenu();
                        if (int.TryParse(Console.ReadLine(), out choice))
                        {
                            if (choice < 1 || choice > 6)
                            {
                                throw new MenuChoiceException(choice);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    switch (choice)
                    {
                        case 1: manager.InputData(); break;
                        case 2: manager.ListData(); break;
                        case 3: manager.SearchData(); break;
                        case 4: manager.SortData(); break;
                        case 5: manager.DeleteData(); break;
                        case 6: manager.WriteToFile().WriteLine("프로그램을 종료합니다"); return;
                    }
                }
                catch (MenuChoiceException err)
                {
                    err.ShowWrongChoice();
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
            }
        }
    }
}
