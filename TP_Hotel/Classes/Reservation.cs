using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace TP_Hotel.Classes
{
    class Reservation
    {

        private int id;
        private List<Chambre> chambres;
        private ReservationStatut statut;
        private Client client;

        private decimal total;

        private static SqlCommand command;
        private static SqlDataReader reader;

        public int Id { get => id; set => id = value; }
        public List<Chambre> Chambres { get => chambres; set => chambres = value; }
        public ReservationStatut Statut { get => statut; set => statut = value; }
        public Client Client { get => client; set => client = value; }
        public decimal Total { get => total; set => total = value; }


        public Reservation()
        {

        }
        public Reservation(Client client)
        {
            Client = client;
            Chambres = new List<Chambre>();
        }

        public Reservation(ReservationStatut statut, Client client, decimal total = 0) : this(client)
        {
            Statut = statut;
            Total = total;
        }

        public Reservation(int id, ReservationStatut statut, Client client, decimal total = 0) : this(client)
        {
            this.id = id;
            Statut = statut;
            Total = total;
        }

        //Sauvegarder les réservations
        public bool SaveReservation()
        {
            string request = "INSERT INTO Reservation (statut, total, id_client) OUTPUT INSERTED.ID " +
                "values(@statut,@total,@id_client)";
            command = new SqlCommand(request, Tools.Connection);
            command.Parameters.Add(new SqlParameter("@statut", Statut.ToString()));
            command.Parameters.Add(new SqlParameter("@total", Total));
            command.Parameters.Add(new SqlParameter("@id_client", Client.Id));
            Tools.Connection.Open();
            id = (int)command.ExecuteScalar();
            command.Dispose();
            Tools.Connection.Close();
            return id > 0;
        }

        public bool Update()
        {
            string request = "UPDATE reservation set total=@total where id=@id";
            command = new SqlCommand(request, Tools.Connection);
            command.Parameters.Add(new SqlParameter("@total", Total));
            command.Parameters.Add(new SqlParameter("@id", Id));
            Tools.Connection.Open();
            int nbRow = command.ExecuteNonQuery();
            command.Dispose();
            Tools.Connection.Close();
            return nbRow == 1;
        }

        //Sauvegarder une chambre dans la table ReseravtionChambre
        public bool SaveRoomOnReservation(int id_chambre)
        {
            string request = "INSERT INTO Reservation_Chambre (id_reservation, id_chambre) OUTPUT INSERTED.ID_RESERVATION " +
                "values(@id_reservation,@id_chambre)";
            command = new SqlCommand(request, Tools.Connection);
            command.Parameters.Add(new SqlParameter("@id_reservation", Id));
            command.Parameters.Add(new SqlParameter("@id_chambre", id_chambre));
            Tools.Connection.Open();
            id = (int)command.ExecuteScalar();
            command.Dispose();
            Tools.Connection.Close();
            return id > 0;
        }
        
        //Recuperer la liste des chambres d'une reservation
        public static List<Chambre> GetReservationChambres(int id_reservation)
        {
            List<Chambre> chambres = new List<Chambre>();
            string request = "SELECT id_reservation, id_chambre from Reservation_Chambre " +
                "where id_reservation = @id_reservation";

            command = new SqlCommand(request, Tools.Connection);
            command.Parameters.Add(new SqlParameter("@id_reservation", id_reservation));
            Tools.Connection.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Chambre c = Chambre.GetChambreById(reader.GetInt32(1));
            }
            reader.Close();
            command.Dispose();
            Tools.Connection.Close();
            return chambres;
        }
    }

    enum ReservationStatut
    {
        Valide,
        Annule
    }
}
