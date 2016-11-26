using System;
using System.CodeDom;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using Notitie;

//using Notitie = Notitie.Notitie;


namespace Notities
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<Categorie> _lijstCategorie =
            new ObservableCollection<Categorie>();

        private SqlConnection connection;
        private SqlDataAdapter adapter;
        private SqlCommandBuilder builder;
        private DataSet dataset;

        private bool Startup = true;                

        public MainWindow()
        {
            InitializeComponent();

            _lijstCategorie.CollectionChanged += LijstCategorie_CollectionChanged;

            connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=WpfData;Integrated Security=True");

            connection.Open();
            var adapter = new SqlDataAdapter("Select * from categorie", connection);
            DataSet dataset = new DataSet();
            adapter.Fill(dataset, "categorie");
            connection.Close();

            foreach (DataRow rij in dataset.Tables["categorie"].Rows)
            {
                _lijstCategorie.Add(new Categorie(Convert.ToInt32(rij[0]), rij[1].ToString()));

            }

            Startup = false;

        }

        #region Collectionchanged LijstCategorie

        private void LijstCategorie_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    foreach (Categorie cat in e.NewItems)
                    {
                        if (!Startup)
                        {
                            try
                            {
                                lblStatus.Content = "Categorie opslaan";
                                connection.Open();
                                var command =
                                    new SqlCommand(
                                        "insert into categorie (titel) values ('" + cat.Titel +
                                        "'); select SCOPE_IDENTITY();",
                                        connection);

                                var result = (decimal) command.ExecuteScalar();
                                
                                cat.Id = Convert.ToInt32(result);

                                var categorieItem = new TreeViewItem
                                {
                                    Header = cat.Titel,
                                    Tag = cat
                                };

                                lblStatus.Content = "Categorie opgeslagen";

                                CategorieLijst.Items.Add(categorieItem);
                               
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                connection.Close();
                                cat.NotitieLijst.CollectionChanged += categorie_NotitieLijst_CollectionChanged;
                            }
                        }
                        else
                        {
                            var categorieItem = new TreeViewItem
                            {
                                Header = cat.Titel,
                                Tag = cat
                            };

                            CategorieLijst.Items.Add(categorieItem);
                            cat.NotitieLijst.CollectionChanged += categorie_NotitieLijst_CollectionChanged;
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:

                    foreach (Categorie cat in e.OldItems)
                    {

                        try
                        {
                            lblStatus.Content = "Categorie verwijderen";
                            connection.Open();
                            var commandnote = new SqlCommand("delete from notitie where cat_id = " + cat.Id, connection);
                            commandnote.ExecuteNonQuery();
                            var command = new SqlCommand("delete from categorie where id = " + cat.Id, connection);
                            command.ExecuteNonQuery();
                            connection.Close();

                            //UI stuff
                            CategorieLijst.Items.Remove(CategorieLijst.SelectedItem);
                            notetext.Clear();
                            NotitieTextBox.Clear();
                            NotitieTextBox.IsEnabled = false;
                            lblStatus.Content = "Categorie verwijdert";
                        }
                        catch (Exception ex)
                        {
                            connection.Close();
                            MessageBox.Show(ex.Message);
                        }


                    }

                    break;
            }
        }

        #endregion

        #region Collectionchanged LijstNotitie

        private void categorie_NotitieLijst_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Notitie.Notitie noteItem in e.NewItems)
                    {
                        
                        if (Startup) continue;
                        try
                        {
                            lblStatus.Content = "Notitie opslaan...";
                            connection.Open();
                            TreeViewItem selectedcategory = (TreeViewItem) CategorieLijst.SelectedItem;
                            var categorie = (Categorie) selectedcategory.Tag;
                            var command =
                                new SqlCommand(
                                    "insert into notitie (titel, tekst, cat_id) values ('" + noteItem.Titel + "', '" +
                                    noteItem.Text + "', " + categorie.Id + "); select SCOPE_IDENTITY();",
                                    connection);
                            var result = (decimal)command.ExecuteScalar();
                            noteItem.id = Convert.ToInt32(result);
                            var treeItem = new TreeViewItem
                            {
                                Header = noteItem.Titel,
                                Tag = noteItem
                            };
                            NotesLijst.Items.Add(treeItem);
                            lblStatus.Content = "Notitie opgeslagen";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);

                        }
                        finally
                        {
                            connection.Close();
                        }


                    }


                    break;
                case NotifyCollectionChangedAction.Remove:

                    foreach (Notitie.Notitie note in e.OldItems)
                    {
                        NotesLijst.Items.Remove(note);

                        try
                        {
                            lblStatus.Content = "Notitie verwijderen...";
                            connection.Open();
                            var command = new SqlCommand("delete from notitie where id = " + note.id, connection);
                            command.ExecuteNonQuery();
                            NotesLijst.Items.Remove(NotesLijst.SelectedItem);

                            //UI stuff
                            NotitieTextBox.Clear();
                            NotitieTextBox.IsEnabled = false;
                            notetext.Clear();
                            lblStatus.Content = "Notitie verwijdert";
                        }
                        catch (Exception w)
                        {

                            MessageBox.Show(w.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }


                    }

                    break;
            }
        }


        #endregion

        #region Collection Triggers

        private void btnNewCategory_Click(object sender, RoutedEventArgs e)
        {
            var categorie = new Categorie("Categorie");
            _lijstCategorie.Add(categorie);
        }

        private void btnDeleteCategory_Click(object sender, RoutedEventArgs e)
        {

            var selectedcategory = (TreeViewItem) CategorieLijst.SelectedItem;
            if (selectedcategory == null) return;
            Categorie categorie = (Categorie) selectedcategory.Tag;
            
            var indexof = _lijstCategorie.IndexOf(categorie);
            
            _lijstCategorie.RemoveAt(indexof);
        }


        private void btnNewNote_Click(object sender, RoutedEventArgs e)
        {

            var selectedcategorie = (TreeViewItem) CategorieLijst.SelectedItem;
            if (selectedcategorie == null) return;
            var cat = (Categorie) selectedcategorie.Tag;
            var notitie = new Notitie.Notitie("Notite", "Tekst");
            cat.NotitieLijst.Add(notitie);

        }

        private void btnDeleteNote_Click(object sender, RoutedEventArgs e)
        {
            var selectedcategorie = (TreeViewItem) CategorieLijst.SelectedItem;
            if (selectedcategorie == null) return;
            var cat = (Categorie) selectedcategorie.Tag;

            var selectednote = (TreeViewItem) NotesLijst.SelectedItem;
            if (selectednote == null)
                return;
            var note = (Notitie.Notitie) selectednote.Tag;

            var indexof = cat.NotitieLijst.IndexOf(note);
            cat.NotitieLijst.RemoveAt(indexof);
        }


        #endregion

        #region Basic Triggers

        private void CategorieLijst_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            NotesLijst.Items.Clear();

            var selectedcategory = (TreeViewItem) CategorieLijst.SelectedItem;


            if (selectedcategory == null)
                return;
            var categorie = (Categorie) selectedcategory.Tag;

            categorytext.Text = (string) selectedcategory.Header;

            connection.Open();
            var adapter = new SqlDataAdapter("Select * from notitie where cat_id = " + categorie.Id, connection);
            DataSet dataset = new DataSet();
            adapter.Fill(dataset, "notitie");
            connection.Close();

            Startup = true;

            foreach (DataRow rij in dataset.Tables["notitie"].Rows)
            {
                Notitie.Notitie notition = new Notitie.Notitie(Convert.ToInt32(rij[0]), rij[1].ToString(),
                    rij[2].ToString(), Convert.ToInt32(rij[3]));

                //_lijstCategorie.Add(new Categorie(Convert.ToInt32(rij[0]), rij[1].ToString()));

                categorie.NotitieLijst.Add(notition);
                var test = new TreeViewItem {Tag = notition, Header = notition.Titel};
                NotesLijst.Items.Add(test);
            }

            Startup = false;


        }

        private void NotesLijst_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {



            NotitieTextBox.IsEnabled = true;

            TreeViewItem selectednote = (TreeViewItem) NotesLijst.SelectedItem;
            notetext.Text = (string) selectednote.Header;
            Notitie.Notitie notitie = (Notitie.Notitie) selectednote.Tag;

            try
            {
                connection.Open();
                var command = new SqlCommand("select tekst from notitie where id = " + notitie.id, connection);

                string value = (string) command.ExecuteScalar();

                //lijstCategorie.Add(new Categorie(Convert.ToInt32(rij[0]), rij[1].ToString()));
                if (value != "")
                {
                    NotitieTextBox.Text = value;
                }

            }
            catch (Exception w)
            {
                MessageBox.Show(w.Message);
            }
            finally
            {
                connection.Close();
            }

        }



        private void NotitieTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var selectednote = (TreeViewItem)NotesLijst.SelectedItem;
                var notitie = (Notitie.Notitie)selectednote.Tag;

                notitie.Text = NotitieTextBox.Text;
            }
            catch (Exception w)
            {
                MessageBox.Show(w.Message);
            }
            
        }

        private void changecategorytext_Click(object sender, RoutedEventArgs e)
        {
            
            TreeViewItem selectedtree = (TreeViewItem)CategorieLijst.SelectedItem;
            if (selectedtree == null) return;
            selectedtree.Header = categorytext.Text;
            Categorie cat = (Categorie)selectedtree.Tag;
            var indexOf = _lijstCategorie.IndexOf(cat);
            _lijstCategorie[indexOf].Titel = categorytext.Text;
        }

        private void changenotetext_click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedtree = (TreeViewItem)CategorieLijst.SelectedItem;
            if (selectedtree == null) return;
            Categorie cat = (Categorie)selectedtree.Tag;
            TreeViewItem selectednote = (TreeViewItem) NotesLijst.SelectedItem;
            selectednote.Header = notetext.Text;
            Notitie.Notitie note = (Notitie.Notitie) selectednote.Tag;
            var indexof = cat.NotitieLijst.IndexOf(note);
            cat.NotitieLijst[indexof].Titel = notetext.Text;
        }
    }

    #endregion


}
