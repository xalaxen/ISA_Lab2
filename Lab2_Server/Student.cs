using Lab2_Server;
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
        private int? groupId;
        private Group group;

        public Student() { }

        public Student(string nsurname, string nname, string npatronymic, bool nsex, int nage, int ngroupId)
        {
            Surname = nsurname;
            Name = nname;
            Patronymic = npatronymic;
            Sex = nsex;
            Age = nage;
            GroupId = ngroupId;
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

        public Group Group
        {
            get { return group; }
            set { group = value; }
        }

        public int? GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }
    }
}

