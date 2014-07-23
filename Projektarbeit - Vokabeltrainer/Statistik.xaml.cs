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
    /// Interaktionslogik für Statistik.xaml
    /// </summary>
    public partial class Statistik : UserControl
    {
        private string uri = "http://localhost:52243/ProjektarbeitVokabeltrainerServer.svc/";
        private Benutzer benutzer;

        public Statistik(Benutzer benutzer)
        {
            InitializeComponent();
            this.benutzer = GetBenutzer(benutzer);
            this.DataContext = this.benutzer;
            FillProgressBars(GetVokabel());
            CheckTimes();
        }

        private void ZurückClick(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();
            mainGrid.Children.Add(new Eingelogged(benutzer));
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
            List<Vokabel> vokabel = new List<Vokabel>();
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

        private void FillProgressBars(List<Vokabel> vokabel)
        {
            if (vokabel.Count != 0)
            {
                progressFach1.Maximum = vokabel.Count;
                progressFach2.Maximum = vokabel.Count;
                progressFach3.Maximum = vokabel.Count;
                progressFach4.Maximum = vokabel.Count;

                progressFach1.Value = vokabel.FindAll(vok => vok.Fach == 1).Count;
                progressFach2.Value = vokabel.FindAll(vok => vok.Fach == 2).Count;
                progressFach3.Value = vokabel.FindAll(vok => vok.Fach == 3).Count;
                progressFach4.Value = vokabel.FindAll(vok => vok.Fach == 4).Count;
            }
        }

        private void CheckTimes()
        {
            DateTime now = DateTime.Now;

            if (fach1.Content.ToString() == "0")
                FillLabelNoVok(fach1Zeit, fach11Zeit);
            else if (now.CompareTo(DateTime.Parse(benutzer.Fach1.Zeit).AddDays(1)) < 0)
                FillLabelNotAvailable(fach1Zeit, fach11Zeit, fach111Zeit, benutzer.Fach1, 1);
            else
                CreateButton(fach1Panel, 1);

            if (fach2.Content.ToString() == "0")
                FillLabelNoVok(fach2Zeit, fach22Zeit);
            else if (now.CompareTo(DateTime.Parse(benutzer.Fach2.Zeit).AddDays(2)) < 0)
                FillLabelNotAvailable(fach2Zeit, fach22Zeit, fach222Zeit, benutzer.Fach2, 2);
            else
                CreateButton(fach2Panel, 2);

            if (fach3.Content.ToString() == "0")
                FillLabelNoVok(fach3Zeit, fach33Zeit);
            else if (now.CompareTo(DateTime.Parse(benutzer.Fach3.Zeit).AddDays(5)) < 0)
                FillLabelNotAvailable(fach3Zeit, fach33Zeit, fach333Zeit, benutzer.Fach3, 5);
            else
                CreateButton(fach3Panel, 3);

            if (fach4.Content.ToString() == "0")
                FillLabelNoVok(fach4Zeit, fach44Zeit);
            else if (now.CompareTo(DateTime.Parse(benutzer.Fach4.Zeit).AddDays(10)) < 0)
                FillLabelNotAvailable(fach4Zeit, fach44Zeit, fach444Zeit, benutzer.Fach4, 10);
            else
                CreateButton(fach4Panel, 4);
        }

        private void FillLabelNoVok(Label label, Label label2)
        {
            label.Content = "Test nicht möglich!";
            label2.Content = "Keine Vokabel!";
        }

        private void FillLabelNotAvailable(Label label, Label label2, Label label3, Fach fach, int tage)
        {
            label.Content = "Test nicht möglich!";
            label2.Content = "Wieder möglich am:";
            label3.Content = (DateTime.Parse(fach.Zeit).AddDays(tage)).ToShortDateString();
        }

        private void CreateButton(Panel panel, int i)
        {
            panel.Children.Clear();
            Button b = new Button();
            b.Width = 75;
            b.Height = 22;
            b.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            b.Content = "Test starten";
            b.Click += b_Click;
            b.MinWidth = i;
            panel.Children.Add(b);
        }

        private void b_Click(object sender, RoutedEventArgs e)
        {
            Button b = new Button();
            if (sender is Button)
                b = sender as Button;
            mainGrid.Children.Clear();
            mainGrid.Children.Add(new Test(0, (int)b.MinWidth, 0, benutzer));
        }
    }
}
