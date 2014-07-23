using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ProjektarbeitVokabeltrainerServer
{
    [DataContract]
    public class Fach
    {
        private int id;
        [DataMember]
        public int ID { get { return id; } set { id = value; } }
        private string zeit;
        [DataMember]
        public string Zeit { get { return zeit; } set { zeit = value; } }

        public Fach() { }

        public Fach(int id, string zeit)
        {
            this.id = id;
            this.zeit = zeit;
        }
    }
}