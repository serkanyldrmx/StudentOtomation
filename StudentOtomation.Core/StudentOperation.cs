using System;
using System.IO;
using StudentOtomation.Type;
using static System.Net.Mime.MediaTypeNames;

namespace StudentOtomation.Core
{
    public class StudentOperation
    {
        /// <summary>
        /// Öğrenci listesi döndürme fonksiyonu
        /// </summary>
        /// <returns></returns>
        public List<Student> GetStudens()
        {
            var returnObject = new List<Student>();

            using (var reader = new StreamReader(Common.studentFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line != null)
                    {
                        Int32 studentNumber = 0;
                        string[] Information = line.Split(' ');

                        if (Information.Length != 3
                            || string.IsNullOrWhiteSpace(Information[2])
                            || !Int32.TryParse(Information[2], out studentNumber))
                        {
                            return null;
                        }
                        var student = new Student()
                        {
                            Name = Information[0],
                            Surname = Information[1],
                            No = studentNumber
                        };
                        returnObject.Add(student);
                    }
                }
            }
            return returnObject;
        }

        /// <summary>
        /// Öğrenci ekleme fonksiyonu
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool StudentAdd(Student item)
        {
            using (StreamWriter write = new StreamWriter(Common.studentFilePath, true))
            {
                var text = string.Concat(item.Name, Common.Space , item.Surname, Common.Space, item.No);
                write.WriteLine(text);
            }
            return true;
        }

        /// <summary>
        /// Öğrenci silme fonksiyonu 
        /// </summary>
        /// <param name="studentNumberText"></param>
        /// <returns></returns>
        public bool StudentDelete(Int32 studentNumber)
        {
            var coreExam = new ExamOperation();
            List<Student> studentList = GetStudens();
            List<StudentExam> examList = coreExam.GetExams();

            if (studentList == null || studentList.Count == 0)
            {
                return false;
            }
            Student? studentDelete = studentList?.Find(x => x.No == studentNumber);
            StudentExam examDelete = examList?.Find(x => x.Number == studentNumber);
            if (studentDelete == null || examDelete==null)
                return false;
            else
            {
                studentList.Remove(studentDelete);
                examList.Remove(examDelete);

                using (StreamWriter write = new StreamWriter(Common.studentFilePath)) { }
                using (StreamWriter write = new StreamWriter(Common.examFilePath)) { }

                foreach (var student in studentList)
                {
                    using (StreamWriter write = new StreamWriter(Common.studentFilePath, true))
                    {
                        var text = string.Concat(student.Name, Common.Space, student.Surname, Common.Space, student.No);
                        write.WriteLine(text);
                    }
                }
                foreach (var exam in examList)
                {
                    using (StreamWriter write = new StreamWriter(Common.examFilePath, true))
                    {
                        var text = string.Concat(exam.Number, Common.Space, exam.Midterm, Common.Space, exam.Final, Common.Space, exam.Average);
                        write.WriteLine(text);
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Öğrenci ad soyad güncelleme.
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public bool StudentUpdate(Student student)
        {
            List<Student> studentList = GetStudens();
            if (studentList == null || studentList.Count == 0)
            {
                return false;
            }
            var student2 =studentList.Find(x => x.No == student.No);
            student2.Name = student.Name;
            student2.Surname = student.Surname;
            
            using (StreamWriter writeing = new StreamWriter(Common.studentFilePath))

            foreach (var item in studentList)
            {
                using (StreamWriter write = new StreamWriter(Common.studentFilePath,true))
                {
                    var text = string.Concat(item.Name,Common.Space, item.Surname, Common.Space, item.No);
                    write.WriteLine(text);
                }
            }
            return true;
        }
    }
}


