using System;
using System.Collections.Generic;
using System.Text;

namespace TP_Hotel.Classes
{
    class Client
    {
        int id;
        string nom;
        string prenom;
        string telephone;

        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public string Telephone { get => telephone; set => telephone = value; }
        public int Id { get => id; set => id = value; }

        public Client()
        {

        }

        public Client(string nom, string prenom, string telephone, int id)
        {
            Nom = nom;
            Prenom = prenom;
            Telephone = telephone;
            Id = id;
        }

        public override string ToString()
        {
            return $"Nom : {Nom}, Prénom : {Prenom}, Téléphone : {Telephone}";
        }
    }
}
