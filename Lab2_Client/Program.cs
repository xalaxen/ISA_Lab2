using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using System.Threading;

namespace Lab2_Client
{
    class Program
    {
        public static UdpClient udpClient;
        private static StringBuilder inputBuilder = new StringBuilder();

        static void Main(string[] args)
        {
            udpClient = new UdpClient(8002);
            Program p = new Program();
            Console.WriteLine("Client works!");
            Console.WriteLine(p.Menu());

            while (true)
            {
                try
                {
                    ConsoleKeyInfo key = Console.ReadKey(false);

                    if (key.Key != ConsoleKey.Escape)
                    {
                        if(key.Key == ConsoleKey.Enter)
                        {
                            p.SendMessage(inputBuilder.ToString());
                            inputBuilder.Clear();
                            Console.WriteLine();
                            p.ReceiveMessage();
                        }
                        else
                        {
                            inputBuilder.Append(key.KeyChar);
                        }
                    }
                    else { break; }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
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

        public void SendMessage(string message)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(message);
                udpClient.Send(data, data.Length, "127.0.0.1", 8001);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ReceiveMessage()
        {
            IPEndPoint remoteIP = (IPEndPoint)udpClient.Client.LocalEndPoint;
            try
            {
                byte[] data = udpClient.Receive(ref remoteIP);
                string message = Encoding.Unicode.GetString(data);
                Console.WriteLine("{0}", message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
