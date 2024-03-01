using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace esercizioW17D5.Models
{
    public class Verbale
    {
        public int IDVerbale { get; set; }
        [DisplayName("Data Violazione")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataViolazione { get; set; }
        [DisplayName("Indirizzo Violazione")]
        [StringLength(250, MinimumLength = 10,
        ErrorMessage = "L'Indirizzo violazione dev'essere minimo di 10 caratteri")]
        [Required(ErrorMessage = "Il campo Indirizzo è obbligatorio")]
        public string IndirizzoViolazione { get; set; }
        [DisplayName("Agente")]
        public string NominativoAgente { get; set; }
        [DisplayName("Data Trascrizione")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataTrascrizioneVerbale { get; set; }
        [DisplayName("Importo")]
        public decimal Importo { get; set; }
        [DisplayName("Punti Decurtati")]
        public int DecurtamentoPunti { get; set; }
        public int IDAnagrafica { get; set; }
        public int IDViolazione { get; set; }

        public Trasgressore Trasgressore { get; set; }
        public Violazione Violazione { get; set; }


        // metodo per mostrare i verbali,
        // restituisce una lista
        // lo richiamo nel controller
        public static List<Verbale> MostraVerbali()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["POLIZIA"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            List<Verbale> Verbali = new List<Verbale>();

            try
            {
                conn.Open();
                string query = "SELECT * FROM VERBALE";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Verbale verbale = new Verbale();
                    verbale.IDVerbale = Convert.ToInt32(reader["idverbale"]);
                    verbale.DataViolazione = Convert.ToDateTime(reader["DataViolazione"]);
                    verbale.IndirizzoViolazione = reader["IndirizzoViolazione"].ToString();
                    verbale.NominativoAgente = reader["Nominativo_Agente"].ToString();
                    verbale.DataTrascrizioneVerbale = Convert.ToDateTime(reader["DataTrascrizioneVerbale"]);
                    verbale.Importo = Convert.ToDecimal(reader["Importo"]);
                    verbale.DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"]);
                    verbale.IDAnagrafica = Convert.ToInt32(reader["idanagrafica"]);
                    verbale.Trasgressore = Trasgressore.MostraTrasgressori().FirstOrDefault(t => t.IDAnagrafica == verbale.IDAnagrafica);
                    verbale.IDViolazione = Convert.ToInt32(reader["idviolazione"]);
                    verbale.Violazione = Violazione.MostraViolazioni().FirstOrDefault(v => v.IDViolazione == verbale.IDViolazione);
                    Verbali.Add(verbale);
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
            return Verbali;
        }


        // metodo statico void per creare un nuovo verbale
        // prende dei valori in input e li inserisce nel db
        // come per il metodo precedente lo richiamo nel controller
        public static void CreaVerbale(Verbale verbale)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["POLIZIA"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "INSERT INTO VERBALE (DataViolazione, IndirizzoViolazione, Nominativo_Agente, DataTrascrizioneVerbale, Importo, DecurtamentoPunti, idanagrafica, idviolazione) VALUES (@DataViolazione, @IndirizzoViolazione, @Nominativo_Agente, @DataTrascrizioneVerbale, @Importo, @DecurtamentoPunti, @idanagrafica, @idviolazione)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DataViolazione", verbale.DataViolazione);
                cmd.Parameters.AddWithValue("@IndirizzoViolazione", verbale.IndirizzoViolazione);
                cmd.Parameters.AddWithValue("@Nominativo_Agente", verbale.NominativoAgente);
                cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", verbale.DataTrascrizioneVerbale);
                cmd.Parameters.AddWithValue("@Importo", verbale.Importo);
                cmd.Parameters.AddWithValue("@DecurtamentoPunti", verbale.DecurtamentoPunti);
                cmd.Parameters.AddWithValue("@idanagrafica", verbale.IDAnagrafica);
                cmd.Parameters.AddWithValue("@idviolazione", verbale.IDViolazione);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Errore SQL: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // metodo statico per ottenere i verbali raggruppati per trasgressore
        // lo richiamo nel controller
        public static List<Verbale> VerbaliXTrasgressore()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["POLIZIA"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            List<Verbale> Verbali = new List<Verbale>();

            try
            {
                conn.Open();
                string query = "SELECT idanagrafica, COUNT(*) AS TotaleVerbaliXAnagrafe FROM VERBALE GROUP BY idanagrafica ORDER BY COUNT(*) DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Verbale verbale = new Verbale();
                    verbale.IDAnagrafica = Convert.ToInt32(reader["idanagrafica"]);
                    Trasgressore trasgressore = Trasgressore.MostraTrasgressori().FirstOrDefault(t => t.IDAnagrafica == verbale.IDAnagrafica);
                    verbale.Trasgressore = trasgressore;
                    verbale.IDVerbale = Convert.ToInt32(reader["TotaleVerbaliXAnagrafe"]);
                    Verbali.Add(verbale);

                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Errore SQL: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return Verbali;
        }

        //metodo statico per ottenere il totale dei punti decurtati
        //raggruppati per trasgressore
        //lo richiamo nel controller
        public static List<Verbale> PuntiDecurtatiXTrasgressore()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["POLIZIA"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            List<Verbale> Verbali = new List<Verbale>();

            try
            {
                conn.Open();
                string query = "SELECT idanagrafica, SUM(DecurtamentoPunti) AS TotalePuntiDecurtatiXAnagrafe FROM VERBALE GROUP BY idanagrafica ORDER BY SUM(DecurtamentoPunti) DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Verbale verbale = new Verbale();
                    verbale.IDAnagrafica = Convert.ToInt32(reader["idanagrafica"]);
                    Trasgressore trasgressore = Trasgressore.MostraTrasgressori().FirstOrDefault(t => t.IDAnagrafica == verbale.IDAnagrafica);
                    verbale.Trasgressore = trasgressore;
                    verbale.DecurtamentoPunti = Convert.ToInt32(reader["TotalePuntiDecurtatiXAnagrafe"]);
                    Verbali.Add(verbale);

                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Errore SQL: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return Verbali;
        }


        //metodo statico che restituisce importo, cognome, nome, data violazione e decurtamento punti
        // delle violazioni che superano i 10 punti
        // lo richiamo nel controller

        public static List<Verbale> VerbaliPuntiMaggioriDi10()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["POLIZIA"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            List<Verbale> Verbali = new List<Verbale>();

            try
            {
                conn.Open();
                string query = "SELECT Importo, DecurtamentoPunti, ANAGRAFICA.Cognome, ANAGRAFICA.Nome, DataViolazione, ANAGRAFICA.idanagrafica FROM VERBALE INNER JOIN ANAGRAFICA ON VERBALE.idanagrafica = ANAGRAFICA.idanagrafica WHERE DecurtamentoPunti > 10";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Verbale verbale = new Verbale();
                    verbale.Importo = Convert.ToDecimal(reader["Importo"]);
                    verbale.DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"]);
                    int idAnagrafica = Convert.ToInt32(reader["idanagrafica"]);
                    verbale.Trasgressore = Trasgressore.MostraTrasgressori().FirstOrDefault(t => t.IDAnagrafica == idAnagrafica);
                    verbale.DataViolazione = Convert.ToDateTime(reader["DataViolazione"]);
                    Verbali.Add(verbale);
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
            return Verbali;
        }



        // metodo statico per ottenere i verbali con importo maggiore di 400
        // lo richiamo nel controller
        // restituisce una lista
        public static List<Verbale> VerbaliMaggioriDi400()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["POLIZIA"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            List<Verbale> Verbali = new List<Verbale>();

            try
            {
                conn.Open();
                string query = "SELECT * FROM VERBALE WHERE Importo > 400";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Verbale verbale = new Verbale();
                    verbale.IDVerbale = Convert.ToInt32(reader["idverbale"]);
                    verbale.DataViolazione = Convert.ToDateTime(reader["DataViolazione"]);
                    verbale.IndirizzoViolazione = reader["IndirizzoViolazione"].ToString();
                    verbale.NominativoAgente = reader["Nominativo_Agente"].ToString();
                    verbale.DataTrascrizioneVerbale = Convert.ToDateTime(reader["DataTrascrizioneVerbale"]);
                    verbale.Importo = Convert.ToDecimal(reader["Importo"]);
                    verbale.DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"]);
                    verbale.IDAnagrafica = Convert.ToInt32(reader["idanagrafica"]);
                    verbale.Trasgressore = Trasgressore.MostraTrasgressori().FirstOrDefault(t => t.IDAnagrafica == verbale.IDAnagrafica);
                    verbale.IDViolazione = Convert.ToInt32(reader["idviolazione"]);
                    verbale.Violazione = Violazione.MostraViolazioni().FirstOrDefault(v => v.IDViolazione == verbale.IDViolazione);
                    Verbali.Add(verbale);
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
            return Verbali;
        }
    }
}