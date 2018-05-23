using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SharedLibrary.Messages;

namespace SharedLibrary
{
    public class ConsolePlus
    {
        private const int _maxLogs = 100;
        private const int _maxText = 50;
        private static readonly Regex _regExit = new Regex(@"^/exit\s*$");
        private static readonly Regex _regRegister = new Regex(@"^/register\s+(?<clientName>\w+)\s*$");
        private static readonly Regex _regJoin = new Regex(@"^/join\s+(?<gameName>\w+)\s*$");
        private static readonly Regex _regMessage = new Regex(@"^(?<message>[^/].*)$");
        private readonly IMessageStream messageStream;
        private Queue<string> logs = new Queue<string>();
        private StringBuilder text = new StringBuilder(_maxText);
        private string BottomLine => $":{text.ToString()}";
        public string RoomKey { get; set; } = String.Empty;
        public ConsolePlus(IMessageStream messageStream)
        {
            this.messageStream = messageStream;
        }
        private void Reset()
        {
            lock (this)
            {
                int width = Console.WindowWidth;
                int height = Console.WindowHeight;
                Console.Clear();
                string emptyLine = new string(' ', width);
                for (int i = 0; i < height - logs.Count - 2; ++i)
                    Console.WriteLine(emptyLine);
                foreach (string str in logs.Take(height - 2))
                    Console.WriteLine(str.Substring(0, Math.Min(str.Length, width)));
                Console.WriteLine(new string('-', width - 1));
                Console.Write(BottomLine);
            }
        }
        public void StartReading()
        {
            bool work = true;
            Reset();
            while (work)
            {
                var key = Console.ReadKey();
                lock (this)
                {
                    if (!char.IsControl(key.KeyChar))
                    {
                        if (text.Length < _maxText)
                            text.Append(key.KeyChar);
                    }
                    else
                    {
                        switch (key.Key)
                        {
                            case ConsoleKey.Enter:
                                ProcessText(out work);
                                break;
                            case ConsoleKey.Backspace:
                                if (text.Length > 0)
                                    text.Remove(text.Length - 1, 1);
                                break;
                            default:
                                break;
                        }
                    }
                    Console.CursorLeft = 0;
                    Console.Write(new string(' ', Console.WindowWidth - 1));
                    Console.CursorLeft = 0;
                    Console.Write(BottomLine);
                }
            }
        }

        private void ProcessText(out bool work)
        {
            string str = text.ToString();
            text.Clear();
            Match match = _regExit.Match(str);
            work = !match.Success;
            if (!work)
                return;
            match = _regRegister.Match(str);
            if (match.Success)
            {
                messageStream.SendMessageAsync(new RegisterMessage {Name = match.Groups["clientName"].Value});
                return;
            }
            match = _regJoin.Match(str);
            if (match.Success)
            {
                messageStream.SendMessageAsync(new JoinRoomMessage { RoomKey = match.Groups["gameName"].Value});
                return;
            }
            match = _regMessage.Match(str);
            if (match.Success)
            {
                messageStream.SendMessageAsync(new ChatRoomMessage {RoomKey = RoomKey, Value = match.Groups["message"].Value});
                return;
            }
            Print("Unkown command.");
        }
        public void Print(string log)
        {
            lock(this)
            {
                logs.Enqueue(log);
                if (logs.Count > _maxLogs)
                    logs.Dequeue();
                Reset();
            }
        }
    }
}