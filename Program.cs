namespace SchoolSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
    public int id;
    public string surname, name;

    public Person(string surname, string name, int id)
    {
        this.surname = surname;
        this.name = name;
        this.id = id;
    }

    public string ReturnString(bool id_needed = false)
    {
        return $"{surname} {name}{(id_needed ? " " + id : "")}";
    }
}

class Student : Person
{
    public int groupId { get; set; }
    public Group group { get; set; }
    public Student(string surname, string name, int id) : base(surname, name, id) { }
}
class Teacher : Person
{
    public List<Subject> subjects;
    public Teacher(string surname, string name, int id, List<Subject> subjects) : base(surname, name, id)
    {
        this.subjects = subjects;
    }

}

class Group
{
    public int id;
    public int grade, index;
    List<Student> students = new List<Student>();

    public Group(int grade, int index)
    {
        this.grade = grade;
        this.index = index;
    }

    public string ReturnString()
    {
        return $"{grade}-{index}";
    }

    void AddStudent(Student student)
    {
        students.Add(student);
    }

    public void PrintGroup()
    {
        Console.WriteLine(this.ReturnString() + "\n-----");
        var nameCount = new Dictionary<string, int>();
        foreach (Student student in students)
        {
            string fullName = student.ReturnString();
            if (students.FindAll(x => x.surname == student.surname && x.name == student.name).Count > 1)
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
            students.Add(new Student(studentName[0], studentName[1], int.Parse(studentName[2])));
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
        this.grade = int.Parse(groupName[0]);
        this.index = int.Parse(groupName[1]);
        foreach (string student in textLines.Skip(1).ToArray())
        {
            string[] studentData = student.Split(' ');
            AddStudent(new Student(studentData[0], studentData[1], int.Parse(studentData[2])));
        }
    }
    public void WriteToFile()
    {
        string textLines = this.ReturnString();
        foreach (Student student in this.students)
        {
            textLines += $"\n{student.ReturnString(id_needed:true)}";
        }
        string currentDirectory = Directory.GetCurrentDirectory();
        string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
        string filePath = Path.Combine(projectDirectory, $"{this.ReturnString()}.txt");
        File.WriteAllText(filePath, textLines);
    }
}

internal class Program
{
    static int Main()
    {
        Group group1 = new Group(2, 4);
        group1.GenerateRandomGroup(20);
        group1.PrintGroup();
        group1.WriteToFile();
        return 0;
    }
}