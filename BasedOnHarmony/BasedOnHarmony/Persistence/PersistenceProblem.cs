using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasedOnHarmony.Persistence
{

    class PersistenceProblem
    {
        public string FileProblem;
        public double Avg;
        public double Rpe;
        public double Desviation;
        public double EfosAvg;
        public double PorcSucces;
        public double TimeAvg;
        public double TimeReal;

        public PersistenceProblem(string fileproblem, double avg, double rpe, double desviation, double efosAvg, double porcSucces, double timeAvg, double timeReal)
        {
            FileProblem = fileproblem;
            Avg = avg;
            Rpe = rpe;
            Desviation = desviation;
            EfosAvg = efosAvg;
            PorcSucces = porcSucces;
            TimeAvg = timeAvg;
            TimeReal = timeReal;
        }


        public  void PersistirSolucionProblema()
        {
            var RootDirectorData = Path.GetFullPath("..\\..\\data_persistence\\");
            if (!Directory.Exists(RootDirectorData))
            {
                DirectoryInfo di = Directory.CreateDirectory(RootDirectorData);
            }

            using (StreamWriter writer = File.AppendText(RootDirectorData+"solution.txt"))
            {
                var linea = FileProblem + ";" + Avg + ";" + Rpe + ";" + Desviation + ";" + EfosAvg;
                linea = linea + ";" + PorcSucces + ";" + TimeAvg + ";" + TimeReal + ";";
                writer.WriteLine(linea);
            }
        }

        public void printObjet()
        {
            Console.WriteLine(this.FileProblem);
            Console.WriteLine(this.Avg);
            Console.WriteLine(this.EfosAvg);
            Console.WriteLine(this.Desviation);
            Console.WriteLine(this.Rpe);
            Console.WriteLine(this.PorcSucces);
            Console.WriteLine(this.TimeAvg);
            Console.WriteLine(this.TimeReal);
        
        }

    }


}
