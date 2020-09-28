using System;

namespace AbstractLibrary
{
    public abstract class AbstractTank
    {
        public string Title { get; protected set; }
        public string Nation { get; protected set; }
        public int YearOfIssue { get; protected set; }
        public int Crew { get; protected set; }
        public float Weight { get; protected set; }
        public float GunCaliber { get; protected set; }

        protected AbstractTank(string title, string nation, int yearOfIssue, int crew, float weight, float gunCaliber)
        {
            Title = title;
            Nation = nation;
            YearOfIssue = yearOfIssue;
            Crew = crew;
            Weight = weight;                               
            GunCaliber = gunCaliber;
        }

        public virtual string GetInfo()
        {
            string a = ($"Name of the tank: {Title}\nCountry of origin: {Nation}\nYear of issue: {YearOfIssue}\n" +
                $"Crew: {Crew} persons\nTank weight : {Weight} tons\nGun caliber: {GunCaliber} mm");
            Console.WriteLine(a);
            return a;
        }

        
    }
}
