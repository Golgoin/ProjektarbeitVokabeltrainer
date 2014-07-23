using ProjektarbeitVokabeltrainer.ProjektarbeitVokabeltrainerServer;
using System;
using System.Collections.Generic;
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
    /// Interaktionslogik für Eingelogged.xaml
    /// </summary>
    public partial class Eingelogged : UserControl
    {
        private string uri = "http://localhost:52243/ProjektarbeitVokabeltrainerServer.svc/";
        private Benutzer benutzer;
        private List<Vokabel> vokabel;

        public Eingelogged(Benutzer benutzer)
        {
            this.benutzer = GetBenutzer(benutzer);
            vokabel = GetVokabel();
            InitializeComponent();
            if (this.benutzer == null || this.vokabel == null)
            {
                mainGrid.Children.Clear();
                mainGrid.Children.Add(new Start());
            }
        }

        //Liefert den eingeloggten Benutzer
        private Benutzer GetBenutzer(Benutzer benutzer)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri + "benutzer/" + benutzer.ID + "/");
            webrequest.Method = "GET";
            HttpWebResponse webresponse = null;
            try
            {
                webresponse = (HttpWebResponse)webrequest.GetResponse();
                HttpStatusCode rc = webresponse.StatusCode;
                DataContractSerializer serl = new DataContractSerializer(typeof(Benutzer));
                return (Benutzer)serl.ReadObject(webresponse.GetResponseStream());
            }
            catch (WebException we)
            {
                if (we.Response != null)
                {
                    webresponse = (HttpWebResponse)we.Response;
                    MessageBox.Show(webresponse.StatusDescription + "!", "Fehler");
                }
                return null;
            }
            finally
            {
                if (webresponse != null)
                    webresponse.Close();
            }
        }

        //Liefert die Vokabel des Benutzers
        private List<Vokabel> GetVokabel()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri + "benutzer/" + benutzer.ID + "/vokabel/");
            webrequest.Method = "GET";
            HttpWebResponse webresponse = null;
            try
            {
                webresponse = (HttpWebResponse)webrequest.GetResponse();
                HttpStatusCode rc = webresponse.StatusCode;
                DataContractSerializer serl = new DataContractSerializer(typeof(List<Vokabel>));
                return (List<Vokabel>)serl.ReadObject(webresponse.GetResponseStream());
            }
            catch (WebException we)
            {
                if (we.Response != null)
                {
                    webresponse = (HttpWebResponse)we.Response;
                    MessageBox.Show(webresponse.StatusDescription + "!", "Fehler");
                }
                return null;
            }
            finally
            {
                if (webresponse != null)
                    webresponse.Close();
            }
        }

        private void AbmeldenClick(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();
            mainGrid.Children.Add(new Start());
        }

        private void HinzufügenClick(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();
            mainGrid.Children.Add(new VokabelHinzufügen(benutzer));
        }

        private void BearbeitenClick(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();
            mainGrid.Children.Add(new VokabelBearbeiten(benutzer));
        }

        private void StatistikClick(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();
            mainGrid.Children.Add(new Statistik(benutzer));
        }

        //Überprüft bei dem ausgewählten Fach ob es für einen Test verfügbar ist
        private void cmbFachSelChanged(object sender, SelectionChangedEventArgs e)
        {
            if (benutzer.Statistik != null)
            {
                btnStartPanel.Children.Clear();
                ComboBoxItem cmbitem = cmbFach.SelectedItem as ComboBoxItem;
                int fach = (int)cmbitem.MinHeight; //MinHeight = Fach der Lernkartei
                int tage = (int)cmbitem.MinWidth; //MinWidth = Intervall bis zum nächsten Test des Faches
                DateTime now = DateTime.Now;
                Button btnStart = new Button();
                btnStart.Content = "Test starten";
                btnStart.IsDefault = true;
                List<DateTime> fachZeiten = new List<DateTime>();
                fachZeiten.Add(new DateTime());
                fachZeiten.Add(DateTime.Parse(benutzer.Fach1.Zeit));
                fachZeiten.Add(DateTime.Parse(benutzer.Fach2.Zeit));
                fachZeiten.Add(DateTime.Parse(benutzer.Fach3.Zeit));
                fachZeiten.Add(DateTime.Parse(benutzer.Fach4.Zeit));

                if (vokabel.FindAll(vok => vok.Fach == fach).Count == 0) //Überprüfung ob Vokabel vorhanden
                {
                    btnStart.Background = Brushes.Red;
                    btnStart.Click += NoVok;
                }
                else if (now.CompareTo(fachZeiten[fach].AddDays(tage)) < 0) //Überprüfung ob Intervall erreicht
                {
                    btnStart.Background = Brushes.Red;
                    btnStart.Click += NotAvailable;
                    btnStart.Tag = fachZeiten[fach];
                    btnStart.MinWidth = tage;
                }
                else //Test verfügbar
                {
                    btnStart.Background = Brushes.LightGreen;
                    btnStart.Click += StartenClick;
                }
                btnStartPanel.Children.Add(btnStart);
            }
        }

        private void NotAvailable(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            DateTime btnTime = (DateTime)btn.Tag;
            MessageBox.Show("Test nicht möglich!\nWieder möglich am: " + btnTime.AddDays(btn.MinWidth), "Fehler");
        }

        private void NoVok(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Keine Vokabel vorhanden!", "Fehler");
        }

        private void StartenClick(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();
            mainGrid.Children.Add(new Test(cmbRichtung.SelectedIndex, ++cmbFach.SelectedIndex, (cmbLänge.SelectedIndex * 10), benutzer));
        }
    }
}
