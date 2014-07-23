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
        string uri = "http://localhost:52243/ProjektarbeitVokabeltrainerServer.svc/";
        private Benutzer benutzer;
        private List<Vokabel> vokabel;

        public Eingelogged(Benutzer benutzer)
        {
            this.benutzer = GetBenutzer(benutzer);
            vokabel = GetVokabel();
            InitializeComponent();
        }

        private Benutzer GetBenutzer(Benutzer benutzer)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri + "benutzer/" + benutzer.ID + "/");
            webrequest.Method = "GET";
            HttpWebResponse webresponse = null;
            try
            {
                webresponse = (HttpWebResponse)webrequest.GetResponse();
                DataContractSerializer serl = new DataContractSerializer(typeof(Benutzer));
                return (Benutzer)serl.ReadObject(webresponse.GetResponseStream());
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

        private List<Vokabel> GetVokabel()
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri + "benutzer/" + benutzer.ID + "/vokabel/");
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

        private void cmbFachSelChanged(object sender, SelectionChangedEventArgs e)
        {
            btnStartPanel.Children.Clear();
            ComboBoxItem cmbitem = cmbFach.SelectedItem as ComboBoxItem;
            int fach = (int)cmbitem.MinHeight;
            int tage = (int)cmbitem.MinWidth;
            DateTime now = DateTime.Now;
            Button btnStart = new Button();
            btnStart.Content = "Test starten";
            List<DateTime> fachZeiten = new List<DateTime>();
            fachZeiten.Add(new DateTime());
            fachZeiten.Add(DateTime.Parse(benutzer.Fach1.Zeit));
            fachZeiten.Add(DateTime.Parse(benutzer.Fach2.Zeit));
            fachZeiten.Add(DateTime.Parse(benutzer.Fach3.Zeit));
            fachZeiten.Add(DateTime.Parse(benutzer.Fach4.Zeit));

            if (vokabel.FindAll(vok => vok.Fach == fach).Count == 0)
            {
                btnStart.Background = Brushes.Red;
                btnStart.Click += NoVok;
            }
            else if (now.CompareTo(fachZeiten[fach].AddDays(tage)) < 0)
            {
                btnStart.Background = Brushes.Red;
                btnStart.Click += NotAvailable;
                btnStart.Tag = fachZeiten[fach];
                btnStart.MinHeight = tage;
            }
            else
            {
                btnStart.Background = Brushes.LightGreen;
                btnStart.Click += StartenClick;
            }
            btnStartPanel.Children.Add(btnStart);
        }

        private void NotAvailable(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            DateTime btnTime = (DateTime)btn.Tag;
            MessageBox.Show("Test nicht möglich!\nWieder möglich am: " + btnTime.AddDays(btn.MinHeight), "Fehler");
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
