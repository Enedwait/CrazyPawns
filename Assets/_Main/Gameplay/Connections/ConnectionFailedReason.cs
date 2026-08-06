using System;

namespace Main.Gameplay.Connections
{
    public enum ConnectionFailedReason { None, NoSocket, SameRoot, AlreadyExists, Unknown }

    public static class ConnectionFailedReasonExtensions
    {
        public static string ToMessage(this ConnectionFailedReason reason) =>
            reason switch
            {
                ConnectionFailedReason.None => null,
                ConnectionFailedReason.NoSocket => "Один из сокетов не установлен",
                ConnectionFailedReason.SameRoot => "Сокеты принадлежат одному владельцу",
                ConnectionFailedReason.AlreadyExists => "Соединение уже установлено",
                ConnectionFailedReason.Unknown => "Неизвестно",
                _ => throw new NotImplementedException(),
            };
    }
}
