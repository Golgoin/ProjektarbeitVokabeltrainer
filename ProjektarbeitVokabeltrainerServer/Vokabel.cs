using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ProjektarbeitVokabeltrainerServer
{
    [DataContract]
    public class Vokabel
    {
        private int id;
        [DataMember]
        public int ID { get { return id; } set { id = value; } }
        private int benutzerId;
        [DataMember]
        public int BenutzerId { get { return benutzerId; } set { benutzerId = value; } }
        private int fach;
        [DataMember]
        public int Fach { get { return fach; } set { fach = value; } }
        private string deutsch;
        [DataMember]
        public string Deutsch { get { return deutsch; } set { deutsch = value; } }
        private string deutsch2;
        [DataMember]
        public string Deutsch2 { get { return deutsch2; } set { deutsch2 = value; } }
        private string englisch;
        [DataMember]
        public string Englisch { get { return englisch; } set { englisch = value; } }
        private string englisch2;
        [DataMember]
        public string Englisch2 { get { return englisch2; } set { englisch2 = value; } }

        public Vokabel() { }

        public Vokabel(int id, int benutzerId, int fach, string deutsch, string deutsch2, string englisch, string englisch2)
        {
            this.id = id;
            this.benutzerId = benutzerId;
            this.fach = fach;
            this.deutsch = deutsch;
            this.deutsch2 = deutsch2;
            this.englisch = englisch;
            this.englisch2 = englisch2;
        }
    }
}