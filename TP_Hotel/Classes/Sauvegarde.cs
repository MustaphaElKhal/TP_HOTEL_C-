using System;
using System.Collections.Generic;
using System.IO;

namespace TP_Hotel.Classes
{
    class Sauvegarde
    {
        private string clientFile = "clients.csv";
        private string roomFile = "rooms.csv";
        private string reservationFile = "reservations.csv";
        private string roomReservedFile = "roomsreserved.csv";

        public List<Client> GetClients()
        {
            List<Client> liste = new List<Client>();
            if (File.Exists(clientFile))
            {
                StreamReader reader = new StreamReader(clientFile);
                string ligne = reader.ReadLine();
                while (ligne != null)
                {
                    ligne = reader.ReadLine();
                    if (ligne != null)
                    {
                        string[] content = ligne.Split(';');
                        int id;
                        string nom = content[0];
                        string prenom = content[1];
                        string telephone = content[2];
                        Int32.TryParse(content[3], out id);

                        Client client = new Client(nom, prenom, telephone, id);
                        liste.Add(client);
                    }
                }
                reader.Close();
            }
            return liste;
        }

        public void SaveClients(List<Client> clients)
        {
            StreamWriter clientWriter = new StreamWriter(clientFile);
            clientWriter.WriteLine("nom;prenom;telephone;id");
            foreach (Client c in clients)
            {
                clientWriter.WriteLine($"{c.Nom};{c.Prenom};{c.Telephone};{c.Id}");
            }
            clientWriter.Close();
        }

        public List<Chambre> GetRooms()
        {
            List<Chambre> liste = new List<Chambre>();
            if (File.Exists(roomFile))
            {
                StreamReader reader = new StreamReader(roomFile);
                string ligne = reader.ReadLine();
                while (ligne != null)
                {
                    ligne = reader.ReadLine();
                    if (ligne != null)
                    {
                        string[] content = ligne.Split(';');
                        int numero;
                        Int32.TryParse(content[0], out numero);
                        int capacite;
                        Int32.TryParse(content[1], out capacite);
                        ChambreStatut statut;
                        Enum.TryParse(content[2], out statut);
                        decimal tarif;
                        decimal.TryParse(content[3], out tarif);

                        Chambre chambre = new Chambre(numero, capacite, statut, tarif);
                        liste.Add(chambre);
                    }
                }
                reader.Close();
            }
            return liste;
        }

        public void SaveRooms(List<Chambre> chambres)
        {
            StreamWriter roomWriter = new StreamWriter(roomFile);
            roomWriter.WriteLine("numero;capacite;statut;tarif");
            foreach (Chambre c in chambres)
            {
                roomWriter.WriteLine($"{c.Numero};{c.Capacite};{c.Statut};{c.Tarif}");
            }
            roomWriter.Close();
        }


        public List<Reservation> GetReservations()
        {
            List<Reservation> liste = new List<Reservation>();
            if (File.Exists(reservationFile))
            {
                StreamReader reader = new StreamReader(reservationFile);
                string ligne = reader.ReadLine();
                while (ligne != null)
                {
                    ligne = reader.ReadLine();
                    if (ligne != null)
                    {
                        string[] content = ligne.Split(';');

                        int idReservation;
                        Int32.TryParse(content[0], out idReservation);

                        ReservationStatut statutReservation;
                        Enum.TryParse(content[1], out statutReservation);

                        int id;
                        string nom = content[2];
                        string prenom = content[3];
                        string telephone = content[4];
                        Int32.TryParse(content[5], out id);


                        decimal total;
                        decimal.TryParse(content[6], out total);

                        Client client = new Client(nom, prenom, telephone, id);
                        Reservation reservation = new Reservation(idReservation, statutReservation, client, total);

                        //Recupérer les chambres reservées à partir du fichier des réservations
                        reservation.Chambres = GetRoomsReserved(idReservation);
                        liste.Add(reservation);
                    }
                }
                reader.Close();
            }
            return liste;
        }

        public void SaveReservations(List<Reservation> reservations)
        {

            StreamWriter reservationWriter = new StreamWriter(reservationFile);
            StreamWriter roomreservationWriter = new StreamWriter(roomReservedFile);
            reservationWriter.WriteLine("id;statut;nom;prenom;telephone;idClient;total");
            roomreservationWriter.WriteLine("id;numeroChambre;capacite;statut;tarif");
            foreach (Reservation r in reservations)
            {
                reservationWriter.WriteLine($"{r.Id};{r.Statut};{r.Client.Nom};{r.Client.Prenom};{r.Client.Telephone};{r.Client.Id};{r.Total}");
                foreach (Chambre c in r.Chambres)
                {
                    roomreservationWriter.WriteLine($"{r.Id};{c.Numero};{c.Capacite};{c.Statut};{c.Tarif}");
                }
            }
            reservationWriter.Close();
            roomreservationWriter.Close();
        }

        public List<Chambre> GetRoomsReserved(int numero)
        {
            List<Chambre> liste = new List<Chambre>();
            if (File.Exists(roomReservedFile))
            {
                StreamReader reader = new StreamReader(roomReservedFile);
                string ligne = reader.ReadLine();
                while (ligne != null)
                {
                    ligne = reader.ReadLine();
                    if (ligne != null)
                    {
                        string[] content = ligne.Split(';');

                        int idReservation;
                        Int32.TryParse(content[0], out idReservation);
                        if (idReservation == numero)
                        {
                            int numchambre;
                            Int32.TryParse(content[1], out numchambre);
                            int capacite;
                            Int32.TryParse(content[2], out capacite);
                            ChambreStatut statut;
                            Enum.TryParse(content[3], out statut);

                            decimal tarif;
                            decimal.TryParse(content[4], out tarif);

                            Chambre chambre = new Chambre(numchambre, capacite, statut, tarif);
                            liste.Add(chambre);
                        }
                    }
                }
                reader.Close();
            }
            return liste;
        }
    }
}

