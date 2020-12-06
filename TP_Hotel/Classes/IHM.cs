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
                        Environment.Exit(0);
                        break;
                }
            } while (choix != "0");
        }

        #region Partie Reservation
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
                        if (ActionAffichageChambresLibres())
                        {
                            ActionReserverChambreLibre();
                        }
                        else
                        {
                            Console.WriteLine("Hotel complet");
                        }
                        break;
                    case "2":
                        ActionLibererChambre();
                        break;
                }
            } while (choix != "0");
        }
        private bool ActionAffichageChambresLibres()
        {
            List<Chambre> chambres = Chambre.Getchambres();
            if (chambres.Count > 0)
            {
                Console.WriteLine("Liste des chambres");
                foreach (Chambre c in chambres)
                {
                    Console.WriteLine(c);
                }
                Console.WriteLine("\n \n******************************************************************* \n \n");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ActionReserverChambreLibre()
        {
            Console.Write("Nvo client 1 | Déja client 2 : ");
            int numero = Convert.ToInt32(Console.ReadLine());
            if (numero == 1)
            {
                Client client = ActionAjouterClient();
                ActionReservationDejaClient();
            }
            else if (numero == 2)
            {
                ActionReservationDejaClient();
            }
        }
        private void ActionReservationDejaClient()
        {
            Client client = ActionChercherClient();
            if (client == null)
            {
                Console.WriteLine("Aucun client avec ce numero");
            }
            else
            {
                string continuer;
                int count = 0;
                Reservation res = new Reservation(ReservationStatut.Valide, client);

                if (res.SaveReservation())
                {
                    do
                    {
                        if (count == 0)
                        {
                            continuer = "o";
                        }
                        else
                        {
                            Console.WriteLine("ajouter nouvelle chambre dans la reservation (o) / (n)");
                            continuer = Console.ReadLine();
                            count++;
                        }
                        if (continuer == "o")
                        {
                            count++;
                            Chambre chambreDejaClient = ActionChercherChambre();
                            if (chambreDejaClient.Statut == ChambreStatut.Occupe)
                            {
                                Console.WriteLine("Impossible la chambre est déjà occupee");
                            }
                            else
                            {
                                if (res.SaveRoomOnReservation(chambreDejaClient.Numero))
                                {
                                    Console.WriteLine("Reservation de la chambre crée");
                                    res.Chambres.Add(chambreDejaClient);
                                    chambreDejaClient.Statut = ChambreStatut.Occupe;
                                    chambreDejaClient.UpdateChambre();
                                }
                                else
                                {
                                    Console.WriteLine("Erreur base de données");
                                }
                            }

                        }
                        else if ((continuer == "n"))
                        {
                            decimal total = 0M;
                            foreach (Chambre c in res.Chambres)
                            {
                                total += c.Tarif;
                            }
                            res.Total = total;
                            res.Update();
                        }
                    } while (continuer == "o");
                }
                else
                {
                    Console.WriteLine("Erreur base de données");
                }
            }
        }
        private Chambre ActionChercherChambre()
        {
            Console.Write("num chambre : ");
            int numero = Convert.ToInt32(Console.ReadLine());
            Chambre chambre = Chambre.GetChambreById(numero);
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
                    chambre.Statut = ChambreStatut.Libre;
                    chambre.UpdateChambre();
                }
            }
        }
        #endregion

        #region Partie Client
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
                        break;
                }
            } while (choix != "0");
        }
        private Client ActionChercherClient()
        {
            Console.Write("Id du client : ");
            int id = Convert.ToInt32(Console.ReadLine());
            Client client = Client.GetClientById(id);
            return client;
        }
        private Client ActionAjouterClient()
        {
            Console.WriteLine("********************** Création d'un client **********************");
            Client client = new Client();
            Tools.TryParseProperty("Nom : ", client, "Nom");
            Tools.TryParseProperty("Prénom : ", client, "Prenom");
            Tools.TryParseProperty("Téléphone : ", client, "Telephone");

            if (client.Save())
            {
                Console.WriteLine("Client crée avec le numéro " + client.Id);
            }
            else
            {
                Console.WriteLine("Erreur base de données");
            }
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
                SousMenuClient(client);
            }
        }

        private void SousMenuClient(Client client)
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
                        ActionModifierClient(client);
                        break;
                    case "2":
                        ActionSupprimerClient(client);
                        break;
                }
            } while (choix != "0");
        }
        private void ActionSupprimerClient(Client client)
        {
            if (client != null)
            {
                if (client.Delete())
                {
                    Console.WriteLine("Suppression effectuée");
                }
                else
                {
                    Console.WriteLine("Erreur base de données");
                }
            }
            else
            {
                Console.WriteLine("Aucun client avec cet id");
            }
        }
        private void ActionModifierClient(Client client)
        {
            if (client != null)
            {
                Console.Write("Merci de saisir le nouveau numéro de téléphone : ");
                client.Telephone = Console.ReadLine();

                if (client.Update())
                {
                    Console.WriteLine("Modification effectuée");
                }
                else
                {
                    Console.WriteLine("Erreur base de données");
                }
            }
            else
            {
                Console.WriteLine("Aucun client avec cet id");
            }
        }
        #endregion

        #region Affichage des choix possibles dans les Menus
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

        private void SousMenuGestionClient()
        {
            Console.WriteLine("1--- Modifier le client");
            Console.WriteLine("2--- Supprimer le client");
        }

        #endregion

    }
}
