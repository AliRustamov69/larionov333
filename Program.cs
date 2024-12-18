using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

class Program
{
    private static readonly string token = "7768755149:AAE2-XzvAgo127AOPiavzHddLOKiCLrcnbo";
    private static readonly TelegramBotClient botClient = new TelegramBotClient(token);

    private static readonly List<string> businessIdeas = new List<string>
    {
        "Кафе для собак с меню из мяса и десертов.",
        "Виртуальный тренер для домашних животных.",
        "Служба доставки еды для людей, которые не хотят выходить из дома.",
        "Интернет-магазин для продажи пустых коробок.",
        "Курс по тому, как не делать ничего.",
        "Приложение для отслеживания, сколько раз вы не сделали зарядку.",
        "Услуга по созданию мемов на заказ.",
        "Платформа для аренды друзей на выходные.",
        "Бот, который генерирует случайные извинения.",
        "Школа для обучения кошек."
    };

    static async Task Main(string[] args)
    {
        var offset = 0;

        while (true)
        {
            var updates = await botClient.GetUpdatesAsync(offset);

            foreach (var update in updates)
            {
                offset = update.Id + 1; // Обновляем offset для получения новых обновлений

                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message && update.Message.Text != null)
                {
                    if (update.Message.Text == "/start")
                    {
                        await SendWelcomeMessage(update.Message.Chat.Id);
                    }
                    else if (update.Message.Text == "Получить бизнес-идею")
                    {
                        await SendBusinessIdea(update.Message.Chat.Id);
                    }
                    else if (update.Message.Text == "Выход")
                    {
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Спасибо за использование бота! До свидания!");
                    }
                }
            }

            // Задержка между запросами, чтобы не перегружать API
            Thread.Sleep(1000);
        }
    }

    private static async Task SendWelcomeMessage(long chatId)
    {
        var keyboard = new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "Получить бизнес-идею", "Выход" }
        })
        {
            ResizeKeyboard = true // Автоматически подгоняет размер клавиатуры
        };

        await botClient.SendTextMessageAsync(chatId, "Привет! Я генератор идиотских бизнес-идей. Выберите опцию ниже:", replyMarkup: keyboard);
    }

    private static async Task SendBusinessIdea(long chatId)
    {
        Random random = new Random();
        string idea = businessIdeas[random.Next(businessIdeas.Count)];
        await botClient.SendTextMessageAsync(chatId, $"Вот ваша бизнес-идея: {idea}");
    }
}
