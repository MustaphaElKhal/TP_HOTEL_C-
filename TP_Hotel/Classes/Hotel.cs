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

        public string Nom { get => nom; set => nom = value; }
        public string Adresse { get => adresse; set => adresse = value; }
        public string Telephone { get => telephone; set => telephone = value; }
        public List<Chambre> Chambres { get => chambres; set => chambres = value; }
        public List<Reservation> Reservations { get => reservations; set => reservations = value; }
        public List<Client> Clients { get => clients; set => clients = value; }

        public Hotel()
        {
        }

        public Client GetClientByTel(string tel)
        {
            return Clients.Find(c => c.Telephone == tel);
        }
    }
}
