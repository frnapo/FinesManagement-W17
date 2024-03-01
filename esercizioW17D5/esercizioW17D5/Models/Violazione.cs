using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;

namespace esercizioW17D5.Models
{
    public class Violazione
    {
        // creo modello Violazione basato sui campi della tabella Violazione del db
        // creo le proprietà del modello con le data annotation per la validazione
        public int IDViolazione { get; set; }
        [DisplayName("Nome violazione")]
        [Required(ErrorMessage = "Inserire la violazione")]
        [StringLength(250, MinimumLength = 5,
        ErrorMessage = "Inserisci una violazione valida")]
        public string Descrizione { get; set; }
        public bool Contestabile { get; set; }



        public static List<Violazione> MostraViolazioni()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["POLIZIA"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            List<Violazione> Violazioni = new List<Violazione>();

            try
            {
                conn.Open();
                //avrei fatto cosi' ma poi per come ho gestito verbali non sarebbe andato bene essenso che i campi con
                //contestabile = 0 non sarebbero stati mostrati accanto ai trasgressori
                //string query = "SELECT * FROM TIPO_VIOLAZIONE WHERE Contestabile = 1";
                string query = "SELECT * FROM TIPO_VIOLAZIONE";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Violazione violazione = new Violazione();
                    violazione.IDViolazione = Convert.ToInt32(reader["idviolazione"]);
                    violazione.Descrizione = reader["descrizione"].ToString();
                    violazione.Contestabile = Convert.ToBoolean(reader["contestabile"]);
                    Violazioni.Add(violazione);
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
            return Violazioni;
        }

        public static void CreaViolazione(Violazione violazione)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["POLIZIA"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = "INSERT INTO TIPO_VIOLAZIONE (descrizione, contestabile) VALUES (@descrizione, @contestabile)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@descrizione", violazione.Descrizione);
                cmd.Parameters.AddWithValue("@contestabile", violazione.Contestabile);
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