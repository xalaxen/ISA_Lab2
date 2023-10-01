using Lab1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab2_Server
{
    public class Server
    {
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private UdpClient udpClient_S;

        private int port;

        private static string file_path = "students.csv";
        static Functions fn = new Functions();

        List<Student> students = fn.ReadAllDate(file_path);

        public Server(int _port)
        {
            udpClient_S = new UdpClient(_port);
            this.port = _port;
            Console.WriteLine("Server works");
        }

        public async Task StartListenAsync()
        {
            while (true)
            {
                allDone.Reset();
                UdpReceiveResult result = await udpClient_S.ReceiveAsync();
                _ = RequestCallback(result);
                allDone.WaitOne();
            }
        }

        private async Task RequestCallback(UdpReceiveResult result)
        {
            allDone.Set();
            var ep = result.RemoteEndPoint;
            string clientMessage = Encoding.Unicode.GetString(result.Buffer);
            Console.WriteLine("Message from client: {0}", clientMessage);

            string serverResponse = await ProccesingClientCommands(clientMessage);
            byte[] response = Encoding.Unicode.GetBytes(serverResponse);
            udpClient_S.Send(response, response.Length, ep);
        }

        private async Task<string> ProccesingClientCommands(string clientMessage)
        {
            switch (clientMessage)
            {
                case "1":
                    SendMessage(fn.PrintAllNotes(ref students));
                    return "";
                case "2":
                    await PrintNotesByNumber();
                    return "";
                case "3":
                    await SaveNotes();
                    return "";
                case "4":
                    await DeleteNote();
                    return "";
                case "5":
                    await AddingNewNote();
                    return "";
                default:
                    SendMessage("There is no such item!");
                    return "";
            }  
        }

        private async Task DeleteNote()
        {
            SendMessage("Enter item number:");
            int note_delete = Int32.Parse(ReceiveMessage());
            fn.RemoveNotesFromFile(note_delete, ref students);
            SendMessage("Element deleted");
            await Task.Delay(1000);
        }

        private async Task SaveNotes()
        {
            fn.WriteNotesToFile(ref students, file_path);
            SendMessage("Data sevedd to file\n");
            await Task.Delay(1000);
        }

        private async Task PrintNotesByNumber()
        {
            SendMessage("Enter number:");
            int note_number = Int32.Parse(ReceiveMessage());
            SendMessage(fn.PrintNotesByNumber(note_number, ref students));
            await Task.Delay(1000);
        }

        private async Task AddingNewNote()
        {
            SendMessage("Enter surname:");
            string surname = ReceiveMessage();
            SendMessage("Enter name:");
            string name = ReceiveMessage();
            SendMessage("Enter patronomyc:");
            string patronomyc = ReceiveMessage();
            SendMessage("Enter sex:");
            string sex = ReceiveMessage();
            SendMessage("Enter age:");
            int age = Int32.Parse(ReceiveMessage());

            fn.AddNoteToFile(surname, name, patronomyc, sex, age, ref students);
            SendMessage("Note added!");
            await Task.Delay(1000);
        }

        private string Menu()
        {
            return "\nЧто хотите сделать?:\n" +
                    "1. Вывод всех записей на экран.\n" +
                    "2. Вывод записи по номеру.\n" +
                    "3. Запись данных в файл.\n" +
                    "4. Удаление записи из файла.\n" +
                    "5. Добавление записи в файл.\n" +
                    "Выход из приложения - ESC.\n";
        }

        public void SendMessage(string text)
        {
            try
            {
                string message = text;
                byte[] data = Encoding.Unicode.GetBytes(message);
                udpClient_S.Send(data, data.Length, "127.0.0.1", 8002);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public string ReceiveMessage()
        {
            string message = "";
            IPEndPoint remoteIP = (IPEndPoint)udpClient_S.Client.LocalEndPoint;
            try
            {
                byte[] data = udpClient_S.Receive(ref remoteIP);
                message = Encoding.Unicode.GetString(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return message;
        }
    }
}
