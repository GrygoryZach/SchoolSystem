namespace SchoolSystem;
using System;
using System.Collections.Generic;
using System.IO;

class Student
{
    public string surname, name;
    public Student(string surname, string name)
    {
        this.surname = surname;
        this.name = name;
    }

    public void PrintStudent()
    {
        Console.WriteLine(surname + ' ' + name);
    }
}

class Class
{
    Tuple<int, int> num;
    List<Student> students = new List<Student>();

    public Class(int grade, int id)
    {
        this.num = new Tuple<int, int>(grade, id);
    }

    public string ClassName()
    {
        return $"{num.Item1}-{num.Item2}";
    }

    void AddStudent(Student student)
    {
        students.Add(student);
    }

    public void PrintClass()
    {
        Console.WriteLine(ClassName());
        foreach (Student student in students)
        {
            student.PrintStudent();
        }
    }

    public void ReadFromFile()
    {
        string[] textLines = File.ReadAllText($"{ClassName()}.txt").Split('\n');
        string[] className = textLines[0].Split('-');
        this.num = new Tuple<int, int>(int.Parse(className[0]), int.Parse(className[1]));
        foreach (string student in textLines)
        {
            string[] studentName = student.Split(' ');
            students.Add(new Student(studentName[0], studentName[1]));
        }
    }

    public void ReadFromStream()
    {
        Console.Write($"Type number of students in the class {ClassName()}: ");
        int studentsNum = int.Parse(Console.ReadLine());
        for (int i = 0; i < studentsNum; i++)
        {
            Console.Write($"Type {i + 1} student surname and name: ");
            string[] studentName = Console.ReadLine().Split(' ');
            students.Add(new Student(studentName[0], studentName[1]));
        }
        Console.WriteLine($"Class {ClassName()} with {studentsNum} students successfully created.\n");
    }

    public void WriteToFile()
    {
        string textLines = ClassName() + '\n';
        foreach (Student student in this.students)
        {
            textLines += $"{student.surname} {student.name}\n";
        }
        File.WriteAllText($"{ClassName()}.txt", textLines);
    }
}

internal class Program
{
    static int Main()
    {
        Console.WriteLine("start\n");
        Class class1 = new Class(2, 4);
        class1.ReadFromStream();
        class1.PrintClass();
        class1.WriteToFile();
        return 0;
    }
}