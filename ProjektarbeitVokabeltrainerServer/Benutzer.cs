using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Web;

namespace ProjektarbeitVokabeltrainerServer
{
    [DataContract]
    public class Benutzer
    {
        private int id;
        [DataMember]
        public int ID { get { return id; } set { id = value; } }
        private string username;
        [DataMember]
        public string Username { get { return username; } set { username = value; } }
        private string passwort;
        [DataMember]
        public string Passwort { get { return passwort; } set { passwort = value; } }
        private Fach fach1;
        [DataMember]
        public Fach Fach1 { get { return fach1; } set { fach1 = value; } }
        private Fach fach2;
        [DataMember]
        public Fach Fach2 { get { return fach2; } set { fach2 = value; } }
        private Fach fach3;
        [DataMember]
        public Fach Fach3 { get { return fach3; } set { fach3 = value; } }
        private Fach fach4;
        [DataMember]
        public Fach Fach4 { get { return fach4; } set { fach4 = value; } }
        private Teststatistik statistik;
        [DataMember]
        public Teststatistik Statistik { get { return statistik; } set { statistik = value; } }

        public Benutzer() { }

        public Benutzer(int id, string username, string passwort, Fach fach1, Fach fach2, Fach fach3, Fach fach4, Teststatistik statistik)
        {
            this.id = id;
            this.username = username;
            this.passwort = passwort;
            this.fach1 = fach1;
            this.fach2 = fach2;
            this.fach3 = fach3;
            this.fach4 = fach4;
            this.statistik = statistik;
        }
    }
}