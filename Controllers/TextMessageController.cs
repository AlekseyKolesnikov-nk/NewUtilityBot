using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using NewUtilityBot.Services;
using NewUtilityBot.Configuration;
using NewUtilityBot.Models;
using NewUtilityBot.Controllers;

namespace NewUtilityBot.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;


        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;

        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":

                    // Объект, представляющий кнопки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Текст" , $"txt"),
                        InlineKeyboardButton.WithCallbackData($" Целые числа" , $"num")
                    });

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>Бот считает количество знаков или сумму чисел в текстовом сообщении</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}Выберите нужный вариант{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;
                default:
                    
                    string choise = _memoryStorage.GetSession(message.Chat.Id).OptionCode;
                    
                    switch (choise)
                    {
                        case "txt":
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Длина сообщения: {message.Text.Length} знаков", cancellationToken: ct);

                            break;
                        case "num":
                            await _telegramClient.SendTextMessageAsync(message.From.Id, $"Сумма чисел в сообщении: {message.Text.Split(' ').Select(x => int.Parse(x)).Sum()} ", cancellationToken: ct);

                            break;
                    }
                    break;
            }
        }
    }
}                        