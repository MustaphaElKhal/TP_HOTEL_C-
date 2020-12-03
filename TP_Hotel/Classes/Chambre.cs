using System;
using System.Collections.Generic;
using System.Text;

namespace TP_Hotel.Classes
{
    class Chambre
    {
        private int numero;

        private int capacite;

        private ChambreStatut statut;

        private decimal tarif;

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
