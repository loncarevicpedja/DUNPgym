using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows;

namespace teretanaApp
{
    public class Racun
    {
        public int IdRacuna { get; set; }
        public int IdKorisnika { get; set; }
        public string NacinPlacanja { get; set; }
        public int Cena { get; set; }
        public string BrojKartice { get; set; }
        public string DatumIsteka { get; set; }
        public string CVV { get; set; }
        public string DatumIzvrsenjaTransakcije { get; set; }

        public Racun(int idRacuna, int idKorisnika, string nacinPlacanja, int cena, string brojKartice, string datumIsteka, string cvv, string datumIzvrsenjaTransakcije)
        {
            IdRacuna = idRacuna;
            IdKorisnika = idKorisnika;
            NacinPlacanja = nacinPlacanja;
            Cena = cena;
            BrojKartice = brojKartice;
            DatumIsteka = datumIsteka;
            CVV = cvv;
            DatumIzvrsenjaTransakcije = datumIzvrsenjaTransakcije;
        }

        public Racun()
        {

        }
        public void EvidentirajPlacanje(string noviDatum, int idKorisnika, string nacinPlacanja, decimal cena, string? brojKartice, string? datumIstekaKartice, string? cvv)
        {
            DateTime datumIzvrsenjaTransakcije = DateTime.Now;
            var datumIzvrsenjaTrans = datumIzvrsenjaTransakcije.ToString("dd/MM/yyyy");
            string connectionString = "Data Source=THINKPAD;Initial Catalog=database;Integrated Security=True; trustServerCertificate=true";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    
                    string poruka = "Cena za produženje članarine je: " + cena + " dinara. Da li želite da nastavite sa plaćanjem?";
                    MessageBoxResult rezultat = MessageBox.Show(poruka, "Potvrda plaćanja", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (rezultat == MessageBoxResult.Yes)
                    {
                        con.Open();

                        string queryUpdateDate = "UPDATE korisnici SET date = @DatumIsteka WHERE id = @IdKorisnika";
                        SqlCommand cmdUpdateDate = new SqlCommand(queryUpdateDate, con);
                        cmdUpdateDate.Parameters.AddWithValue("@IdKorisnika", idKorisnika);
                        cmdUpdateDate.Parameters.AddWithValue("@DatumIsteka", noviDatum);
                        cmdUpdateDate.ExecuteNonQuery();

                        string queryInsert = "INSERT INTO evidencijaPlacanja (idKorisnika, nacinPlacanja, cena, brojKartice, datumIsteka, cvv, datumIzvrsenjaTransakcije) VALUES (@IdKorisnika, @NacinPlacanja, @Cena, @BrojKartice, @DatumIsteka, @CVV, @DatumIzvrsenjaTransakcije)";
                        SqlCommand cmdInsert = new SqlCommand(queryInsert, con);
                        cmdInsert.Parameters.AddWithValue("@IdKorisnika", idKorisnika);
                        cmdInsert.Parameters.AddWithValue("@NacinPlacanja", nacinPlacanja);
                        cmdInsert.Parameters.AddWithValue("@Cena", cena);
                        cmdInsert.Parameters.AddWithValue("@BrojKartice", brojKartice);
                        cmdInsert.Parameters.AddWithValue("@DatumIsteka", datumIstekaKartice);
                        cmdInsert.Parameters.AddWithValue("@CVV", cvv);
                        cmdInsert.Parameters.AddWithValue("@DatumIzvrsenjaTransakcije", datumIzvrsenjaTrans);
                        cmdInsert.ExecuteNonQuery();

                        con.Close();

                        KreirajRacun(brojKartice, nacinPlacanja, MainWindow.TrenutniKorisnik.ImePrezime, MainWindow.TrenutniKorisnik.Email, cena, datumIzvrsenjaTransakcije);

                        MessageBox.Show("Vaša uplata je usprešno obavljena, možete pogledati račun!");
                    }
                    else
                    {
                        MessageBox.Show("Vaša uplata je obustavljena!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri pristupu bazi ili izvršavanju upita: " + ex.Message);
            }
        }

        public decimal IzracunavanjeCene(int idKorisnika, string datumProduzenjaString)
        {
            decimal cenaProduzenja = 0;

            string connectionString = "Data Source=THINKPAD;Initial Catalog=database;Integrated Security=True; trustServerCertificate=true";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string querySelect = "SELECT date FROM korisnici WHERE id = @IdKorisnika";

                SqlCommand cmdSelect = new SqlCommand(querySelect, con);
                cmdSelect.Parameters.AddWithValue("@IdKorisnika", idKorisnika);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmdSelect.ExecuteReader();

                    if (reader.Read())
                    {
                        DateTime datumVazenja = Convert.ToDateTime(reader["date"]);
                        reader.Close();

                        DateTime datumProduzenja = Convert.ToDateTime(datumProduzenjaString);

                        bool clanarinaIstekla = datumVazenja < DateTime.Today;

                        if (clanarinaIstekla)
                        {
                            TimeSpan razlika = datumProduzenja - DateTime.Today;
                            int brojDanaProduzenja = (int)razlika.TotalDays;
                            cenaProduzenja = brojDanaProduzenja * 200;
                        }
                        else
                        {
                            TimeSpan razlika = datumProduzenja - datumVazenja;
                            int brojDanaProduzenja = (int)razlika.TotalDays;
                            cenaProduzenja = brojDanaProduzenja * 200;
                        }
                    }
                    else
                    {
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri pristupu bazi ili izvršavanju upita: " + ex.Message);
                }
            }

            return cenaProduzenja;
        }
        public void KreirajRacun(string? brojKartice, string nacinPlacanja, string nazivKupca, string kontaktKupca, decimal cena, DateTime datumFakture)
        {
            string jedinstveniNaziv = $"{nazivKupca}_{DateTime.Now.Ticks}.pdf";

            string putanja = Path.Combine("C:\\Users\\Predrag\\Desktop\\teretanaApp\\racuni", jedinstveniNaziv);

            Document doc = new Document(PageSize.A4, 50, 50, 25, 25);

            try
            {
                PdfWriter.GetInstance(doc, new FileStream(putanja, FileMode.Create));

                doc.Open();

                doc.Add(new Paragraph("Faktura", new Font(Font.FontFamily.HELVETICA, 20, Font.BOLD)));
                doc.Add(new Paragraph("---------------------------------------------------------"));

                doc.Add(new Paragraph("Naziv preduzeca: DUNPGym"));
                doc.Add(new Paragraph("Adresa preduzeca: Vuka Karadžića bb"));
                doc.Add(new Paragraph("Kontakt preduzeca: kontakt@dunpgym.com"));
                doc.Add(new Paragraph("---------------------------------------------------------"));

                doc.Add(new Paragraph("Naziv kupca: " + nazivKupca));
                doc.Add(new Paragraph("Kontakt kupca: " + kontaktKupca));
                doc.Add(new Paragraph("---------------------------------------------------------"));

                doc.Add(new Paragraph("Opis stavke:  produženje clanarine"));
                doc.Add(new Paragraph("Cena: " + cena.ToString("C")));
                doc.Add(new Paragraph("---------------------------------------------------------"));

                doc.Add(new Paragraph("Datum fakture: " + datumFakture.ToString("dd.MM.yyyy.")));

                doc.Close();

                Console.WriteLine("Faktura je uspešno kreirana i sačuvana na putanji: " + putanja);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška pri kreiranju fakture: " + ex.Message);
            }
        }

    }
}
