using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASB.Data
{
    internal class PersonalInformation
    {
        public string? FullName { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public string? MedicalConditions { get; set; }

        public PersonalInformation()
        {
            // Default constructor
        }

        public PersonalInformation(string fullName, int age, string gender, double weight, double height, string medicalConditions)
        {
            FullName = fullName;
            Age = age;
            Gender = gender;
            Weight = weight;
            Height = height;
            MedicalConditions = medicalConditions;
        }
    }


}
