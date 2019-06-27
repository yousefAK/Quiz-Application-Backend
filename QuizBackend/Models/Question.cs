using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizBackend.Models
{
    public class Question
    {
        public int ID { get; set; }
        public String Text { get; set; }
        public String CorrectAnswer { get; set; }
        public String Answer1 { get; set; }
        public String Answer2 { get; set; }
        public String Answer3 { get; set; }

        public int QuizID { get; set; }

    }
}
