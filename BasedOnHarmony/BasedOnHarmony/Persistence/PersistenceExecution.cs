using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasedOnHarmony.Persistence
{
    class PersistenceExecution
    {
        public List<int> Efos;
        public List<double> MediaF;
        public List<double> Times;
        public int SuccesRate;
        public int NumRep;

        public PersistenceExecution(int numRep) {
            NumRep = 0;
        }

        public PersistenceExecution(int numRep, List<int> efos, List<double> mediaF, List<double> times, int succesRate)
        {
            Efos = efos;
            MediaF = mediaF;
            Times = times;
            SuccesRate = succesRate;
            NumRep = numRep;
        }

        public string getPathDirectory() {
            var RootDirectorData = Path.GetFullPath("..\\..\\data_persistence\\");
            if (!Directory.Exists(RootDirectorData))
            {
                DirectoryInfo di = Directory.CreateDirectory(RootDirectorData);
            }
            return RootDirectorData;
        }

        public void PersistirEjecusionProblema()
        {
            var RootDirectoryData = getPathDirectory();
            using (StreamWriter writer = new StreamWriter(RootDirectoryData + "Execution.txt", false))
            {
                writer.WriteLine(NumRep + ";" + SuccesRate + ";");
                for (var a = 0; a < NumRep; a++)
                {
                    writer.WriteLine(Efos[a] + ";" + MediaF[a] +";"+ Times[a] + ";");
                }
            }
        }
        public void  LoadPersistenceExecute()
        {
            var RootDirectorData = Path.GetFullPath("..\\..\\data_persistence\\Execution.txt");
            if (File.Exists(RootDirectorData))
            {
                var lines = File.ReadAllLines(RootDirectorData);
                var firstline = lines[0].Split(';');
                NumRep = int.Parse(firstline[1]);
                SuccesRate = int.Parse(firstline[2]);
                var positionLine = 1;
                for (var i = 0; i < 3; i++)
                {
                    var line = lines[positionLine++].Split(';');
                    Efos.Add(int.Parse(line[1]));
                    MediaF.Add(double.Parse(line[2]));
                    Times.Add(double.Parse(line[3]));
                }
            }

        }
        public void printObjet()
        {
            Console.WriteLine(this.NumRep);
            Console.WriteLine(this.SuccesRate);
            printAux(MediaF);
            printAux(Times);
        }
        private void  printAux( List<double> list) {
            for (var a = 0; a < NumRep; a++)
            {
                Console.Write(list[a]+";");
            }
        }

    }
}
