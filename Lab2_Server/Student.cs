using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    public class Student
    {
        public int Id {get; set;}

        private string surname;
        private string name;
        private string patronymic;
        private bool sex;
        private int age;
        private string group;

        public Student() { }

        public Student(string nsurname, string nname, string npatronymic, bool nsex, int nage, string ngroup)
        {
            Surname = nsurname;
            Name = nname;
            Patronymic = npatronymic;
            Sex = nsex;
            Age = nage;
            Group = ngroup;
        }

        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Patronymic
        {
            get { return patronymic; }
            set { patronymic = value; }
        }

        public bool Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public string Group
        {
            get { return group; }
            set { group = value; }
        }
    }
}

