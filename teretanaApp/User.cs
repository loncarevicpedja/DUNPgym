using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows;

namespace teretanaApp
{
    public class User
    {
        public string ImePrezime { get; set; }
        public string Email { get; set; }
        public string Lozinka { get; set; }
        public string PotvrdaLozinke { get; set; }
        public string VrstaNaloga { get; set; }
        public string VrstaTreninga { get; set; }
        public string DatumClanarine { get; set; }
        public int Id { get; set; }

        public User(string imePrezime, string email, string lozinka, string vrstaNaloga, string vrstaTreninga, string datumClanarine, int id)
        {
            ImePrezime = imePrezime;
            Email = email;
            Lozinka = lozinka;
            VrstaNaloga = vrstaNaloga;
            VrstaTreninga = vrstaTreninga;
            DatumClanarine = datumClanarine;
            Id = id;
        }

        public User()
        {
        }

        public void UpisUBazu(string LozinkaPotvrda, string DatumClanarineString)
        {
            DateTime DatumClanarine = DateTime.ParseExact(DatumClanarineString, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            if (ProveriPostojanjeKorisnika(Email))
            {
                MessageBox.Show("Korisnik sa datim username-om već postoji. Molimo izaberite drugi username.");
                return;
            }

            if (Lozinka != LozinkaPotvrda)
            {
                MessageBox.Show("Lozinka i lozinka za potvrdu se ne podudaraju. Molimo unesite iste vrednosti za lozinku.");
                return;
            }

            if (DatumClanarine < DateTime.Today)
            {
                MessageBox.Show("Datum članarine ne može biti u prošlosti. Molimo izaberite validan datum.");
                return;
            }
            string formatiraniDatum = DatumClanarine.ToString("dd/MM/yyyy");
            string[] arrVrstaNaloga = VrstaNaloga.Split(' ');
            VrstaNaloga = arrVrstaNaloga[arrVrstaNaloga.Length - 1];
            string[] arrVrstaTreninga = VrstaTreninga.Split(' ');
            VrstaTreninga = arrVrstaTreninga[arrVrstaTreninga.Length - 1];
            SqlConnection con = new SqlConnection("Data Source=THINKPAD;Initial Catalog=database;Integrated Security=True; trustServerCertificate=true");
            SqlCommand cmd = new SqlCommand("INSERT INTO korisnici (username, password, usertype, name, trainingtype, date) VALUES (@Email, @Lozinka, @VrstaNaloga, @ImePrezime, @VrstaTreninga, @DatumClanarine)", con);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Lozinka", Lozinka);
            cmd.Parameters.AddWithValue("@VrstaNaloga", VrstaNaloga);
            cmd.Parameters.AddWithValue("@ImePrezime", ImePrezime);
            cmd.Parameters.AddWithValue("@VrstaTreninga", VrstaTreninga);
            cmd.Parameters.AddWithValue("@DatumClanarine", formatiraniDatum);

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

        private bool ProveriPostojanjeKorisnika(string username)
        {
            string connectionString = "Data Source=THINKPAD;Initial Catalog=database;Integrated Security=True; trustServerCertificate=true";
            string query = "SELECT COUNT(*) FROM korisnici WHERE username = @Username";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);

                con.Open();
                int count = (int)cmd.ExecuteScalar();
                con.Close();

                return count > 0;
            }
        }

        public ObservableCollection<User> Pretrazi(string imePrezime, string korisnickoIme)
        {
            ObservableCollection<User> rezultati = new ObservableCollection<User>();

            string connectionString = "Data Source=THINKPAD;Initial Catalog=database;Integrated Security=True; trustServerCertificate=true";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM korisnici WHERE name = @ImePrezime OR username = @Email";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ImePrezime", imePrezime);
                cmd.Parameters.AddWithValue("@Email", korisnickoIme);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string imePrezimeDB = reader["name"].ToString();
                        string emailDB = reader["username"].ToString();
                        string lozinkaDB = reader["password"].ToString();
                        string vrstaNalogaDB = reader["usertype"].ToString();
                        string vrstaTreningaDB = reader["trainingtype"].ToString();
                        string datumClanarineDB = reader["date"].ToString();
                        int idDB = Convert.ToInt32(reader["id"].ToString());

