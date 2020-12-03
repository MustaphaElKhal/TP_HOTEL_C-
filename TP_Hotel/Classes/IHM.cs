using System;
using System.Collections.Generic;
using System.Text;

namespace TP_Hotel.Classes
{
    class IHM
    {
        private Hotel hotel;

        public IHM()
        {
            hotel = new Hotel();
        }
        public void Start()
        {
            string choix;
            do
            {
                MenuPrincipal();
                choix = Console.ReadLine();
                Console.Clear();
                switch (choix)
                {
                    case "1":
                        ActionReservation();
                        break;
                    case "2":
                        ActionClient();
                        break;
                    case "0":
                        hotel.Sauvegarde();
                        Environment.Exit(0);
                        break;
                }
            } while (choix != "0");
        }

        private void MenuPrincipal()
        {
            Console.WriteLine("1--- Gestion Réservation");
            Console.WriteLine("2--- Gestion client");
        }

        private void MenuReservation()
        {
            Console.WriteLine("1--- Reserver une chambre");
            Console.WriteLine("2--- Liberer une chambre");
        }

        private void MenuClient()
        {
            Console.WriteLine("1--- Ajouter un client");
            Console.WriteLine("2--- Rechercher un client");
        }

        private void SousMenuClient()
        {
            Console.WriteLine("=====Sous Menu client =====");
            string choix;
            do
            {
                SousMenuGestionClient();
                choix = Console.ReadLine();
                switch (choix)
                {
                    case "1":
                        Console.WriteLine("Modification");
                        break;
                    case "2":
                        Console.WriteLine("Suppression");
                        break;
                }
            } while (choix != "0");
        }

        private void SousMenuGestionClient()
        {
            Console.WriteLine("1--- Modifier le client");
            Console.WriteLine("2--- Supprimer le client");
        }

        private void ActionReservation()
        {
            Console.WriteLine("=====Menu reservation =====");
            string choix;
            do
            {
                MenuReservation();
                choix = Console.ReadLine();
                switch (choix)
                {
                    case "1":
                        ActionAffichageChambresLibres();
                        ActionReserverChambreLibre();
                        break;
                    case "2":
                        ActionLibererChambre();
                        break;
                }
            } while (choix != "0");
        }

        private void ActionClient()
        {
            Console.WriteLine("=====Menu client =====");
            string choix;
            do
            {
                MenuClient();
                choix = Console.ReadLine();
                switch (choix)
                {
                    case "1":
                        ActionAjouterClient();
                        break;
                    case "2":
                        ActionAffichageClient();
                        SousMenuClient();
                        break;
                    case "0":
                        hotel.Sauvegarde();
                        break;
                }
            } while (choix != "0");
        }

        private Client ActionAjouterClient()
        {
            Console.WriteLine("********************** Création d'un client **********************");
            Client client = new Client();
            Tools.TryParseProperty("Nom : ", client, "Nom");
            Tools.TryParseProperty("Prénom : ", client, "Prenom");
            Tools.TryParseProperty("Téléphone : ", client, "Telephone");
            client.Id = hotel.LastClientNumber() + 1;
            hotel.Clients.Add(client);
            Console.WriteLine("Client crée avec le numéro " + client.Id);
            return client;
        }

        private Client ActionChercherClient()
        {
            Console.Write("Tel du client : ");
            string telephone = (Console.ReadLine());
            Client client = hotel.GetClientByTel(telephone);
            return client;
        }

        private void ActionAffichageClient()
        {
            Client client = ActionChercherClient();
            if (client == null)
            {
                Console.WriteLine("Aucun client avec ce numero");
                Console.WriteLine("\n \n******************************************************************* \n \n");
            }
            else
            {
                Console.WriteLine(client);
                Console.WriteLine("\n \n******************************************************************* \n \n");
            }
        }

        private List<Chambre> ActionChercherChambresLibres()
        {
            List<Chambre> chambres = hotel.GetChambresLibres();
            return chambres;
        }

        private void ActionAffichageChambresLibres()
        {
            List<Chambre> chambres = ActionChercherChambresLibres();
            if (chambres.Count > 0)
            {
                Console.WriteLine("Liste des chambres");
                foreach (Chambre c in chambres)
                {
                    Console.WriteLine(c);
                }
                Console.WriteLine("\n \n******************************************************************* \n \n");
            }
            else
            {
                Console.WriteLine("Hotel complet");
                Console.WriteLine("\n \n******************************************************************* \n \n");
            }
        }

        private void ActionReserverChambreLibre()
        {
            Console.Write("Nvo client 1 | Déja client 2 : ");
            int numero = Convert.ToInt32(Console.ReadLine());
            decimal total = 0;
            if (numero == 1)
            {
                //NvoClient
                Client client = ActionAjouterClient();
                hotel.Sauvegarde();
                Console.WriteLine(client);
                Chambre chambreNvoClient = ActionChercherChambre();
                if (chambreNvoClient == null)
                {
                    Console.WriteLine("Aucune chambre avec ce numero");
                }
                else
                {
                    if (chambreNvoClient.Statut == ChambreStatut.Occupe)
                    {
                        Console.WriteLine("Impossible la chambre est déjà occupee");
                    }
                    else
                    {
                        numero = chambreNvoClient.Numero;
                        hotel.ReserverChambreById(numero);
                        Console.WriteLine($"\n\nChambre {numero} maintenant occupe\n\n");
                    }
                }
            }
            else if (numero == 2)
            {
                //DejaClient
                Client client = ActionChercherClient();
                if (client == null)
                {
                    Console.WriteLine("Aucun client avec ce numero");
                }
                else
                {
                    Console.WriteLine(client);
                    string continuer;
                    int id = hotel.LastReservationNumber() + 1;
                    Reservation res = new Reservation(id, ReservationStatut.Valide, client);
                    do
                    {
                        Console.WriteLine("nouvelle reservation (o) / (n)");
                        continuer = Console.ReadLine();
                        if (continuer == "o")
                        {
                            Chambre chambreDejaClient = ActionChercherChambre();
                            hotel.ReserverChambreById(chambreDejaClient.Numero);
                            res.Chambres.Add(chambreDejaClient);
                        } else if (continuer == "n")
                        {
                            foreach (Chambre c in res.Chambres)
                            {
                                total += c.Tarif;
                            }
                            res.Total = total;
                            hotel.Reservations.Add(res);
                        }
                    } while (continuer == "o");
                    
                    hotel.Sauvegarde();
                    Console.WriteLine($"\n\nChambre {numero} maintenant occupe\n\n");
                }
            }
        }

        private Chambre ActionChercherChambre()
        {
            Console.Write("num chambre : ");
            int numero = Convert.ToInt32(Console.ReadLine());
            Chambre chambre = hotel.GetChambreById(numero);
            return chambre;
        }

        private void ActionLibererChambre()
        {
            Chambre chambre = ActionChercherChambre();
            if (chambre == null)
            {
                Console.WriteLine("Aucune chambre avec ce numero");
            }
            else
            {
                if (chambre.Statut == ChambreStatut.Libre)
                {
                    Console.WriteLine("Impossible la chambre est déjà libre");
                }
                else
                {
                    int num = chambre.Numero;
                    hotel.LibererChambreById(num);
                    Console.WriteLine($"\n\nChambre {num} de nouveau libre\n\n");
                }
            }
        }
    }
}
