using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using Lab2_Server;
using System.Data.Entity;

namespace Lab1
{
    public class Functions
    {
        // не нужный метод, тк даныне перешли в базу данных
        public List<Student> ReadAllDate(string path)
        {
            List<Student> students = new List<Student>();
            using (StreamReader sr = new StreamReader(path))
            {
                string tempNotes;
                while ((tempNotes = sr.ReadLine()) != null)
                {
                    try
                    {
                        students.Add(new Student(tempNotes.Split(',')[0],
                                                tempNotes.Split(',')[1],
                                                tempNotes.Split(',')[2],
                                                Convert.ToBoolean(tempNotes.Split(',')[3]),
                                                Convert.ToInt32(tempNotes.Split(',')[4])));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }

            return students;
        }

        public string PrintAllNotes(ref DbSet<Student> students)
        {
            string StudentsList = "";
            foreach (Student s in students)
            {
                StudentsList += "ID: " + s.Id.ToString() + " | " +
                    "ФИО: " + s.Surname.ToString() + " " + s.Name.ToString() + " " + s.Patronymic.ToString() +
                    " | Пол: " + ConvertSex(s.Sex).ToString() +
                    " | Возраст: " + s.Age.ToString() + "\n";
            }
            return StudentsList;
        }

        public string ConvertSex(bool s)
        {
            if (s == true) { return "М"; }
            else { return "Ж"; }
        }

        public string ConvertSex(string s)
        {
            if (s == "М" || s == "м") { return "true"; }
            if(s == "Ж" || s == "ж") { return "false"; }
            else { return ""; }
        }

        public string PrintNotesByNumber(int note_number, ref DbSet<Student> students)
        {
            try
            {
                Student nstudent = students.Find(note_number);
                string student = "ФИО: " + nstudent.Surname + " "
                                + nstudent.Name + " "
                                + nstudent.Patronymic + " | "
                                + "Пол: " + ConvertSex(nstudent.Sex) + " | "
                                + "Возраст: " + nstudent.Age + "\n";
                return student;
            }
            catch (Exception e) { return "Такой записи нет!\n"; }
        }

        // так же не нужный метод из-за перехода на базу данных
        public string WriteNotesToFile(ref List<Student> students, string path)
        {
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                foreach (var student in students)
                {
                    string student_line = student.Surname + "," + student.Name + "," +
                        student.Patronymic + "," + student.Sex + "," + student.Age;
                    sw.WriteLine(student_line);
                }
            }
            return "Даныне записаны в файл!\n";
        }

        public string RemoveNotesFromFile(int note_number)
        {
            using (StudentContext db = new StudentContext())
            {
                try
                {
                    db.Students.Remove(db.Students.Find(note_number));
                    db.SaveChanges();
                    return "";
                }
                catch (Exception e) { return "Записи с таким номром не существует!\n"; }
            }
        }

        public string AddNoteToFile(string surname, string name, string patronymic, string sex, int age)
        {
            using (StudentContext db = new StudentContext())
            {
                try
                {
                    db.Students.Add(new Student(surname, name, patronymic, Convert.ToBoolean(ConvertSex(sex)), age));
                    db.SaveChanges();
                    return "";
                }
                catch (Exception e) { return "Форма заполнена не верно!\n"; }
            }
        }
    }
}

