using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using LogParser_1.Services.Menu;
using System.Globalization;

namespace LogParser_1
{
    internal class Program
    {       
        static void Main(string[] args)
        {
            Menu.Launch();


            //prei manipulaivimo padaryt kad resultus butu galima saugoti failuose
            //padaryt kazkoki tai failu sortinima kombinavima gal pagal severity ar kazka panasaus.
            //tarkim kad rodytu 5 dienu senumo tik
        }
    }
}
