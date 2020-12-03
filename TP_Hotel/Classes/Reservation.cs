using System;
using System.Collections.Generic;
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

        public Reservation(int id, ReservationStatut statut, Client client, decimal total = 0) : this(client)
        {
            this.id = id;
            Statut = statut;
            Total = total;
        }

        public void AjoutChambreReservation(Chambre chambre)
        {
            Chambres.Add(chambre);
        }

    }

    enum ReservationStatut
    {
        Valide,
        Annule
    }
}
