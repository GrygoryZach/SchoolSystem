namespace SchoolSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Student
{
    public string surname, name;
    public Student(string surname, string name)
    {
        this.surname = surname;
        this.name = name;
    }

    public string ReturnString()
    {
        return $"{surname} {name}";
    }
}

class Group
{
    Tuple<int, int> num;
    List<Student> students = new List<Student>();

    public Group(int grade, int id)
    {
        this.num = new Tuple<int, int>(grade, id);
    }

    public string GroupName()
    {
        return $"{num.Item1}-{num.Item2}";
    }

    void AddStudent(Student student)
    {
        students.Add(student);
    }

    public void PrintGroup()
    {
        Console.WriteLine(GroupName() + "\n-----");
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

    public void ReadFromStream()
    {
        Console.Write($"Type number of students in the group {GroupName()}: ");
        int studentsNum = int.Parse(Console.ReadLine());
        for (int i = 0; i < studentsNum; i++)
        {
            Console.Write($"Type {i + 1} student surname and name: ");
            string[] studentName = Console.ReadLine().Split(' ');
            students.Add(new Student(studentName[0], studentName[1]));
        }
        Console.WriteLine($"Group {GroupName()} with {studentsNum} students successfully created.\n");
    }

    public void ReadFromFile()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
        string filePath = Path.Combine(projectDirectory, $"{GroupName()}.txt");
        string[] textLines = File.ReadAllText(filePath).Split('\n');
        string[] groupName = textLines[0].Split('-');
        this.num = new Tuple<int, int>(int.Parse(groupName[0]), int.Parse(groupName[1]));
        foreach (string student in textLines.Skip(1).ToArray())
        {
            string[] studentData = student.Split(' ');
            AddStudent(new Student(studentData[0], studentData[1]));
        }
    }
    public void WriteToFile()
    {
        string textLines = GroupName();
        foreach (Student student in this.students)
        {
            textLines += $"\n{student.ReturnString()}";
        }
        string currentDirectory = Directory.GetCurrentDirectory();
        string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
        string filePath = Path.Combine(projectDirectory, $"{GroupName()}.txt");
        File.WriteAllText(filePath, textLines);
    }
}

internal class Program
{
    static int Main()
    {
        Console.WriteLine("start\n");
        Group group1 = new Group(2, 4);
        group1.ReadFromStream();
        group1.PrintGroup();
        group1.WriteToFile();
        return 0;
    }
}