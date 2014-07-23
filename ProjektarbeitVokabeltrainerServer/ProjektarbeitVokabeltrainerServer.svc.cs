using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;
using System.ServiceModel.Web;
using System.Security;
using System.Runtime.InteropServices;

namespace ProjektarbeitVokabeltrainerServer
{
    // HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Klassennamen "ProjektarbeitVokabeltrainerServer" sowohl im Code als auch in der SVC- und der Konfigurationsdatei ändern.
    // HINWEIS: Wählen Sie zum Starten des WCF-Testclients zum Testen dieses Diensts ProjektarbeitVokabeltrainerServer.svc oder ProjektarbeitVokabeltrainerServer.svc.cs im Projektmappen-Explorer aus, und starten Sie das Debuggen.
    public class ProjektarbeitVokabeltrainerServer : IProjektarbeitVokabeltrainerServer
    {
        string constring = "Server=(localdb)\\Projects; Integrated security=true; Database = MyDatabase";
        //string constring = "Server=(local)\\SQLEXPRESS; Integrated security=true; Database = MyDatabase";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;


        //Registriert einen neuen Benutzer
        public bool Registrieren(Benutzer benutzer)
        {
            using (con = new SqlConnection(constring))
            {
                try
                {
                    cmd = new SqlCommand("SELECT Benutzername FROM BenutzerDB", con);
                    con.Open();
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (benutzer.Username == (string)reader["Benutzername"])
                        {
                            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Conflict;
                            WebOperationContext.Current.OutgoingResponse.StatusDescription = "Benutzername bereits vergeben";
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return false;
                }
            }
            Guid guid = System.Guid.NewGuid();
            string hpw = Security.HashSHA1(benutzer.Passwort + guid.ToString()); //Hashwert aus Passworthash und GUID wird erstellt
            using (con = new SqlConnection(constring))
            {
                try
                {
                    cmd = new SqlCommand("INSERT INTO BenutzerDB (Benutzername, Passwort, Fach1, Fach2, Fach3, Fach4, TestGestartet, TestBeendet, VokabelGeübt, VokabelRichtig, Guid) VALUES (@benutzername, @passwort, @fach1, @fach2, @fach3, @fach4, @testgestartet, @testbeendet, @vokabelgeübt, @vokabelrichtig, @guid)", con);
                    cmd.Parameters.AddWithValue("@benutzername", benutzer.Username);
                    cmd.Parameters.AddWithValue("@passwort", hpw);
                    cmd.Parameters.AddWithValue("@fach1", DateTime.Now.AddDays(-1).ToString());
                    cmd.Parameters.AddWithValue("@fach2", DateTime.Now.ToString());
                    cmd.Parameters.AddWithValue("@fach3", DateTime.Now.ToString());
                    cmd.Parameters.AddWithValue("@fach4", DateTime.Now.ToString());
                    cmd.Parameters.AddWithValue("@testgestartet", 0);
                    cmd.Parameters.AddWithValue("@testbeendet", 0);
                    cmd.Parameters.AddWithValue("@vokabelgeübt", 0);
                    cmd.Parameters.AddWithValue("@vokabelrichtig", 0);
                    cmd.Parameters.AddWithValue("@guid", guid);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    return true;
                }
                catch (Exception)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return false;
                }
            }
        }

