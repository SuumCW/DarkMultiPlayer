using System;
using System.IO;

namespace DarkMultiPlayerServer
{
    public class NukeKSC
    {
        public static void RunNukeKSC(string commandText)
        {
            string[] vesselList = Directory.GetFiles(Path.Combine(Server.universeDirectory, "Vessels"));
            int numberOfRemovals = 0;
            foreach (string vesselFile in vesselList)
            {
                string vesselID = Path.GetFileNameWithoutExtension(vesselFile);
                bool landedAtKSC = false;
                bool landedAtRunway = false;
                using (StreamReader sr = new StreamReader(vesselFile))
                {
                    string currentLine = sr.ReadLine();
                    while (currentLine != null && !landedAtKSC && !landedAtRunway)
                    {
                        string trimmedLine = currentLine.Trim();
                        if (trimmedLine.StartsWith("landedAt = "))
                        {
                            string landedAt = trimmedLine.Substring(trimmedLine.IndexOf("=") + 2);
                            if (landedAt == "KSC")
                            {
                                landedAtKSC = true;
                            }
                            if (landedAt == "Runway")
                            {
                                landedAtRunway = true;
                            }
                        }
                        currentLine = sr.ReadLine();
                    }
                }
                if (landedAtKSC | landedAtRunway)
                {
                    DarkLog.Normal("Removing vessel: " + vesselID);
                    File.Delete(vesselFile);
                    numberOfRemovals++;
                }
            }
            DarkLog.Normal("Nuked " + numberOfRemovals + " vessels around the KSC");
        }
    }
}
