using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Common_DLL
{
    public class ObservableObject : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private SqlConnection connection;


        protected virtual void OnCollectionChanged()
        {

        }


        protected void onPropertyChanged(int sender, PropertyChangedEventArgs e, string type)
        {
            connection =
                      new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=WpfData;Integrated Security=True");

            switch (type)
            {
                case "notitietekst":


                    Console.Write(sender.ToString(sender.ToString()));

                    try
                    {
                        connection.Open();
                        var command =
                            new SqlCommand("update notitie set tekst = '" + e.PropertyName + "' where id = " + sender,
                                connection);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception w)
                    {
                        Console.WriteLine(w.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    break;




                case "notitietitel":
                    Console.Write(sender.ToString(sender.ToString()));

                    try
                    {
                        connection.Open();
                        var command =
                            new SqlCommand("update notitie set titel = '" + e.PropertyName + "' where id = " + sender,
                                connection);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception w)
                    {
                        Console.WriteLine(w.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    break;

                case "categorietitel":

                    try
                    {
                        connection.Open();
                        var command =
                            new SqlCommand("update categorie set titel = '" + e.PropertyName + "' where id = " + sender,
                                connection);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception w)
                    {
                        Console.WriteLine(w.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    break;

            }
            
        }

        protected void onPropertyChanged(int e, string s, string type)
        {
            Console.WriteLine(e.ToString());
            
            
            onPropertyChanged(e, new PropertyChangedEventArgs(s), type);
        }



        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { throw new NotImplementedException(); }

            remove { throw new NotImplementedException(); }
        }
    }

}