        //Überprüft die Anmeldedaten eines Benutzers und liefert diesen zurück
        public Benutzer Anmelden(string benutzerid, Benutzer benutzer)
        {
            using (con = new SqlConnection(constring))
            {
                try
                {
                    cmd = new SqlCommand("SELECT * FROM BenutzerDB WHERE ID = @benutzerid", con);
                    cmd.Parameters.AddWithValue("@benutzerid", Convert.ToInt32(benutzerid));
                    con.Open();
                    reader = cmd.ExecuteReader();
                    Benutzer benutzerNeu = new Benutzer();
                    while (reader.Read())
                    {
                        if (Security.HashSHA1(benutzer.Passwort + Convert.ToString(reader["Guid"])) == (string)reader["Passwort"])
                        {
                            benutzerNeu.ID = (int)reader["ID"];
                            benutzerNeu.Username = (string)reader["Benutzername"];
                            benutzerNeu.Fach1 = new Fach(1, (string)reader["Fach1"]);
                            benutzerNeu.Fach2 = new Fach(2, (string)reader["Fach2"]);
                            benutzerNeu.Fach3 = new Fach(3, (string)reader["Fach3"]);
                            benutzerNeu.Fach4 = new Fach(4, (string)reader["Fach4"]);
                            benutzerNeu.Statistik = new Teststatistik((int)reader["TestGestartet"], (int)reader["TestBeendet"], (int)reader["VokabelGeübt"], (int)reader["VokabelRichtig"]);
                            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                        }
                        else
                        {
                            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                            WebOperationContext.Current.OutgoingResponse.StatusDescription = "Passwort falsch!";
                        }
                    }
                    return benutzerNeu;
                }
                catch (Exception ex)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusDescription = ex.Message;
                    return new Benutzer();
                }
            }
        }

