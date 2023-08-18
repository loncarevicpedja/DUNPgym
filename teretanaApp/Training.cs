using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows;

namespace teretanaApp
{
    public class Trening
    {
        public int Id { get; set; }
        public string DatumOdrzavanja { get; set; }
        public string PocetakTreninga { get; set; }
        public string KrajTreninga { get; set; }
        public string VrstaTreninga { get; set; }
        public string Instruktor { get; set; }
        public int KapacitetTreninga { get; set; }

        public Trening(int id, string datumOdrzavanja, string pocetakTreninga, string krajTreninga, string vrstaTreninga, string instruktor, int kapacitetTreninga)
        {
            Id = id;
            DatumOdrzavanja = datumOdrzavanja;
            PocetakTreninga = pocetakTreninga;
            KrajTreninga = krajTreninga;
            VrstaTreninga = vrstaTreninga;
            Instruktor = instruktor;
            KapacitetTreninga = kapacitetTreninga;
        }

        public Trening()
        {
        }

        public void EvidentirajTreningUBazi()
        {
            string connectionString = "Data Source=THINKPAD;Initial Catalog=database;Integrated Security=True; trustServerCertificate=true";
            string query = "INSERT INTO treninzi (datumOdrzavanja, pocetakTreninga, krajTreninga, vrstaTreninga, instruktor, kapacitet, brojPrijavljenih) VALUES (@DatumOdrzavanja, @PocetakTreninga, @KrajTreninga, @VrstaTreninga, @Instruktor, @KapacitetTreninga, 0)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@DatumOdrzavanja", DatumOdrzavanja);
                cmd.Parameters.AddWithValue("@PocetakTreninga", PocetakTreninga);
                cmd.Parameters.AddWithValue("@KrajTreninga", KrajTreninga);
                cmd.Parameters.AddWithValue("@VrstaTreninga", VrstaTreninga);
                cmd.Parameters.AddWithValue("@Instruktor", Instruktor);
                cmd.Parameters.AddWithValue("@KapacitetTreninga", KapacitetTreninga);

                try
                {
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Trening je uspešno evidentiran u bazi podataka.");
                    }
                    else
                    {
                        MessageBox.Show("Neuspešno evidentiranje treninga u bazi podataka.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri pristupu bazi ili izvršavanju upita: " + ex.Message);
                }
            }
        }
        public void UpisUBazu()
        {
            DateTime datumOdrzavanjaTreninga = DateTime.ParseExact(DatumOdrzavanja, "M/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);

            if (datumOdrzavanjaTreninga < DateTime.Today)
            {
                MessageBox.Show("Nije moguće kreirati trening u prošlosti. Molimo izaberite validan datum održavanja treninga.");
                return;
            }

            string[] arrPocetakTreninga = PocetakTreninga.Split(' ');
            PocetakTreninga = arrPocetakTreninga[arrPocetakTreninga.Length - 1];
            string[] arrKrajTreninga = KrajTreninga.Split(' ');
            KrajTreninga = arrKrajTreninga[arrKrajTreninga.Length - 1];
            string[] arrVrstaTrening = VrstaTreninga.Split(' ');
            VrstaTreninga = arrVrstaTrening[arrVrstaTrening.Length - 1];
            string FormatiraniDatumClanarine = datumOdrzavanjaTreninga.ToString("dd/MM/yyyy");
            SqlConnection con = new SqlConnection("Data Source=THINKPAD;Initial Catalog=database;Integrated Security=True; trustServerCertificate=true");
            SqlCommand cmd = new SqlCommand("INSERT INTO treninzi (datumOdrzavanja, pocetakTreninga, krajTreninga, vrstaTreninga, instruktor, kapacitet) VALUES (@DatumOdrzavanja, @PocetakTreninga, @KrajTreninga, @VrstaTreninga, @Instruktor, @KapacitetTreninga)", con);
            cmd.Parameters.AddWithValue("@DatumOdrzavanja", FormatiraniDatumClanarine);
            cmd.Parameters.AddWithValue("@PocetakTreninga", PocetakTreninga);
            cmd.Parameters.AddWithValue("@KrajTreninga", KrajTreninga);
            cmd.Parameters.AddWithValue("@VrstaTreninga", VrstaTreninga);
            cmd.Parameters.AddWithValue("@Instruktor", Instruktor);
            cmd.Parameters.AddWithValue("@KapacitetTreninga", KapacitetTreninga);

            try
            {
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Uspesno je izvrsen upis u bazu");
                }
                else
                {
                    MessageBox.Show("Neuspesno je izvrsen upis u bazu");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri pristupu bazi ili izvršavanju upita" + ex);
            }
            finally
            {
                con.Close();
            }
        }


        public List<Trening> PretraziTreninge(string datum, string vrstaTreninga)
        {
            DateTime DatumTreninga = DateTime.ParseExact(datum, "M/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            string formatiraniDatum = DatumTreninga.ToString("dd/MM/yyyy");
            List<Trening> rezultat = new List<Trening>();
            string connectionString = "Data Source=THINKPAD;Initial Catalog=database;Integrated Security=True; trustServerCertificate=true";
            string query = "SELECT * FROM treninzi WHERE datumOdrzavanja = @Datum AND vrstaTreninga = @VrstaTreninga";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Datum", formatiraniDatum);
                cmd.Parameters.AddWithValue("@VrstaTreninga", vrstaTreninga);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Trening trening = new Trening();
                        trening.DatumOdrzavanja = reader["datumOdrzavanja"].ToString();
                        trening.PocetakTreninga = reader["pocetakTreninga"].ToString();
                        trening.KrajTreninga = reader["krajTreninga"].ToString();
                        trening.VrstaTreninga = reader["vrstaTreninga"].ToString();
                        trening.Instruktor = reader["instruktor"].ToString();
                        trening.KapacitetTreninga = Convert.ToInt32(reader["kapacitet"]);
                        trening.Id = Convert.ToInt32(reader["idTreninga"]);

                        int brojRezervacija = BrojRezervacijaTreninga(trening.Id);
                        if (brojRezervacija < trening.KapacitetTreninga)
                        {
                            rezultat.Add(trening);
                        }
                        else
                        {
                            MessageBox.Show("Nije moguće rezervisati odabrani trening jer je popunjen!");
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri pristupu bazi ili izvršavanju upita: " + ex.Message);
                }
            }

            return rezultat;
        }

        private int BrojRezervacijaTreninga(int idTreninga)
        {
            int brojRezervacija = 0;
            string connectionString = "Data Source=THINKPAD;Initial Catalog=database;Integrated Security=True; trustServerCertificate=true";
            string query = "SELECT COUNT(*) FROM rezervacije WHERE idTreninga = @IdTreninga";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@IdTreninga", idTreninga);

                try
                {
                    con.Open();
                    brojRezervacija = (int)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri pristupu bazi ili izvršavanju upita: " + ex.Message);
                }
            }

            return brojRezervacija;
        }


    }

}
