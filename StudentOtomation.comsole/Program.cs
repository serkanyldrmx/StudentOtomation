using System.Xml.Linq;
using Microsoft.VisualBasic;
using StudentOtomation.Core;
using StudentOtomation.Type;
namespace StudentOtomation.comsole;


class Program
{
    static void Main(string[] args)
    {
        var coreStudent = new StudentOperation();
        var coreExam = new ExamOperation();

        while (true)
        {
            Console.WriteLine("*****MENU*****");
            Console.WriteLine("1- Ogrencileri Listele");
            Console.WriteLine("2- Ogrenci Ekleme");
            Console.WriteLine("3- Ogrenci Ve Not Silme");
            Console.WriteLine("4- Ogrenci Guncelle");
            Console.WriteLine("5- Ders Notlarini Gir");
            Console.WriteLine("6- Yeni Ogrenci Ders Notu Gir");
            Console.WriteLine("7- Ders Notları Guncelleme");
            Console.WriteLine("8- Ogrencilerin Ders Notlarini Listele");
            Console.WriteLine("9- Dersten Kalanlar");
            Console.WriteLine("10- Dersten Gecenler");
            Console.WriteLine("11- Cikis");

            string Choose = Console.ReadLine();
            int choose = Convert.ToInt32(Choose);

            switch (choose)
            {
                #region Öğrenci listeleme seçeneği.
                case 1:
                    {
                        List<Student> list = coreStudent.GetStudens();
                        if (list == null)
                        {
                            Console.WriteLine("liste bos veya cevirilemedi...");
                            break;
                        }

                        foreach (var item in list)
                        {
                            Console.Write(item.Name + " ");
                            Console.Write(item.Surname + " ");
                            Console.Write(item.No);
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                        break;
                    }
                #endregion

                #region Öğrenci ekleme seçeneği.
                case 2:
                    {
                        Console.WriteLine("Ogrenci adini giriniz :");
                        string studentName = Console.ReadLine();
                        Console.WriteLine("Ogrenci soyadini giriniz :");
                        string Surname = Console.ReadLine();
                        Console.WriteLine("Ogrenci No'su giriniz :");
                        string studentNumberText = Console.ReadLine();
                        Int32 studentNumber = 0;

                        if (string.IsNullOrWhiteSpace(studentNumberText)
                            || !Int32.TryParse(studentNumberText, out studentNumber))
                        {
                            Console.WriteLine("Numarayı Int'e cevirme hatasi.");
                            break;
                        }
                        int spaceCount = studentName.Split().Length - 1;

                        if (spaceCount==1)
                        {
                            string[] name = studentName.Split(' ');
                            studentName = name[0] + Common.Underscore + name[1];
                        }
                        if(spaceCount==2)
                        {
                            string[] name = studentName.Split(' ');
                            studentName = name[0] + Common.Underscore + name[1] + Common.Underscore + name[2];
                        }

                        var student = new Student()
                        {
                            Name = studentName,
                            Surname = Surname,
                            No = studentNumber
                        };
                        if(student==null)
                        {
                            Console.WriteLine("Ogrenci bos tekrar deneyin.");
                            break;
                        }
                        bool control = coreStudent.StudentAdd(student);
                        Console.WriteLine();
                        break;
                    }
                #endregion

                #region Öğrenci silme seçeneği
                case 3:
                    {
                        Console.WriteLine("Silmek istediğiniz Öğrencinin numarasını giriniz :");
                        string studentNumberText = Console.ReadLine();

                        Int32 studentNumber = 0;

                        if (string.IsNullOrWhiteSpace(studentNumberText)
                            || !Int32.TryParse(studentNumberText, out studentNumber))
                        {
                            Console.WriteLine("Numarayı Int'e cevirme hatasi.");
                            break;
                        }

                        bool control = coreStudent.StudentDelete(studentNumber);

                        if (control == false)
                        {
                            Console.WriteLine("liste bos veya yanlis numara girdiniz lutfen tekrar deneyin...");
                        }
                        else
                        {
                            Console.WriteLine("Ogrenci Basariyla silindi...");
                        }
                        Console.WriteLine();
                        break;
                    }
                #endregion

                #region Öğrenci güncelleme seçeneği
                case 4:
                    {
                        Console.WriteLine("Guncellemek istediginiz ogrenci numarasi giriniz :");
                        string studentNumberText = Console.ReadLine();

                        Console.WriteLine("Ogrenci adini giriniz :");
                        string Name = Console.ReadLine();
                        Console.WriteLine("Ogrenci soyadini giriniz :");
                        string Surname = Console.ReadLine();

                        Int32 studentNumber = 0;
                        if (string.IsNullOrWhiteSpace(studentNumberText)
                            || !Int32.TryParse(studentNumberText, out studentNumber))
                        {
                            Console.WriteLine("Numarayı Int'e cevirme hatasi.");
                            break;
                        }
                        var student = new Student()
                        {
                            Name = Name,
                            Surname = Surname,
                            No = studentNumber
                        };
                        bool control = coreStudent.StudentUpdate(student);

                        if(control==false)
                        {
                            Console.WriteLine("Liste Bos Veya Ogrenci Bulunamadi...");
                        }
                        else
                        {
                            Console.WriteLine("Ogrenci Basariyla Güncellendi...");
                            Console.WriteLine();
                        }
                        break;
                    }
                #endregion

                #region Öğrenci ders notlari girme seceneği
                case 5:
                    {
                        List<Student> studentList = coreStudent.GetStudens();
                        
                        if (studentList == null || studentList.Count == 0)
                        {
                            Console.WriteLine("liste bos.");
                            break;
                        }
                     
                        foreach (var item in studentList)
                        {
                            Console.WriteLine(item.Name+Common.Space+item.Surname+" vize notunu giriniz :");
                            string Midterm = Console.ReadLine();
                            Console.WriteLine(item.Name+ Common.Space+ item.Surname+ " final notunu giriniz :");
                            string Final = Console.ReadLine();

                            Int32 midterm = 0;
                            Int32 final = 0;

                            if ( !Int32.TryParse(Midterm, out midterm)
                            || !Int32.TryParse(Final, out final))
                            {
                                break;
                            }
                            var exam = new StudentExam();
                            exam.Number = item.No;
                            exam.Midterm = midterm;
                            exam.Final = final;
                            exam.Average = (exam.Midterm * 0.4) + (exam.Final * 0.6);

                            bool control = coreExam.SaveExamGrade(exam);
                            if(control==true)
                            {
                                Console.WriteLine("kayit başariyla listeye eklendi...");
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine("kayit listeye eklenemedi...");
                                Console.WriteLine();
                            }
                        }
                        break;
                    }
                #endregion

                #region Yeni girilen öğrenci ders notlari girme
                case 6:
                    {
                        List<Student> studentList = coreStudent.GetStudens();
                        List<StudentExam> examList = coreExam.GetExams();

                        if (studentList == null || studentList.Count == 0 || examList.Count==0)
                        {
                            Console.WriteLine("liste bos.");
                            break;
                        }
                        Console.WriteLine("Notunu girmek istediginiz ogrenci numarasını giriniz :");
                        string studentNumber = Console.ReadLine();
                        Int32 StudentNumber = 0;

                        if(studentNumber==null 
                           || !Int32.TryParse(studentNumber, out StudentNumber))
                        {
                            Console.WriteLine("Hatali giris...");
                            break;
                        }
                        Student? student = studentList?.Find(x => x.No == StudentNumber);

                        Console.WriteLine(student.Name + Common.Space + student.Surname + " vize notunu giriniz :");
                        string Midterm = Console.ReadLine();
                        Console.WriteLine(student.Name + Common.Space + student.Surname + " final notunu giriniz :");
                        string Final = Console.ReadLine();

                        var exam = new StudentExam();
                        exam.Number = StudentNumber;
                        exam.Midterm = Convert.ToInt32(Midterm);
                        exam.Final = Convert.ToInt32(Final);
                        exam.Average = (exam.Midterm * 0.4) + (exam.Final * 0.6);

                        bool control = coreExam.SaveExamGrade(exam);
                        if(control==true)
                            Console.WriteLine("Ogrenci basariyla eklendi.");
                        else
                            Console.WriteLine("Ogrenci eklenemedi.");
                        break;
                    }
                #endregion

                #region Ders Notlari güncelleme
                case 7:
                    {
                        List<Student> studentList = coreStudent.GetStudens();

                        if (studentList == null || studentList.Count == 0 )
                        {
                            Console.WriteLine("Ogrenci listesi bos.");
                            break;
                        }
                        Console.WriteLine("Notunu guncellemek istediginiz ogrenci numarasını giriniz :");
                        string ExamUpdate = Console.ReadLine();
                        Int32 examUpdate = 0;

                        if (ExamUpdate == null
                           || !Int32.TryParse(ExamUpdate, out examUpdate))
                        {
                            Console.WriteLine("Hatali giris...");
                            break;
                        }
                        Student? student = studentList?.Find(x => x.No == examUpdate);

                        Console.WriteLine(student.Name + Common.Space + student.Surname + " vize notunu giriniz :");
                        string Midterm = Console.ReadLine();
                        Console.WriteLine(student.Name + Common.Space + student.Surname + " final notunu giriniz :");
                        string Final = Console.ReadLine();

                        Int32 midterm = 0;
                        Int32 final = 0;

                        if (!Int32.TryParse(Midterm, out midterm)
                        || !Int32.TryParse(Final, out final))
                        {
                            break;
                        }
                        var exam = new StudentExam();
                        exam.Number = examUpdate;
                        exam.Midterm = midterm;
                        exam.Final = final;
                        exam.Average = (exam.Midterm * 0.4) + (exam.Final * 0.6);

                       bool control = coreExam.ExamUpdate(examUpdate,exam);
                        if (control == true)
                            Console.WriteLine("Ogrenci basariyla guncellendi.");
                        else
                            Console.WriteLine("Ogrenci guncellenemedi.");
                        break;

                    }
                #endregion

                #region Oğrencilerin ders notlarini listeleme seceneği
                case 8:
                    {
                        List<StudentExam> examList = coreExam.GetExams();
                        List<Student> studentList = coreStudent.GetStudens();

                        if(examList==null || studentList==null)
                        {
                            Console.WriteLine("Liste bos.");
                            break;
                        }

                        foreach (var student in studentList)
                        {
                            foreach (var exam in examList)
                            {
                                StudentExam? newExam = examList?.Find(x => x.Number == student.No);
                                string studentName = student.No + Common.Space + student.Name + Common.Space + student.Surname+ Common.Space+"Vize :" + newExam.Midterm + Common.Space +"Final :" +newExam.Final + Common.Space +"Ortalama :"+ newExam.Average;
                                Console.WriteLine(studentName);
                                Console.WriteLine();
                                break;
                            }
                        }
                        Console.WriteLine();

                        break;
                    }
                #endregion

                #region Dersten kalan öğrencileri listeleme seceneği
                case 9:
                    {
                        List<StudentExam> examList = coreExam.GetExams();
                        List<Student> studentList = coreStudent.GetStudens();

                        if (examList == null || studentList == null)
                        {
                            Console.WriteLine("Liste bos.");
                            break;
                        }

                        Console.WriteLine("*****Dersten kalan ogrenciler*****");
                        foreach(var item in examList)
                        {
                            if (item.Average<=50)
                            {
                                Student? student = studentList?.Find(x => x.No == item.Number);
                                var text = string.Concat(student.Name, Common.Space, student.Surname, Common.Space, student.No, Common.Space,"Ortalama :", item.Average);
                                Console.WriteLine(text);
                            }
                        }
                        Console.WriteLine();
                        break;
                    }
                #endregion

                #region Dersten gecen öğrencileri listeleme seceneği
                case 10:
                    {
                        {
                            List<StudentExam> examList = coreExam.GetExams();
                            List<Student> studentList = coreStudent.GetStudens();

                            if (examList == null || studentList == null)
                            {
                                Console.WriteLine("Liste bos.");
                                break;
                            }

                            Console.WriteLine("*****Dersten gecen ogrenciler*****");
                            foreach (var exam in examList)
                            {
                                if (exam.Average > 50)
                                {
                                    Student? student = studentList?.Find(x => x.No == exam.Number);
                                    var text = string.Concat(student.Name, Common.Space, student.Surname, Common.Space, student.No, Common.Space, "Ortalama :", exam.Average);
                                    Console.WriteLine(text);
                                }
                            }
                            Console.WriteLine();
                            break;
                        }
                    }
                #endregion

                #region Uygulamadan çıkış yapma seceneği
                case 11:
                    {
                        Console.WriteLine("Cikis yapiliyor...");
                        Thread.Sleep(2000);
                        return;
                    }
                #endregion
                default:
                    break;
            }
        }
    }
}

