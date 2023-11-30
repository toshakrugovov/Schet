using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Тест_на_скоропечатение;

namespace YourNamespace
{
    public class LeaderboardManager
    {
        public static readonly string leaderboardFilePath = "лидеры.json";
        public static List<User> leaderBoard;

        public static async Task<ConsoleKeyInfo> ReadKeyAsync()
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
            return await tcs.Task;
        }

        public static void SaveUserResult(string userName, int charactersPerMinute)
        {
            List<User> users = LoadUsers();
            User user = users.FirstOrDefault(u => u.Name == userName);

            if (user == null)
            {
                user = new User { Name = userName };
                users.Add(user);
            }

            user.CharactersPerMinute = charactersPerMinute;

            SaveUsers(users);
        }

        public static List<User> LoadUsers()
        {
            if (File.Exists(leaderboardFilePath))
            {
                string json = File.ReadAllText(leaderboardFilePath);
                return JsonSerializer.Deserialize<List<User>>(json);
            }

            return new List<User>();
        }

        public static void SaveUsers(List<User> users)
        {
            string json = JsonSerializer.Serialize(users);
            File.WriteAllText(leaderboardFilePath, json, System.Text.Encoding.UTF8);
        }

        public static void DisplayLeaderboard()
        {
            leaderBoard = LoadUsers().OrderByDescending(u => u.CharactersPerMinute).ToList();

            Console.WriteLine("{0,-15} {1,-25}", "Имя", "Символов за минуту");

            foreach (var user in leaderBoard)
            {
                Console.WriteLine($"{user.Name,-15} {user.CharactersPerMinute,-25}");
            }
        }
    }
}