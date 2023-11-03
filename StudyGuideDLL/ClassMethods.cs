using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyGuideDLL
{
    //contains calendar emthods
    public class ClassMethods
    {
        //calculates and returns the study hours for the module
        public int totStudyHours(int modCredits) { return modCredits * 10; }
        //calculates and returns the study hours required by the user weekly
        public double weeklyHours(double credits, double numWeeks, double classHrs)
        {
            double totHrs = credits * 10;
            double totClassHrs = classHrs * numWeeks;
            return (totHrs - totClassHrs) / numWeeks;
        }
    }
}
