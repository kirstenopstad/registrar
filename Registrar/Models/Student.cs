using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Registrar.Models
{
  public class Student
  {
    // properties, constructors, methods, etc. go here
    [Required(ErrorMessage = "Name must be entered!")]
    public string Name { get; set; }
    
    [Range(typeof(DateTime), "1/1/2023", "12/31/2023", ErrorMessage = "Registration is only open for calendar year 2023")]
    [Required(ErrorMessage = "Enter a valid date")]
    public Nullable<DateTime> EnrollmentDate { get; set; }
    public int StudentId { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
    public List<CourseStudent> JoinCourseStudents { get; } // collection navigation property
  }
}

// class student : name, date of enroll 1
// class course : name, course number, courseId 3
// class student_course : studentId, courseId

// As a registrar, I want to enter a student, so I can keep track of all students enrolled at this University. I should be able to provide a name and date of enrollment.
// As a registrar, I want to enter a course, so I can keep track of all of the courses the University offers. I should be able to provide a course name and a course number (ex. HIST100).
// As a registrar, I want to be able to assign students to a course, so that teachers know which students are in their course. A course can have many students and a student can take many courses at the same time.