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
        //Group group1 = new Group(2, 4);
        //group1.GenerateRandomGroup(20);
        //group1.PrintGroup();
        //group1.WriteToFile();

        using var db = new SchoolContext();

        // Note: This sample requires the database to be created before running.
        Console.WriteLine($"Database path: {db.DbPath}.");

        // Create
        Console.WriteLine("Inserting a new student");
        db.Add(new Teacher(Surname: "Jack", Name: "Saml", Id: 3));
        db.Add(new Student(Surname: "Mor", Name: "", Id: 4));
        db.SaveChanges();

        // Read
        Console.WriteLine("Querying for a blog");
        var student1 = db.Students
            .OrderBy(s => s.Id)
            .First();
        var teacher1 = db.Teachers
                    .OrderBy(s => s.Id)
                    .First();
        Console.WriteLine(student1.ReturnString());
        Console.WriteLine(teacher1.ReturnString());

        // Update
        Console.WriteLine("Updating the blog and adding a post");
        student1.Surname = "Carlson";
        teacher1.Subjects.Add(
            new Subject(Name: "Math", SubjectId: 1));
        db.SaveChanges();

        // Delete
        Console.WriteLine("Delete the blog");
        db.Remove(student1);
        db.SaveChanges();
        return 0;
    }
}