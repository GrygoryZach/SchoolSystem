using Microsoft.EntityFrameworkCore;
using SchoolSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;



class Subject
{
    public int id;
    public string name;
    public Subject(string name, int id)
    {
        this.name = name;
        this.id = id;
    }
}

abstract class Person
{
    public int PersonId { get; set; }
    public string Surname { get; set; }
    public string Name { get; set; }

    public Person(string Surname, string Name, int PersonId)
    {
        this.Surname = Surname;
        this.Name = Name;
        this.PersonId = PersonId;
    }

    public string ReturnString(bool id_needed = false)
    {
        return $"{Surname} {Name}{(id_needed ? " " + PersonId : "")}";
    }
}

class Student : Person
{
    public int GroupId { get; set; }
    public Group Group { get; set; }
    public Student(string Surname, string Name, int PersonId) : base(Surname, Name, PersonId) { }
}

class Teacher : Person
{
    public List<Student> Subjects { get; } = new();
    public Teacher(string Surname, string Name, int PersonId) : base(Surname, Name, PersonId) { }
}

class Group
{
    public int GroupId { get; set; }
    public List<Student> Students { get; } = new();

    private int Grade { get; set; }
    private int Index { get; set; }

    public Group(int Grade, int Index)
    {
        this.Grade = Grade;
        this.Index = Index;
    }

    public string ReturnString()
    {
        return $"{Grade}-{Index}";
    }

    void AddStudent(Student student)
    {
        Students.Add(student);
    }

    public void PrintGroup()
    {
        Console.WriteLine(this.ReturnString() + "\n-----");
        var nameCount = new Dictionary<string, int>();
        foreach (Student student in Students)
        {
            string fullName = student.ReturnString();
            if (Students.FindAll(x => x.Surname == student.Surname && x.Name == student.Name).Count > 1)
            {
                if (!nameCount.ContainsKey(student.ReturnString()))
                    nameCount[fullName] = 1;
                else
                    nameCount[fullName]++;
                Console.WriteLine($"{fullName} ({nameCount[fullName]})");
            }
            else
            {
                Console.WriteLine(fullName);
            }
        }
    }

    public void GenerateRandomGroup(int students_num)
    {
        string[] surnames = {
            "Smith", "Anderson", "Clark", "Wright", "Mitchell",
            "Johnson", "Thomas", "Rodriguez", "Lopez", "Perez",
            "Williams", "Jackson", "Lewis", "Hill", "Roberts",
            "Jones", "White", "Lee", "Scott", "Turner",
            "Brown", "Harris", "Walker", "Green", "Phillips"

        };
        string[] names =  {
            "Linda", "Susan", "Karen", "Carol", "Sarah",
            "Barbara", "Margaret", "Betty", "Ruth", "Mary",
            "Robert", "Charles", "Paul", "Steven", "Kevin",
            "Michael", "Joseph", "Mark", "Edward", "Jason"
        };

        for (int i = 0; i < students_num; i++)
        {
            int i_nam = new Random().Next(names.Length - 1), i_sur = new Random().Next(surnames.Length - 1);
            AddStudent(new Student(surnames[i_sur], names[i_nam], new Random().Next()));
        }
    }

    public void ReadFromStream()
    {
        Console.Write($"Type number of students in the group {this.ReturnString()}: ");
        int studentsNum = int.Parse(Console.ReadLine());
        for (int i = 0; i < studentsNum; i++)
        {
            Console.Write($"Type {i + 1} student surname, name and ID: ");
            string[] studentName = Console.ReadLine().Split(' ');
            Students.Add(new Student(studentName[0], studentName[1], int.Parse(studentName[2])));
        }
        Console.WriteLine($"Group {this.ReturnString()} with {studentsNum} students successfully created.\n");
    }

    public void ReadFromFile()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
        string filePath = Path.Combine(projectDirectory, $"{this.ReturnString()}.txt");
        string[] textLines = File.ReadAllText(filePath).Split('\n');

        string[] groupName = textLines[0].Split('-');
        this.Grade = int.Parse(groupName[0]);
        this.Index = int.Parse(groupName[1]);
        foreach (string student in textLines.Skip(1).ToArray())
        {
            string[] studentData = student.Split(' ');
            AddStudent(new Student(studentData[0], studentData[1], int.Parse(studentData[2])));
        }
    }

    public void WriteToFile()
    {
        string textLines = this.ReturnString();
        foreach (Student student in this.Students)
        {
            textLines += $"\n{student.ReturnString(id_needed: true)}";
        }
        string currentDirectory = Directory.GetCurrentDirectory();
        string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
        string filePath = Path.Combine(projectDirectory, $"{this.ReturnString()}.txt");
        File.WriteAllText(filePath, textLines);
    }
}

public class SchoolContext : DbContext
{
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Group> Groups { get; set; }

    public string DbPath { get; }

    public SchoolContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "school.db");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasOne(s => s.Group)
            .WithMany(c => c.Students)
            .HasForeignKey(s => s.GroupId);
    }
}
