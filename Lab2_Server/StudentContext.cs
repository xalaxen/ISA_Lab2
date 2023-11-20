using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Lab1;

namespace Lab2_Server
{
    class StudentContext:DbContext
    {
        public StudentContext() : base("DBConnection") { }
        public DbSet<Student> Students { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}
