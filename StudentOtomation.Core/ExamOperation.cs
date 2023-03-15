using System;
using StudentOtomation.Type;

namespace StudentOtomation.Core
{
	public class ExamOperation
	{
        /// <summary>
        /// Öğrenci notlarını TXT dosyasına yazdırma fonksiyonu
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
		public bool SaveExamGrade(StudentExam exam)
		{
            var returnObject = new List<StudentExam>();

            string FilePats = Common.examFilePath;

            using (StreamWriter write = new StreamWriter(FilePats, true))
            {
                var text = string.Concat(exam.Number, Common.Space, exam.Midterm, Common.Space, exam.Final, Common.Space, exam.Average);
                write.WriteLine(text);
            }
            return true;
        }

        /// <summary>
        /// Öğrenci notlarını TXT dosyasından okuyup liste döndürme fonksiyonu
        /// </summary>
        /// <returns></returns>
        public List<StudentExam> GetExams()
        {
            var returnObject = new List<StudentExam>();

            using (var reader = new StreamReader(Common.examFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line != null)
                    {
                        Int32 studentNumber = 0;
                        Int32 studentMidterm = 0;
                        Int32 stundentFinal = 0;
                        double avarage = 0;
                        string[] Information = line.Split(' ');

                        if (Information.Length != 4
                            || !Int32.TryParse(Information[0], out studentNumber)
                            || !Int32.TryParse(Information[1], out studentMidterm)
                            || !Int32.TryParse(Information[2], out stundentFinal)
                            || !double.TryParse(Information[3], out avarage))
                        {
                            return null;
                        }
                        var studentExam1 = new StudentExam()
                        {
                            Number = studentNumber,
                            Midterm = studentMidterm,
                            Final = stundentFinal,
                            Average = avarage
                        };
                        returnObject.Add(studentExam1);
                    }
                }
            }
            return returnObject;
        }

        /// <summary>
        /// Öğrenci sinav notu güncelleme metodu
        /// </summary>
        /// <param name="examUpdate"></param>
        /// <param name="exam"></param>
        /// <returns></returns>
        public bool ExamUpdate(Int32 examUpdate,StudentExam exam)
        {
            List<StudentExam> examList = GetExams();
            StudentExam? ExamUpdate = examList?.Find(x => x.Number == examUpdate);
            if (ExamUpdate == null)
            {
                return false;
            }
                
            ExamUpdate.Midterm = exam.Midterm;
            ExamUpdate.Final = exam.Final;
            ExamUpdate.Average = exam.Average;

            string FilePats = Common.examFilePath;
            using (StreamWriter write = new StreamWriter(FilePats)) { }
                foreach (var item in examList)
            {
                using (StreamWriter write = new StreamWriter(FilePats, true))
                {
                    var text = string.Concat(item.Number, Common.Space, item.Midterm, Common.Space, item.Final, Common.Space, item.Average);
                    write.WriteLine(text);
                }
            }
            return true;
        }
    }
}

