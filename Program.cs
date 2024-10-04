namespace SchoolSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;

internal class Program
{
    static int Main()
    {
        using var dbContext = new SchoolContext();
        dbContext.Initialize();

        Console.WriteLine($"Database path: {dbContext.DbPath}.");

        Console.WriteLine("Inserting new student and teacher");
        dbContext.Add(new Teacher(Surname: "Morrison", Name: "Jim", Id: 28533));
        dbContext.Add(new Student(Surname: "Lennon", Name: "John", Id: 34556));
        dbContext.SaveChanges();

        Console.WriteLine("Querying");
        var student1 = dbContext.Students
            .OrderBy(s => s.Id)
            .First();
        var teacher1 = dbContext.Teachers
                    .OrderBy(s => s.Id)
                    .First();
        Console.WriteLine($"First student: {student1.ReturnString()}");
        Console.WriteLine($"First teacher: {teacher1.ReturnString()}");

        Console.WriteLine("Editing");
        student1.Surname = "Carlson";
        var subject1 = new Subject(Name: "Mathematics");
        teacher1.Subjects.Add(subject1);
        dbContext.SaveChanges();
        Console.WriteLine("------\nFinished successfully");
        return 0;
    }
}