using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektarbeitVokabeltrainerDatenbank
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection con = new SqlConnection("Server=(localdb)\\Projects; Integrated security=true; Database = MyDatabase");
            //SqlConnection con = new SqlConnection("Server=(local)\\SQLEXPRESS; Integrated security=true; Database = MyDatabase");
            SqlCommand createDB = new SqlCommand("Create database MyDatabase on primary " +
                                "(Name = MyDatabase_Data, " +
                                "Filename = 'C:\\temp\\Datenbank\\MyDatabase.mdf')" +
                                "Log on (Name = MyDatabase_Log," +
                                "Filename = 'C:\\temp\\Datenbank\\MyDatabase.ldf')", con);
            SqlCommand command = new SqlCommand("Create Table BenutzerDB(ID Int identity(1,1) primary key, Benutzername varchar(100), Passwort varchar(100), Fach1 varchar(100), Fach2 varchar(100), Fach3 varchar(100), Fach4 varchar(100), TestGestartet int, TestBeendet int, VokabelGeübt int, VokabelRichtig int)", con);
            SqlCommand command2 = new SqlCommand("Create Table VokDB(ID Int identity(1,1) primary key, BenutzerID int, Deutsch varchar(100), Deutsch2 varchar(100), Englisch varchar(100), Englisch2 varchar(100), Fach int)", con);
            SqlCommand command3 = new SqlCommand("Alter Table BenutzerDB Add Guid uniqueidentifier NULL", con);

            try
            {
                con.Open();
                //createDB.ExecuteNonQuery();
                //Console.WriteLine("Datenbank erzeugt");
                //command.ExecuteNonQuery();
                //Console.WriteLine("Tabelle erzeugt");
                //command2.ExecuteNonQuery();
                //Console.WriteLine("Tabelle erzeugt");
                command3.ExecuteNonQuery();
                Console.WriteLine("Tabelle geändert");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open)
                    con.Close();
            }
        }
    }
}
