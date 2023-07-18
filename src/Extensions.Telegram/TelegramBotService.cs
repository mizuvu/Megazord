﻿using Telegram.Bot;
using Zord.Extensions.Telegram;

namespace Extensions.Telegram
{
    public class TelegramBotService : ITelegramBotService
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public TelegramBotService(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public async Task SendMessage(string message)
        {
            var chatId = -1001969080979;
            //var chatId = "@wtcvn";

            await _telegramBotClient.SendTextMessageAsync(chatId, message);
        }
    }
}
