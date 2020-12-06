using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace TP_Hotel.Classes
{
    class Client
    {
        int id;
        string nom;
        string prenom;
        string telephone;

        private static SqlCommand command;
        private static SqlDataReader reader;

        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public string Telephone { get => telephone; set => telephone = value; }
        public int Id { get => id; set => id = value; }

        public Client()
        {

        }

        public Client(string nom, string prenom, string telephone)
        {
            Nom = nom;
            Prenom = prenom;
            Telephone = telephone;
        }

        public Client(int id, string nom, string prenom, string telephone) : this(nom, prenom, telephone)
        {
            Id = id;
        }
        //Ajouter un client
        public bool Save()
        {
            string request = "INSERT INTO Client (nom, prenom, telephone) OUTPUT INSERTED.id values (@nom, @prenom, @telephone)";
            command = new SqlCommand(request, Tools.Connection);
            command.Parameters.Add(new SqlParameter("@nom", Nom));
            command.Parameters.Add(new SqlParameter("@prenom", Prenom));
            command.Parameters.Add(new SqlParameter("@telephone", Telephone));

            Tools.Connection.Open();
            Id = (int)command.ExecuteScalar();
            command.Dispose();
            Tools.Connection.Close();

            return Id > 0;
        }

        //supprimer un client
        public bool Delete()
        {
            string request = "DELETE FROM client where id=@id";
            command = new SqlCommand(request, Tools.Connection);
            command.Parameters.Add(new SqlParameter("@id", Id));
            Tools.Connection.Open();
            int nbRow = command.ExecuteNonQuery();
            command.Dispose();
            Tools.Connection.Close();
            return nbRow == 1;
        }

        //Mise à jour d'un client
        public bool Update()
        {
            string request = "UPDATE client set nom=@nom, prenom=@prenom, telephone=@telephone where id=@id";
            command = new SqlCommand(request, Tools.Connection);
            command.Parameters.Add(new SqlParameter("@nom", Nom));
            command.Parameters.Add(new SqlParameter("@prenom", Prenom));
            command.Parameters.Add(new SqlParameter("@telephone", Telephone));
            command.Parameters.Add(new SqlParameter("@id", Id));
            Tools.Connection.Open();
            int nbRow = command.ExecuteNonQuery();
            command.Dispose();
            Tools.Connection.Close();
            return nbRow == 1;
        }

        //Recuperer un client en fonction de son id
        public static Client GetClientById(int id)
        {
            Client client = null;
            string request = "SELECT id, nom, prenom, telephone from client where id=@id";
            command = new SqlCommand(request, Tools.Connection);
            command.Parameters.Add(new SqlParameter("@id", id));
            Tools.Connection.Open();
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                client = new Client(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
            }
            reader.Close();
            command.Dispose();
            Tools.Connection.Close();
            return client;
        }

        public override string ToString()
        {
            return $"Nom : {Nom}, Prénom : {Prenom}, Téléphone : {Telephone}";
        }
    }
}
