using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Data.SqlClient;

namespace teretanaApp
{
    public class Uplata
    {
        public string ImePrezime { get; set; }
        public DateTime DatumIzvrsenjaTransakcije { get; set; }
        public decimal Cena { get; set; }

        public Uplata(string imePrezime, DateTime datumIzvrsenjaTransakcije, decimal cena)
        {
            ImePrezime = imePrezime;
            DatumIzvrsenjaTransakcije = datumIzvrsenjaTransakcije;
            Cena = cena;
        }

        public Uplata()
        {
        }

        public static List<Uplata> DohvatiUplate(DateTime pocetakPerioda, DateTime krajPerioda)
        {
            List<Uplata> uplate = new List<Uplata>();
            string connectionString = "Data Source=THINKPAD;Initial Catalog=database;Integrated Security=True; trustServerCertificate=true";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT k.name AS Ime, ep.datumIzvrsenjaTransakcije, ep.cena " +
                       "FROM evidencijaPlacanja ep " +
                       "JOIN korisnici k ON ep.idKorisnika = k.id " +
                       "WHERE ep.datumIzvrsenjaTransakcije BETWEEN @PocetakPerioda AND @KrajPerioda";

        
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@PocetakPerioda", pocetakPerioda.ToString("dd/MM/yyyy"));
                cmd.Parameters.AddWithValue("@KrajPerioda", krajPerioda.ToString("dd/MM/yyyy"));

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string imePrezime = reader["Ime"].ToString();
                        DateTime datumIzvrsenjaTransakcije = Convert.ToDateTime(reader["datumIzvrsenjaTransakcije"]);
                        decimal cena = Convert.ToDecimal(reader["cena"]);

                        Uplata uplata = new Uplata(imePrezime, datumIzvrsenjaTransakcije, cena);
                        uplate.Add(uplata);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri pristupu bazi ili izvršavanju upita: " + ex.Message);
                }
            }

            return uplate;
        }
    }
}
