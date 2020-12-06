using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace TP_Hotel.Classes
{
    class Tools
    {
        public static SqlConnection Connection = new SqlConnection(@"Data Source=(LocalDb)\coursMusM2I;Integrated Security=True");

        public static bool CheckName(string name)
        {
            string pattern = @"[a-zA-Z\s-]+";
            return Regex.IsMatch(name, pattern);
        }

        public static bool CheckPhone(string phone)
        {
            string pattern = @"^0[1-9]{1}([\s\.-]?\d{2}){4}$";
            return Regex.IsMatch(phone, pattern);
        }

        public static void TryParsePropertyClient(string message, Client client, string nameProperty)
        {
            bool error = false;
            do
            {
                Console.Write(message);
                try
                {
                    //Pour modifier une propriété d'un objet d'une manière dynamique en fonction du nom de la propriété
                    client.GetType().GetProperty(nameProperty).SetValue(client, Console.ReadLine());
                    error = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    error = true;
                }
            } while (error);

        }

        public static void TryParseProperty<T>(string message, T objet, string propertyName)
        {
            bool error = false;
            do
            {
                Console.Write(message);
                try
                {
                    //Pour modifier une propriété d'un objet d'une manière dynamique en fonction du nom de la propriété
                    try
                    {
                        objet.GetType().GetProperty(propertyName).SetValue(objet, Console.ReadLine());
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.InnerException;
                    }
                    error = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    error = true;
                }
            } while (error);
        }
    }
}
