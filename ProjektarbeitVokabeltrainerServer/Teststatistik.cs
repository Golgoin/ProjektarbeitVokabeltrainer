using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ProjektarbeitVokabeltrainerServer
{
    [DataContract]
    public class Teststatistik
    {
        private int testGestartet;
        [DataMember]
        public int TestGestartet { get { return testGestartet; } set { testGestartet = value; } }
        private int testBeendet;
        [DataMember]
        public int TestBeendet { get { return testBeendet; } set { testBeendet = value; } }
        private int vokabelGeübt;
        [DataMember]
        public int VokabelGeübt { get { return vokabelGeübt; } set { vokabelGeübt = value; } }
        private int vokabelRichtig;
        [DataMember]
        public int VokabelRichtig { get { return vokabelRichtig; } set { vokabelRichtig = value; } }

        public Teststatistik() { }

        public Teststatistik(int testGestartet, int testBeendet, int vokabelGeübt, int vokabelRichtig)
        {
            this.testGestartet = testGestartet;
            this.testBeendet = testBeendet;
            this.vokabelGeübt = vokabelGeübt;
            this.vokabelRichtig = vokabelRichtig;
        }
    }
}