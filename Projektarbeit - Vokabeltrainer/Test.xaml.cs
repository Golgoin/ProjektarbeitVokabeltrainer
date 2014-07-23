using ProjektarbeitVokabeltrainer.ProjektarbeitVokabeltrainerServer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjektarbeitVokabeltrainer
{
    /// <summary>
    /// Interaktionslogik für Test.xaml
    /// </summary>
    public partial class Test : UserControl
    {
        string uri = "http://localhost:52243/ProjektarbeitVokabeltrainerServer.svc/";
        private Benutzer benutzer;
        private int richtung;
        private int richtungNeu;
        private int fach;
        private int anzahl;
        private int zähler;
        private int richtig;
        private int zufallszahl;
        private ArrayList zufallszahlen = new ArrayList();
        private List<Vokabel> vokabel = new List<Vokabel>();
        private Random random = new Random();

        public Test(int richtung, int fach, int anzahl, Benutzer benutzer)
        {
            InitializeComponent();
            this.benutzer = benutzer;
            this.richtung = richtung;
            this.fach = fach;
            vokabel = LoadVokabel();
            UpdateStatistikStart();
            if (anzahl == 0 || vokabel.Count < anzahl)
                this.anzahl = vokabel.Count;
            else
                this.anzahl = anzahl;
            progressBar.Maximum = this.anzahl;
            lblBeantwortet.Content = zähler;
            lblRichtig.Content = richtig;
            lblVerbleibend.Content = (this.anzahl - zähler);
            Richtung();
        }
        
        private List<Vokabel> LoadVokabel()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri + "benutzer/" + benutzer.ID + "/vokabel/" + fach + "/");
            webrequest.Method = "GET";
            HttpWebResponse webresponse = null;
            try
            {
                webresponse = (HttpWebResponse)webrequest.GetResponse();
                DataContractSerializer serl = new DataContractSerializer(typeof(List<Vokabel>));
                return (List<Vokabel>)serl.ReadObject(webresponse.GetResponseStream());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (webresponse != null)
                    webresponse.Close();
            }
        }

        private void Überprüfen_Click(object sender, RoutedEventArgs e)
        {
            if (txtAntwort.Background == Brushes.LightGreen || txtAntwort.Background == Brushes.Red)
            {
                überprüfenNext.Content = "Überprüfen";
                Richtung();
            }
            else
            {
                überprüfenNext.Content = "Nächstes Vokabel";
                Überprüfen();
            }
        }

        private void Richtung()
        {
            if (richtung == 0)
            {
                if (random.Next(0, 2) == 1)
                {
                    richtungNeu = 1;
                    TestDeutschEnglisch();
                }
                else
                {
                    richtungNeu = 2;
                    TestEnglischDeutsch();
                }
            }
            if (richtung == 1)
            {
                richtungNeu = 1;
                TestDeutschEnglisch();
            }
            if (richtung == 2)
            {
                richtungNeu = 2;
                TestEnglischDeutsch();
            }
        }

        private void TestEnglischDeutsch()
        {
            if (Ende())
                Ende2();
            else
            {
                txtAntwort.Background = Brushes.White;
                txtAntwort.Text = "";
                zufallszahlen.Add(Zufallszahl());
                txtFrage.Text = vokabel[zufallszahl].Englisch;
                if (vokabel[zufallszahl].Englisch2 != "")
                    txtFrage.Text += " / " + vokabel[zufallszahl].Englisch2;
            }
        }

        private void TestDeutschEnglisch()
        {
            if (Ende())
                Ende2();
            else
            {
                txtAntwort.Background = Brushes.White;
                txtAntwort.Text = "";
                zufallszahlen.Add(Zufallszahl());
                txtFrage.Text = vokabel[zufallszahl].Deutsch;
                if (vokabel[zufallszahl].Deutsch2 != "")
                    txtFrage.Text += " / " + vokabel[zufallszahl].Deutsch2;
            }
        }

        private int Zufallszahl()
        {
            zufallszahl = random.Next(0, vokabel.Count);
            while (zufallszahlen.Contains(zufallszahl))
                zufallszahl = random.Next(0, vokabel.Count);
            return zufallszahl;
        }
        
        private void Überprüfen()
        {
            if (richtungNeu == 1)
            {
                if (txtAntwort.Text == vokabel[zufallszahl].Englisch || txtAntwort.Text == vokabel[zufallszahl].Englisch2)
                {
                    txtAntwort.Background = Brushes.LightGreen;
                    txtAntwort.Text = "Richtig";
                    if (vokabel[zufallszahl].Fach < 4)
                        vokabel[zufallszahl].Fach += 1;
                    zähler++;
                    richtig++;
                }
                else
                {
                    txtAntwort.Background = Brushes.Red;
                    txtAntwort.Text = "Leider falsch! Richtig wäre:\n" + vokabel[zufallszahl].Englisch;
                    if (vokabel[zufallszahl].Englisch2 != "")
                        txtAntwort.Text += " / " + vokabel[zufallszahl].Englisch2;
                    vokabel[zufallszahl].Fach = 1;
                    zähler++;
                }
            }
            if (richtungNeu == 2)
            {
                if (txtAntwort.Text == vokabel[zufallszahl].Deutsch || txtAntwort.Text == vokabel[zufallszahl].Deutsch2)
                {
                    txtAntwort.Background = Brushes.LightGreen;
                    txtAntwort.Text = "Richtig";
                    if (vokabel[zufallszahl].Fach < 4)
                        vokabel[zufallszahl].Fach += 1;
                    zähler++;
                    richtig++;
                }
                else
                {
                    txtAntwort.Background = Brushes.Red;
                    txtAntwort.Text = "Leider falsch! Richtig wäre:\n" + vokabel[zufallszahl].Deutsch;
                    if (vokabel[zufallszahl].Deutsch2 != "")
                        txtAntwort.Text += " / " + vokabel[zufallszahl].Deutsch2;
                    vokabel[zufallszahl].Fach = 1;
                    zähler++;
                }
            }
            progressBar.Value++;
            lblBeantwortet.Content = zähler;
            lblRichtig.Content = richtig;
            lblVerbleibend.Content = (this.anzahl - zähler);
        }

        private bool Ende()
        {
            if (zähler == anzahl)
                return true;
            if (zähler == vokabel.Count)
                return true;
            return false;
        }

        private void Ende2()
        {
            UpdateStatistikEnde();
            UpdateVokabelList();
            UpdateFachEnde();
            txtFrage.Text = "Test beendet!";
            txtAntwort.Text = "Richtige Antworten: " + richtig;
            überprüfenNext.Content = "Hauptmenü";
            überprüfenNext.Click -= Überprüfen_Click;
            überprüfenNext.Click += ÜberprüfenHauptmenü;
        }

        private void UpdateVokabelList()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri + "vokabel/");
            webrequest.Method = "PUT";
            webrequest.ContentType = "application/xml";
            HttpWebResponse webresponse = null;
            try
            {
                DataContractSerializer serl = new DataContractSerializer(typeof(List<Vokabel>));
                using (Stream requestStream = webrequest.GetRequestStream())
                    serl.WriteObject(requestStream, vokabel);
                webresponse = (HttpWebResponse)webrequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (webresponse != null)
                    webresponse.Close();
            }
        }

        private void UpdateStatistikStart()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri + "benutzer/" + benutzer.ID + "/Statistik/");
            webrequest.Method = "PUT";
            webrequest.ContentType = "application/xml";
            HttpWebResponse webresponse = null;
            try
            {
                DataContractSerializer serl = new DataContractSerializer(typeof(Teststatistik));
                Teststatistik statistik = benutzer.Statistik;
                statistik.TestGestartet += 1;
                benutzer.Statistik = statistik;
                using (Stream requestStream = webrequest.GetRequestStream())
                    serl.WriteObject(requestStream, statistik);
                webresponse = (HttpWebResponse)webrequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (webresponse != null)
                    webresponse.Close();
            }
        }

        private void UpdateStatistikEnde()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri + "benutzer/" + benutzer.ID + "/Statistik/");
            webrequest.Method = "PUT";
            webrequest.ContentType = "application/xml";
            HttpWebResponse webresponse = null;
            try
            {
                DataContractSerializer serl = new DataContractSerializer(typeof(Teststatistik));
                Teststatistik statistik = benutzer.Statistik;
                statistik.TestBeendet += 1;
                statistik.VokabelGeübt += anzahl;
                statistik.VokabelRichtig += richtig;
                using (Stream requestStream = webrequest.GetRequestStream())
                    serl.WriteObject(requestStream, statistik);
                webresponse = (HttpWebResponse)webrequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (webresponse != null)
                    webresponse.Close();
            }
        }

        private void UpdateFachEnde()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri + "benutzer/" + benutzer.ID + "/Fach/");
            webrequest.Method = "PUT";
            webrequest.ContentType = "application/xml";
            HttpWebResponse webresponse = null;
            try
            {
                DataContractSerializer serl = new DataContractSerializer(typeof(Fach));
                Fach fach = new Fach();
                fach.ID = this.fach;
                fach.Zeit = DateTime.Now.ToString();
                using (Stream requestStream = webrequest.GetRequestStream())
                    serl.WriteObject(requestStream, fach);
                webresponse = (HttpWebResponse)webrequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (webresponse != null)
                    webresponse.Close();
            }
        }

        private void ÜberprüfenHauptmenü(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();
            mainGrid.Children.Add(new Eingelogged(benutzer));
        }

        private void AbbrechenClick(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();
            mainGrid.Children.Add(new Eingelogged(benutzer));
        }
    }
}