        //Liefert einen bestimmten Benutzer
        public Benutzer GetBenutzer(string benutzerid)
        {
            using (con = new SqlConnection(constring))
            {
                try
                {
                    cmd = new SqlCommand("SELECT * FROM BenutzerDB WHERE ID = @benutzerid", con);
                    cmd.Parameters.AddWithValue("@benutzerid", Convert.ToInt32(benutzerid));
                    con.Open();
                    reader = cmd.ExecuteReader();
                    Benutzer benutzerNeu = new Benutzer();
                    while (reader.Read())
                    {
                        benutzerNeu.ID = (int)reader["ID"];
                        benutzerNeu.Username = (string)reader["Benutzername"];
                        benutzerNeu.Fach1 = new Fach(1, (string)reader["Fach1"]);
                        benutzerNeu.Fach2 = new Fach(2, (string)reader["Fach2"]);
                        benutzerNeu.Fach3 = new Fach(3, (string)reader["Fach3"]);
                        benutzerNeu.Fach4 = new Fach(4, (string)reader["Fach4"]);
                        benutzerNeu.Statistik = new Teststatistik((int)reader["TestGestartet"], (int)reader["TestBeendet"], (int)reader["VokabelGeübt"], (int)reader["VokabelRichtig"]);
                    }
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    return benutzerNeu;
                }
                catch (Exception)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return new Benutzer();
                }
            }
        }

        //Liefert alle Benutzer
        public List<Benutzer> GetAllBenutzer()
        {
            using (con = new SqlConnection(constring))
            {
                try
                {
                    cmd = new SqlCommand("SELECT * FROM BenutzerDB", con);
                    con.Open();
                    reader = cmd.ExecuteReader();
                    List<Benutzer> BenutzerAlle = new List<Benutzer>();
                    while (reader.Read())
                    {
                        Benutzer benutzerNeu = new Benutzer();
                        benutzerNeu.ID = (int)reader["ID"];
                        benutzerNeu.Username = (string)reader["Benutzername"];
                        benutzerNeu.Fach1 = new Fach(1, (string)reader["Fach1"]);
                        benutzerNeu.Fach2 = new Fach(2, (string)reader["Fach2"]);
                        benutzerNeu.Fach3 = new Fach(3, (string)reader["Fach3"]);
                        benutzerNeu.Fach4 = new Fach(4, (string)reader["Fach4"]);
                        benutzerNeu.Statistik = new Teststatistik((int)reader["TestGestartet"], (int)reader["TestBeendet"], (int)reader["VokabelGeübt"], (int)reader["VokabelRichtig"]);
                        BenutzerAlle.Add(benutzerNeu);
                    }
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    return BenutzerAlle;
                }
                catch (Exception e)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusDescription = e.Message;
                    return new List<Benutzer>();
                }
            }
        }

        //Erzeugt ein neues Vokabel
        public bool CreateVokabel(Vokabel vokabel)
        {
            using (con = new SqlConnection(constring))
            {
                try
                {
                    cmd = new SqlCommand("INSERT INTO VokDB (BenutzerID, Deutsch, Deutsch2, Englisch, Englisch2, Fach) VALUES (@benutzerID, @deutsch, @deutsch2, @englisch, @englisch2, 1)", con);
                    cmd.Parameters.AddWithValue("@benutzerID", vokabel.BenutzerId);
                    cmd.Parameters.AddWithValue("@deutsch", vokabel.Deutsch);
                    cmd.Parameters.AddWithValue("@deutsch2", vokabel.Deutsch2);
                    cmd.Parameters.AddWithValue("@englisch", vokabel.Englisch);
                    cmd.Parameters.AddWithValue("@englisch2", vokabel.Englisch2);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    return true;
                }
                catch (Exception)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return false;
                }
            }
        }

        //Bearbeitet ein bestimmtes Vokabel
        public bool UpdateVokabel(string vokabelid, Vokabel vokabel)
        {
            using (con = new SqlConnection(constring))
            {
                try
                {
                    cmd = new SqlCommand("UPDATE VokDB SET BenutzerID = @benutzerID, Deutsch = @deutsch, Deutsch2 = @deutsch2, Englisch = @englisch, Englisch2 = @englisch2, Fach = @fach WHERE ID = @id", con);
                    cmd.Parameters.AddWithValue("@benutzerID", vokabel.BenutzerId);
                    cmd.Parameters.AddWithValue("@deutsch", vokabel.Deutsch);
                    cmd.Parameters.AddWithValue("@deutsch2", vokabel.Deutsch2);
                    cmd.Parameters.AddWithValue("@englisch", vokabel.Englisch);
                    cmd.Parameters.AddWithValue("@englisch2", vokabel.Englisch2);
                    cmd.Parameters.AddWithValue("@fach", vokabel.Fach);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(vokabelid));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    return true;
                }
                catch (Exception)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return false;
                }
            }
        }

        //Bearbeitet eine Liste von Vokabeln
        public bool UpdateVokabelList(List<Vokabel> vokabel)
        {
            foreach (Vokabel vok in vokabel)
            {
                using (con = new SqlConnection(constring))
                {
                    try
                    {
                        cmd = new SqlCommand("UPDATE VokDB SET BenutzerID = @benutzerID, Deutsch = @deutsch, Deutsch2 = @deutsch2, Englisch = @englisch, Englisch2 = @englisch2, Fach = @fach WHERE ID = @id", con);
                        cmd.Parameters.AddWithValue("@benutzerID", vok.BenutzerId);
                        cmd.Parameters.AddWithValue("@deutsch", vok.Deutsch);
                        cmd.Parameters.AddWithValue("@deutsch2", vok.Deutsch2);
                        cmd.Parameters.AddWithValue("@englisch", vok.Englisch);
                        cmd.Parameters.AddWithValue("@englisch2", vok.Englisch2);
                        cmd.Parameters.AddWithValue("@fach", vok.Fach);
                        cmd.Parameters.AddWithValue("@id", vok.ID);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        return false;
                    }
                }
            }
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
            return true;
        }

        //Löscht ein bestimmtes Vokabel
        public bool DeleteVokabel(string vokabelid)
        {
            using (con = new SqlConnection(constring))
            {
                try
                {
                    cmd = new SqlCommand("DELETE FROM VokDB WHERE ID = @id", con);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(vokabelid));
                    con.Open();
                    cmd.ExecuteNonQuery();
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    return true;
                }
                catch (Exception)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return false;
                }
            }
        }

        //Liefert ein bestimmtes Vokabel
        public Vokabel GetVokabel(string vokabelid)
        {
            using (con = new SqlConnection(constring))
            {
                try
                {
                    cmd = new SqlCommand("SELECT * FROM VokDB WHERE ID = @id", con);
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(vokabelid));
                    con.Open();
                    reader = cmd.ExecuteReader();
                    Vokabel vokabelNeu = new Vokabel();
                    while (reader.Read())
                    {
                        vokabelNeu.ID = (int)reader["ID"];
                        vokabelNeu.BenutzerId = (int)reader["BenutzerID"];
                        vokabelNeu.Deutsch = (string)reader["Deutsch"];
                        vokabelNeu.Deutsch2 = (string)reader["Deutsch2"];
                        vokabelNeu.Englisch = (string)reader["Englisch"];
                        vokabelNeu.Englisch2 = (string)reader["Englisch2"];
                        vokabelNeu.Fach = (int)reader["Fach"];
                    }
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    return vokabelNeu;
                    
                }
                catch (Exception)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return new Vokabel();
                }
            }
        }

        //Liefert alle Vokabel für einen bestimmten Benutzer
        public List<Vokabel> GetVokabelForBenutzer(string benutzerid)
        {
            using (con = new SqlConnection(constring))
            {
                try
                {
                    cmd = new SqlCommand("SELECT * FROM VokDB WHERE BenutzerID = @benutzerId", con);
                    cmd.Parameters.AddWithValue("@benutzerId", Convert.ToInt32(benutzerid));
                    con.Open();
                    reader = cmd.ExecuteReader();
                    List<Vokabel> vokabel = new List<Vokabel>();
                    while (reader.Read())
                    {
                        Vokabel vokabelNeu = new Vokabel();
                        vokabelNeu.ID = (int)reader["ID"];
                        vokabelNeu.BenutzerId = (int)reader["BenutzerID"];
                        vokabelNeu.Deutsch = (string)reader["Deutsch"];
                        vokabelNeu.Deutsch2 = (string)reader["Deutsch2"];
                        vokabelNeu.Englisch = (string)reader["Englisch"];
                        vokabelNeu.Englisch2 = (string)reader["Englisch2"];
                        vokabelNeu.Fach = (int)reader["Fach"];
                        vokabel.Add(vokabelNeu);
                    }
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    return vokabel;

                }
                catch (Exception ex)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusDescription = ex.Message;
                    Vokabel neu = new Vokabel();
                    neu.Deutsch = ex.Message;
                    List<Vokabel> vok = new List<Vokabel>();
                    vok.Add(neu);
                    return vok;
                }
            }
        }

        //Liefert alle Vokabel für einen bestimmten Benutzer und ein bestimmtes Fach
        public List<Vokabel> GetVokabelForBenutzerForFach(string benutzerid, string vokabelFach)
        {
            using (con = new SqlConnection(constring))
            {
                try
                {
                    cmd = new SqlCommand("SELECT * FROM VokDB WHERE BenutzerID = @benutzerId AND Fach = @fach", con);
                    cmd.Parameters.AddWithValue("@benutzerId", Convert.ToInt32(benutzerid));
                    cmd.Parameters.AddWithValue("@fach", Convert.ToInt32(vokabelFach));
                    con.Open();
                    reader = cmd.ExecuteReader();
                    List<Vokabel> vokabel = new List<Vokabel>();
                    while (reader.Read())
                    {
                        Vokabel vokabelNeu = new Vokabel();
                        vokabelNeu.ID = (int)reader["ID"];
                        vokabelNeu.BenutzerId = (int)reader["BenutzerID"];
                        vokabelNeu.Deutsch = (string)reader["Deutsch"];
                        vokabelNeu.Deutsch2 = (string)reader["Deutsch2"];
                        vokabelNeu.Englisch = (string)reader["Englisch"];
                        vokabelNeu.Englisch2 = (string)reader["Englisch2"];
                        vokabelNeu.Fach = (int)reader["Fach"];
                        vokabel.Add(vokabelNeu);
                    }
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    return vokabel;

                }
                catch (Exception)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return new List<Vokabel>();
                }
            }
        }

        //Liefert alle Fächer für einen bestimmten Benutzer
        //public List<Fach> GetFächer(string benutzerid)
        //{
        //    using (con = new SqlConnection(constring))
        //    {
        //        try
        //        {
        //            cmd = new SqlCommand("SELECT * FROM BenutzerDB WHERE ID = @benutzerId", con);
        //            cmd.Parameters.AddWithValue("@benutzerId", Convert.ToInt32(benutzerid));
        //            con.Open();
        //            reader = cmd.ExecuteReader();
        //            List<Fach> fächer = new List<Fach>();
        //            reader.Read();
        //            for (int i = 1; i <= 4; i++)
        //            {
        //                Fach fachNeu = new Fach();
        //                fachNeu.ID = i;
        //                fachNeu.Zeit = (string)reader["Fach" + i];
        //            }
        //            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
        //            return fächer;

        //        }
        //        catch (Exception)
        //        {
        //            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
        //            return new List<Fach>();
        //        }
        //    }
        //}

        //Bearbeitet ein bestimmtes Fach eines bestimmten Benutzers
        public bool UpdateFach(string benutzerid, Fach fach)
        {
            using (con = new SqlConnection(constring))
            {
                try
                {
                    cmd = new SqlCommand("UPDATE BenutzerDB SET Fach" + fach.ID + " = @fachzeit WHERE ID = @benutzerId", con);
                    cmd.Parameters.AddWithValue("@benutzerId", Convert.ToInt32(benutzerid));
                    cmd.Parameters.AddWithValue("@fachzeit", fach.Zeit);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    return true;
                }
                catch (Exception)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return false;
                }
            }
        }

        //public Teststatistik GetStatistik(string benutzerid)
        //{
        //    using (con = new SqlConnection(constring))
        //    {
        //        try
        //        {
        //            cmd = new SqlCommand("SELECT * FROM BenutzerDB WHERE ID = @benutzerId", con);
        //            cmd.Parameters.AddWithValue("@benutzerId", Convert.ToInt32(benutzerid));
        //            con.Open();
        //            reader = cmd.ExecuteReader();
        //            Teststatistik statistik = new Teststatistik();
        //            while (reader.Read())
        //            {
        //                statistik.TestBeendet = (int)reader["TestBeendet"];
        //                statistik.TestGestartet = (int)reader["TestGestartet"];
        //                statistik.VokabelGeübt = (int)reader["VokabelGeübt"];
        //                statistik.VokabelRichtig = (int)reader["VokabelRichtig"];
        //            }
        //            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
        //            return statistik;

        //        }
        //        catch (Exception e)
        //        {
        //            WebOperationContext.Current.OutgoingResponse.StatusDescription = e.Message;
        //            return new Teststatistik();
        //        }
        //    }
        //}


        //Bearbeitet die Statistik eines bestimmten Benutzers
        public bool UpdateStatistik(string benutzerid, Teststatistik statistik)
        {
            using (con = new SqlConnection(constring))
            {
                try
                {
                    cmd = new SqlCommand("UPDATE BenutzerDB SET TestGestartet = @testgestartet, TestBeendet = @testbeendet, VokabelGeübt = @vokabelgeübt, VokabelRichtig = @vokabelrichtig WHERE ID = @benutzerId", con);
                    cmd.Parameters.AddWithValue("@benutzerId", Convert.ToInt32(benutzerid));
                    cmd.Parameters.AddWithValue("@testgestartet", statistik.TestGestartet);
                    cmd.Parameters.AddWithValue("@testbeendet", statistik.TestBeendet);
                    cmd.Parameters.AddWithValue("@vokabelgeübt", statistik.VokabelGeübt);
                    cmd.Parameters.AddWithValue("@vokabelrichtig", statistik.VokabelRichtig);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    return true;
                }
                catch (Exception)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return false;
                }
            }
        }        
    }
}
