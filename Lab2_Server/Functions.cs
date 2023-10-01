using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;

namespace Lab1
{
    public class Functions
    {
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

        public string PrintAllNotes(ref List<Student> students)
        {
            string StudentsList = "";
            for (int i = 0; i < students.Count; i++)
            {
                StudentsList += (i + 1).ToString() + ": " +
                    "ФИО: " + students[i].Surname + " " + students[i].Name + " " + students[i].Patronymic +
                    " | Пол: " + ConvertSex(students[i].Sex).ToString() +
                    " | Возраст: " + students[i].Age + "\n";
            }
            return StudentsList;
        }

        public string ConvertSex(bool s)
        {
            if (s == true) { return "М"; }
            else { return "Ж"; }
        }

        public string PrintNotesByNumber(int note_number, ref List<Student> students)
        {
            try
            {
                Student nstudent = students[note_number - 1];
                string student = "ФИО: " + nstudent.Surname + " "
                                + nstudent.Name + " "
                                + nstudent.Patronymic + " | "
                                + "Пол: " + ConvertSex(nstudent.Sex) + " | "
                                + "Возраст: " + nstudent.Age + "\n";
                return student;
            }
            catch (Exception e) { return "Такой записи нет!\n"; }
        }

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

        public string RemoveNotesFromFile(int note_number, ref List<Student> students)
        {
            try
            {
                students.RemoveAt(note_number-1);
                return "";
            }
            catch (Exception e) { return "Записи с таким номром не существует!\n"; }
        }

        public string AddNoteToFile(string surname, string name, string patronymic, string sex, int age, ref List<Student> students)
        {
            bool tsex = true;
            if (sex == "М" || sex == "м") { tsex = true; }
            if (sex == "Ж" || sex == "ж") { tsex = false; } else { return "Форма заполнена не верно!\n"; }
            try
            {
                students.Add(new Student(surname, name, patronymic, tsex, age));
                return "";
            }
            catch (Exception e) { return "Форма заполнена не верно!\n"; }
        }
    }
}

