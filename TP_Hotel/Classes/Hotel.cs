using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TP_Hotel.Classes
{
    class Hotel
    {
        private string nom;
        private string adresse;
        private string telephone;

        private List<Chambre> chambres;
        private List<Reservation> reservations;
        private List<Client> clients;

        private Sauvegarde sv;
        public string Nom { get => nom; set => nom = value; }
        public string Adresse { get => adresse; set => adresse = value; }
        public string Telephone { get => telephone; set => telephone = value; }
        public List<Chambre> Chambres { get => chambres; set => chambres = value; }
        public List<Reservation> Reservations { get => reservations; set => reservations = value; }
        public List<Client> Clients { get => clients; set => clients = value; }

        public Hotel()
        {
            sv = new Sauvegarde();
            Clients = sv.GetClients();
            Chambres = sv.GetRooms();
            Reservations = sv.GetReservations();
        }

        public Client GetClientByTel(string tel)
        {
            return Clients.Find(c => c.Telephone == tel);
        }


        public Chambre GetChambreById(int num)
        {
            return Chambres.Find(c => c.Numero == num);
        }
        public List<Chambre> GetChambresLibres()
        {
            return Chambres.FindAll(c => c.Statut == 0);
        }

        public Chambre ReserverChambreById(int num)
        {
            Chambre chambre = Chambres.Find(c => c.Numero == num);
            chambre.Statut = ChambreStatut.Occupe;
            sv.SaveRooms(Chambres);
            return chambre;
        }

        public void LibererChambreById(int num)
        {
            Chambre chambre = Chambres.Find(c => c.Numero == num);
            chambre.Statut = ChambreStatut.Libre;
            sv.SaveRooms(Chambres);
        }

        public void Sauvegarde()
        {
            sv.SaveClients(Clients);
            sv.SaveRooms(Chambres);
            sv.SaveReservations(Reservations);
        }

        public int LastClientNumber()
        {
            return Clients.Count;
        }

        public int LastReservationNumber()
        {
            return Reservations.Count;
        }
    }
}
