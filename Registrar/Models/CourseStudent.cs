namespace Registrar.Models
{
  public class CourseStudent
  {
    // properties, constructors, methods, etc. go here
    public int CourseStudentId { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public bool Status { get; set; }
  }
}
// class student : name, date of enroll 1
// class course : name, course number, courseId 3
// class student_course : studentId, courseId