using Lab1;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Lab2_Server
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static UdpClient udpServer;
        public string message = "";
        private static string file_path = "students.csv";

        static void Main(string[] args)
        {
            udpServer = new UdpClient(8001);
            Program p = new Program();
            Functions f = new Functions();
            List<Student> students = f.ReadAllDate(file_path);

            Console.WriteLine("Server works!");

            while (true)
            {
                string response;
                try
                {
                    p.ReceiveMessage();
                    logger.Info("Server got:" + p.message);
                    switch (p.message)
                    {
                        case "1":
                            response = f.PrintAllNotes(ref students);
                            logger.Info($"Server sent: {response}");
                            p.SendMessage(response + "\n" + p.Menu());
                            break;
                        case "2":
                            p.SendMessage("Enter number:");
                            p.ReceiveMessage();
                            int number_to_print = Int32.Parse(p.message);
                            logger.Info($"Server got: {number_to_print}");
                            response = f.PrintNotesByNumber(number_to_print, ref students);
                            logger.Info($"Server sent: {response}");
                            p.SendMessage(response + "\n" + p.Menu());
                            break;
                        case "3":
                            f.WriteNotesToFile(ref students, file_path);
                            logger.Debug("Data was saved");
                            p.SendMessage("Data was saved!" + "\n" + p.Menu());
                            break;
                        case "4":
                            p.SendMessage("Enter number:");
                            p.ReceiveMessage();
                            try
                            {
                                int number_to_delete = Int32.Parse(p.message);
                                logger.Info($"Server got: {number_to_delete}");
                                f.RemoveNotesFromFile(number_to_delete, ref students);
                                logger.Debug($"Item with inxex {number_to_delete - 1} deleted");
                                p.SendMessage("Item was deleted!" + "\n" + p.Menu());
                            }
                            catch(Exception e) 
                            { 
                                p.SendMessage(e.Message);
                                logger.Error(e.StackTrace);
                            }
                            break;
                        case "5":
                            p.SendMessage("Enter surname:");
                            p.ReceiveMessage();
                            string surname = p.message;
                            logger.Info($"Server got: {surname}");

                            p.SendMessage("Enter name:");
                            p.ReceiveMessage();
                            string name = p.message;
                            logger.Info($"Server got: {name}");

                            p.SendMessage("Enter patronymic:");
                            p.ReceiveMessage();
                            string patronymic = p.message;
                            logger.Info($"Server got: {patronymic}");

                            p.SendMessage("Enter sex:");
                            p.ReceiveMessage();
                            string sex = p.message;
                            logger.Info($"Server got: {sex}");

                            p.SendMessage("Enter age:");
                            p.ReceiveMessage();
                            try
                            {
                                int age = Int32.Parse(p.message);
                                logger.Info($"Server got: {age}");
                                f.AddNoteToFile(surname, name, patronymic, sex, age, ref students);
                                logger.Debug("New note added");
                                p.SendMessage("Note added!" + "\n" + p.Menu());
                            }
                            catch(Exception e) 
                            { 
                                p.SendMessage("Incorrect input!" + "\n" + p.Menu());
                                logger.Error(e.StackTrace);
                            }
                            break;
                        default:
                            response = "There is no such item!";
                            p.SendMessage(response + "\n" + p.Menu());
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    logger.Error(ex.StackTrace);
                }
            }
        }

        public void SendMessage(string response)
        {
            try
            {
                //Console.WriteLine("Enter your response:");
                string message = response;
                byte[] data = Encoding.Unicode.GetBytes(message);
                udpServer.Send(data, data.Length, "127.0.0.1", 8002);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ReceiveMessage()
        {
            IPEndPoint remoteIP = (IPEndPoint)udpServer.Client.LocalEndPoint;
            try
            {
                byte[] data = udpServer.Receive(ref remoteIP);
                message = Encoding.Unicode.GetString(data);
                Console.WriteLine("Message from client: {0}", message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        public string Menu()
        {
            return "\nЧто хотите сделать?:\n" +
                    "1. Вывод всех записей на экран.\n" +
                    "2. Вывод записи по номеру.\n" +
                    "3. Запись данных в файл.\n" +
                    "4. Удаление записи из файла.\n" +
                    "5. Добавление записи в файл.\n" +
                    "Выход из приложения - ESC.\n";
        }
    }
}
