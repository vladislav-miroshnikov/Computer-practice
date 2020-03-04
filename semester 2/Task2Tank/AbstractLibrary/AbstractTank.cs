using System;

namespace AbstractLibrary
{
    public abstract class AbstractTank
    {
        public string title { get; set; }
        public string nation { get; set; }
        public int yearOfIssue { get; set; }
        public int crew { get; set; }
        public float weight { get; set; }
        public float gunCaliber { get; set; }

        protected AbstractTank(string _title, string _nation, int _yearOfIssue, int _crew, float _weight, float _gunCaliber)
        {
            title = _title;
            nation = _nation;
            yearOfIssue = _yearOfIssue;
            crew = _crew;
            weight = _weight;                               
            gunCaliber = _gunCaliber;
        }

        public virtual void GetInfo()
        {
            Console.WriteLine($"Name of the tank: {title}\nCountry of origin: {nation}\nYear of issue: {yearOfIssue}\n" +
                $"Crew: {crew} persons\nTank weight : {weight} tons\nGun caliber: {gunCaliber} mm");
        }

        
    }
}
