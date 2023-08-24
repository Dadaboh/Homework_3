using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Homework_3.Game
{
    internal class Game
    {
        public bool playagain = false;
        public int help;
        public int diffChoise;
        public int range;
        public int key;
        Random random = new Random();

        public List<Tuple<int, int, string>> Records = new List<Tuple<int, int, string>>();

        #region Start
        public void Start()
        {           
            do
            {
                range = GetUsersDiffChoise();
                Console.WriteLine($"Вгадайте число від 0 до {range}. \nЩоб отримати підказку введіть ключове слово \"Help\"");

                switch (range)
                {
                    case 10:
                        help = 1;
                        break;

                    case 100:
                        help = 2;
                        break;

                    case 1000:
                        help = 4;
                        break;
                }

                key = random.Next(1, range);
                //Console.WriteLine(key);

                GetUsersAnswer();

            }
            while (playagain);
        }
        #endregion

        #region CheckDivideByFive
        public void CheckDivideByFive()
        {
            help--;

            var tmp = key % 5;
            if (tmp == 0)
            {
                Console.WriteLine("Число ділиться на 5.");
            }
            else
            {
                Console.WriteLine("Число не ділиться на 5.");
            }
            GetUsersAnswer();
        }
        #endregion

        #region CheckDivideByThree
        public void CheckDivideByThree()
        {
            help--;

            var tmp = key % 3;
            if (tmp == 0)
            {
                Console.WriteLine("Число ділиться на 3.");
            }
            else
            {
                Console.WriteLine("Число не ділиться на 3.");
            }
            GetUsersAnswer();
        }
        #endregion

        #region CheckPrimeNumber
        public void CheckPrimeNumber()
        {
            help--;

            var flag = 0;

            for(int i = 2; i < key; i++)
            {
                if (key % i == 0)
                {
                    Console.WriteLine("Число не є простим.");
                    flag = 1;
                    break;
                }
            }
            if (flag == 0)
            {
                Console.WriteLine("Число є простим.");
            }

            GetUsersAnswer();
        }
        #endregion

        #region CheckUsersAnswer
        public bool CheckUsersAnswer(string answear)
        {
            if (answear == "Help")
            {
                Help();
            }
            else if (!int.TryParse(answear, out int answearToTint))
            {
                Console.WriteLine("Можна вводити тільки числа та ключові слова.");
                GetUsersAnswer();
            }
            else
            {
                if (key == answearToTint)
                {
                    Win();
                    return false;
                }
                else
                {
                    Console.WriteLine("Не вірно.");
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region FinallChoise
        public void FinallChoise()
        {
            playagain = false;

            switch (Console.ReadLine())
            {
                case "H":
                    ShowHistory();
                    break;

                case "Y":
                    playagain = true;
                    break;

                case "N":
                    break;

                default:
                    FinallChoise();
                    break;
            }
        }
        #endregion

        #region GetUsersAnswer
        public void GetUsersAnswer()
        {
            var answer = "";

            do
            {
                answer = Console.ReadLine();
            }
            while (CheckUsersAnswer(answer));
        }
        #endregion

        #region GetUsersDiffChoise
        public int GetUsersDiffChoise()
        {
            Console.WriteLine("Оберіть рівень складності:\n 1 - низький\n 2 - середній\n 3 - високий");

            while (!int.TryParse(Console.ReadLine(), out diffChoise) || diffChoise < 1 || diffChoise > 3)
            {
                Console.WriteLine("Оберіть рівень складності.");
            }

            switch (diffChoise)
            {
                case 1:
                    return 10;

                case 2:
                    return 100;

                case 3:
                    return 1000;
            }

            return diffChoise;
        }
        #endregion

        #region Help
        public void Help()
        {
            if(help < 1)
            {
                Console.WriteLine("Більше немає доступних підказок.");
                GetUsersAnswer();
            }
            else
            {
                Console.WriteLine($"Оберіть тип підказки:\n 1 - Cкоротити діапазон рандомним числом в меншу сторону 1\n 2 - Дізнатися чи ділиться на 3" +
                $"\n 3 - Дізнатися чи ділиться на 5\n 4 - Дізнатися чи це просте число");

                int typeChoise;

                while (!int.TryParse(Console.ReadLine(), out typeChoise) || typeChoise < 1 || typeChoise > 4)
                {
                    Console.WriteLine("Оберіть рівень складності.");
                }

                switch (typeChoise)
                {
                    case 1:
                        RangeCutDown();
                        break;

                    case 2:
                        CheckDivideByThree();
                        break;

                    case 3:
                        CheckDivideByFive();
                        break;

                    case 4:
                        CheckPrimeNumber();
                        break;
                }
            }            
        }
        #endregion

        #region RangeCutDown
        public void RangeCutDown()
        {
            help--;

            var num = random.Next(1, range - key);
            range -= num;


            Console.WriteLine($"Вгадайте число від 0 до {range}. \nЩоб отримати підказку введіть ключове слово \"Help\"");
            GetUsersAnswer();
        }
        #endregion

        #region ShowHistory
        public void ShowHistory()
        {
            Console.WriteLine("---------------------------------------");
            foreach (var item in Records.OrderByDescending(o => o.Item1).OrderByDescending(o => o.Item2))
            {
                Console.WriteLine($"Користувач: {item.Item3} | Рівень складності: {item.Item1} | Підказок залишилось: {item.Item2}");
            }
            Console.WriteLine("---------------------------------------");

            Console.WriteLine("Щоб переглянути таблицю рекордів натисніть: \"H\", щоб грати знову: \"Y\", щоб завершити гру: \"N\"");
            FinallChoise();
        }
        #endregion

        #region Win
        public void Win()
        {
            Console.WriteLine("Правильна відповідь.");
            Console.WriteLine("Введіть ім'я користувача для таблиці рекордів:");

            var name = Console.ReadLine();
            Records.Add(Tuple.Create(diffChoise, help, name));

            Console.WriteLine("Щоб переглянути таблицю рекордів натисніть: \"H\", щоб грати знову: \"Y\", щоб завершити гру: \"N\"");
            FinallChoise();
            
        }
        #endregion

    }


}

