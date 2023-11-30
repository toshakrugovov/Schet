using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourNamespace;

namespace Тест_на_скоропечатение
{
    public class Test
    {
        private static string testText = "Сегодня был прекрасный день. Я прогулялся по парку, наслаждаясь теплым солнечным днем и свежим весенним воздухом и пением птиц. Как же хочется каникул";
         
        private static int currentIndex;
        private static Stopwatch stopwatch;
        private static bool typingFinished;
       

        public static async Task StartTestAsync(string userName)
        {

            Console.Clear();

            Console.WriteLine($"Добро пожаловать, {userName}! Приготовьтесь к тесту на скоропечатание.");
            Console.WriteLine("Нажмите Enter, чтобы начать печатать.");

            Console.ReadLine();

            Console.Clear();
            Console.WriteLine($"Введите следующий текст:\n\n{testText}\n");

            currentIndex = 0;
            stopwatch = Stopwatch.StartNew();
            typingFinished = false;

            Task timerTask = StartTimerAsync();
            Task typingTask = TypingTaskAsync();

            await Task.WhenAll(timerTask, typingTask);

            stopwatch.Stop();

            Console.Clear();
            Console.WriteLine("Тест завершен!\n");

            int charactersPerMinute = (int)(currentIndex / stopwatch.Elapsed.TotalMinutes);
            Console.WriteLine($"{userName}, ваша скорость печати: {charactersPerMinute} знаков в минуту");

            LeaderboardManager.SaveUserResult(userName, charactersPerMinute);

            Console.WriteLine("\nТаблица лидеров:\n");
            LeaderboardManager.DisplayLeaderboard();

            Console.WriteLine("\nНажмите Enter, чтобы продолжить печать.");
            Console.ReadLine();




        }



        private static async Task StartTimerAsync()
        {
            while (!typingFinished && stopwatch.Elapsed.TotalSeconds < 60)
            {
                TimeSpan timeLeft = TimeSpan.FromSeconds(60) - stopwatch.Elapsed;
                Console.SetCursorPosition(0, 4);
                Console.Write($"Осталось времени: {timeLeft.ToString(@"hh\:mm\:ss")}");
                await Task.Delay(1000);
            }

            typingFinished = true;
        }

        private static Task<ConsoleKeyInfo> ReadKeyAsync()
        {
            var tcs = new TaskCompletionSource<ConsoleKeyInfo>();
            Thread backgroundThread = new Thread(() =>
            {
                try
                {
                    tcs.SetResult(Console.ReadKey(true));
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            backgroundThread.Start();
            return tcs.Task;
        }





        private static async Task TypingTaskAsync()
        {
            Console.CursorVisible = false;

            Console.SetCursorPosition(0, 2);
            Console.Write(testText);

            Console.SetCursorPosition(currentIndex, 2);

            while (!typingFinished && stopwatch.Elapsed < TimeSpan.FromSeconds(60))
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    char expectedChar = testText[currentIndex];

                    if (keyInfo.KeyChar == expectedChar)
                    {
                        if (currentIndex < Console.BufferWidth - 1)
                        {
                            Console.SetCursorPosition(currentIndex, 2);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(keyInfo.KeyChar);
                            Console.ResetColor();
                            currentIndex++;
                        }
                    }
                    else
                    {
                        if (currentIndex < Console.BufferWidth - 1)
                        {
                            Console.SetCursorPosition(currentIndex, 2);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(expectedChar);
                            Console.ResetColor();
                        }
                    }

                    if (currentIndex == testText.Length)
                    {
                        typingFinished = true;
                    }
                }
            }
        }



    }
}
