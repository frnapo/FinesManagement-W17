using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;

namespace esercizioW17D5.Models
{
    public class Trasgressore
    {
        // creo modello Trasgressore basato sui campi della tabella Trasgressore del db
        // creo le proprietà del modello con le data annotation per la validazione
        public int IDAnagrafica { get; set; }
        [DisplayName("Cognome")]
        [Required(ErrorMessage = "Il Cognome è obbligatorio")]
        [StringLength(30, MinimumLength = 3,
        ErrorMessage = "Il Cognome dev'essere minimo di 3 caratteri e massimo di 30")]
        public string Cognome { get; set; }
        [DisplayName("Nome")]
        [StringLength(30, MinimumLength = 3,
        ErrorMessage = "Il Nome dev'essere minimo di 3 caratteri e massimo di 30")]
        [Required(ErrorMessage = "Il campo Nome è obbligatorio")]
        public string Nome { get; set; }
        [DisplayName("Indirizzo")]
        [StringLength(250, MinimumLength = 10,
        ErrorMessage = "L'Indirizzo dev'essere minimo di 10 caratteri")]
        [Required(ErrorMessage = "Il campo Indirizzo è obbligatorio")]
        public string Indirizzo { get; set; }
        [DisplayName("Città")]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Il Città dev'essere minimo di 3 caratteri")]
        [Required(ErrorMessage = "Il campo Città è obbligatorio")]

        public string Citta { get; set; }
        [DisplayName("CAP")]
        [StringLength(5, MinimumLength = 5,
        ErrorMessage = "Il CAP dev'essere di 5 caratteri")]
        [Required(ErrorMessage = "Il campo CAP è obbligatorio")]
        public string CAP { get; set; }
        [DisplayName("Cod. Fisc.")]
        [StringLength(16, MinimumLength = 16,
        ErrorMessage = "Il Codice Fiscale dev'essere di 16 caratteri")]
        [Required(ErrorMessage = "Il campo Cod. Fisc. è obbligatorio")]
        public string Cod_Fisc { get; set; }
        // metodo per mostrare i trasgressore, 
        // restituisce una lista
        // lo richiamo nel controller
        public static List<Trasgressore> MostraTrasgressori()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["POLIZIA"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            List<Trasgressore> Trasgressori = new List<Trasgressore>();

            try
            {
                conn.Open();
                string query = "SELECT * FROM ANAGRAFICA";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Trasgressore trasgressore = new Trasgressore();
                    trasgressore.IDAnagrafica = Convert.ToInt32(reader["idanagrafica"]);
                    trasgressore.Cognome = reader["Cognome"].ToString();
                    trasgressore.Nome = reader["Nome"].ToString();
                    trasgressore.Indirizzo = reader["Indirizzo"].ToString();
                    trasgressore.Citta = reader["Città"].ToString();
                    trasgressore.CAP = reader["CAP"].ToString();
                    trasgressore.Cod_Fisc = reader["Cod_Fisc"].ToString();
                    Trasgressori.Add(trasgressore);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return Trasgressori;
        }


        // metodo statico void per creare un nuovo trasgressore
        // prende dei valori in input e li inserisce nel db
        // come per il metodo precedente lo richiamo nel controller
        public static void CreaTrasgressore(Trasgressore trasgressore)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["POLIZIA"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = "INSERT INTO ANAGRAFICA (Cognome, Nome, Indirizzo, Città, CAP, Cod_Fisc) VALUES (@Cognome, @Nome, @Indirizzo, @Città, @CAP, @Cod_Fisc)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Cognome", trasgressore.Cognome);
                cmd.Parameters.AddWithValue("@Nome", trasgressore.Nome);
                cmd.Parameters.AddWithValue("@Indirizzo", trasgressore.Indirizzo);
                cmd.Parameters.AddWithValue("@Città", trasgressore.Citta);
                cmd.Parameters.AddWithValue("@CAP", trasgressore.CAP);
                cmd.Parameters.AddWithValue("@Cod_Fisc", trasgressore.Cod_Fisc);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}