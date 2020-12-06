using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace TP_Hotel.Classes
{
    class Chambre
    {
        private int numero;

        private int capacite;

        private ChambreStatut statut;

        private decimal tarif;

        private static SqlCommand command;
        private static SqlDataReader reader;

        public int Numero { get => numero; set => numero = value; }
        public int Capacite { get => capacite; set => capacite = value; }
        public ChambreStatut Statut { get => statut; set => statut = value; }
        public decimal Tarif { get => tarif; set => tarif = value; }

        public Chambre(int numero, int capacite, ChambreStatut statut, decimal tarif)
        {
            Numero = numero;
            Capacite = capacite;
            Statut = statut;
            Tarif = tarif;
        }
        //Mise à jour du statut d'une chambre
        public bool UpdateChambre()
        {
            string request = "UPDATE chambre set statut=@statut where id=@id";
            command = new SqlCommand(request, Tools.Connection);
            command.Parameters.Add(new SqlParameter("@statut", Statut.ToString()));
            command.Parameters.Add(new SqlParameter("@id", Numero));
            Tools.Connection.Open();
            int nbRow = command.ExecuteNonQuery();
            command.Dispose();
            Tools.Connection.Close();
            return nbRow == 1;
        }

        //Recuperer une chambre en fonction de son id
        public static Chambre GetChambreById(int id)
        {
            Chambre chambre = null;
            string request = "SELECT capacite, statut, tarif FROM Chambre where id = @id";
            command = new SqlCommand(request, Tools.Connection);
            command.Parameters.Add(new SqlParameter("@id", id));
            Tools.Connection.Open();
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                ChambreStatut statutChambre;
                Enum.TryParse(reader.GetString(1), out statutChambre);
                chambre = new Chambre(id, reader.GetInt32(0), statutChambre, reader.GetDecimal(2));
            }
            reader.Close();
            command.Dispose();
            Tools.Connection.Close();
            return chambre;
        }

        public static List<Chambre> Getchambres()
        {
            List < Chambre > chambres = new List<Chambre>();
            string request = "SELECT * FROM Chambre";
            command = new SqlCommand(request, Tools.Connection);
            Tools.Connection.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                ChambreStatut statutChambre;
                Enum.TryParse(reader.GetString(2), out statutChambre);
                Chambre c = new Chambre(reader.GetInt32(0), reader.GetInt32(1), statutChambre, reader.GetDecimal(3));
                if (c.Statut == ChambreStatut.Libre)
                {
                    chambres.Add(c);
                }
            }
            reader.Close();
            command.Dispose();
            Tools.Connection.Close();
            return chambres;
        }

        public override string ToString()
        {
            return $"Numero : {Numero}, Capacite : {Capacite}, Statut : {Statut}, Tarif : {Tarif}";
        }
    }

    enum ChambreStatut
    {
        Libre,
        Occupe
    }
}