                        User user = new User(imePrezimeDB, emailDB, lozinkaDB, vrstaNalogaDB, vrstaTreningaDB, datumClanarineDB, idDB);
                        rezultati.Add(user);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri pristupu bazi ili izvršavanju upita: " + ex.Message);
                }
            }

            return rezultati;
        }

        public User Prijava(string email, string lozinka)
        {
            string connectionString = "Data Source=THINKPAD;Initial Catalog=database;Integrated Security=True; trustServerCertificate=true";
            string query = "SELECT * FROM korisnici WHERE username=@Email AND password=@Lozinka" ;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Lozinka", lozinka);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        string imePrezime = reader["name"].ToString();
                        string vrstaTreninga = reader["trainingtype"].ToString();
                        string datumClanarine = reader["date"].ToString();
                        string vrstaNaloga = reader["usertype"].ToString();

                        User prijavljeniKorisnik = new User(imePrezime, email, lozinka, vrstaNaloga, vrstaTreninga, datumClanarine, id);
                        return prijavljeniKorisnik;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri pristupu bazi ili izvršavanju upita: " + ex.Message);
                    return null;
                }
            }
        }

        public void UpdateInDatabase()
        {
            string connectionString = "Data Source=THINKPAD;Initial Catalog=database;Integrated Security=True; trustServerCertificate=true";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE korisnici SET name = @ImePrezime, username = @Email, password = @Lozinka, usertype = @VrstaNaloga, trainingtype = @VrstaTreninga, date = @DatumClanarine WHERE id = @Id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Parameters.AddWithValue("@ImePrezime", ImePrezime);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Lozinka", Lozinka);
                cmd.Parameters.AddWithValue("@VrstaNaloga", VrstaNaloga);
                cmd.Parameters.AddWithValue("@VrstaTreninga", VrstaTreninga);
                cmd.Parameters.AddWithValue("@DatumClanarine", DatumClanarine);

                try
                {
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Podaci su uspešno ažurirani.");
                    }
                    else
                    {
                        MessageBox.Show("Nije bilo promena podataka za ažuriranje.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri ažuriranju podataka: " + ex.Message);
                }
            }
        }


        public void DeleteUser(int userId)
        {
            string connectionString = "Data Source=THINKPAD;Initial Catalog=database;Integrated Security=True; trustServerCertificate=true";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM korisnici WHERE id = @Id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", userId);

                try
                {
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Korisnik je uspešno obrisan.");
                    }
                    else
                    {
                        MessageBox.Show("Korisnik nije pronađen.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri brisanju korisnika: " + ex.Message);
                }
            }
        }
        public void RezervisiTrening(int idTreninga, int idKorisnika)
        {
            string connectionString = "Data Source=THINKPAD;Initial Catalog=database;Integrated Security=True; trustServerCertificate=true";
            string query = "INSERT INTO rezervacije (idKorinsnika, idTreninga) VALUES (@IdKorisnika, @IdTreninga)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@IdKorisnika", idKorisnika);
                cmd.Parameters.AddWithValue("@IdTreninga", idTreninga);

                try
                {
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Trening je uspešno rezervisan.");
                    }
                    else
                    {
                        MessageBox.Show("Neuspešna rezervacija treninga.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri pristupu bazi ili izvršavanju upita: " + ex.Message);
                }
            }
        }
        public List<Trening> ListRezervisaneTreninge(int idKorisnika)
        {
            List<Trening> trainingList = new List<Trening>();
            string connectionString = "Data Source=THINKPAD;Initial Catalog=database;Integrated Security=True; trustServerCertificate=true";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT t.* " +
                                "FROM treninzi t " +
                                "JOIN rezervacije r ON t.idTreninga = r.idTreninga " +
                                "WHERE r.idKorinsnika = @IdKorisnika ";


                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@IdKorisnika", idKorisnika);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        DateTime datumOdrzavanja = Convert.ToDateTime(reader["datumOdrzavanja"]);
                        if (datumOdrzavanja.Date > DateTime.Today) 
                        {
                            string pocetakTreninga = reader["pocetakTreninga"].ToString();
                            string krajTreninga = reader["krajTreninga"].ToString();
                            string vrstaTreninga = reader["vrstaTreninga"].ToString();
                            string instruktor = reader["instruktor"].ToString();
                            int kapacitet = Convert.ToInt32(reader["kapacitet"].ToString());
                            int idTreninga = Convert.ToInt32(reader["idTreninga"].ToString());
                            Trening trening = new Trening(idTreninga, datumOdrzavanja.ToString("dd/MM/yyyy"), pocetakTreninga, krajTreninga, vrstaTreninga, instruktor, kapacitet);
                            trainingList.Add(trening);
                        }
                    }

                    reader.Close();

                    if (trainingList.Count == 0)
                    {
                        MessageBox.Show("Nema rezervisanih treninga.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri pristupu bazi ili izvršavanju upita: " + ex.Message);
                }
            }

            return trainingList;
        }



        public bool ProveriIstekClanarine(int idKorisnika)
        {
            string connectionString = "Data Source=THINKPAD;Initial Catalog=database;Integrated Security=True; trustServerCertificate=true";
            string query = "SELECT date FROM korisnici WHERE id = @IdKorisnika";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@IdKorisnika", idKorisnika);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string datumClanarineDB = reader["date"].ToString();

                        DateTime datumClanarine = DateTime.ParseExact(datumClanarineDB, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        return datumClanarine >= DateTime.Today;
                    }
                    else
                    {
                        MessageBox.Show("Ne mozemo prona'i korisnika sa tim id-jem.");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri pristupu bazi ili izvršavanju upita: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
