using System.Windows;
using Notities;
using Common_DLL;
using System.Windows.Controls;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System;
using System.Collections.Specialized;

namespace Notities
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Notitie.Categorie> LijstCategorie = new ObservableCollection<Notitie.Categorie>();
        public List<Notitie.Categorie> ListCategorie = new List<Notitie.Categorie>();

        public MainWindow()
        {
            InitializeComponent();

            LijstCategorie.CollectionChanged += LijstCategorie_CollectionChanged;

        }

        private void LijstCategorie_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    
                    foreach (Notitie.Categorie cat in e.NewItems) {
                        TreeViewItem CategorieItem = new TreeViewItem();
                        CategorieItem.Header = cat.Titel;
                        Notitie.Categorie Categorie = new Notitie.Categorie(CategorieItem.Header.ToString());
                        CategorieItem.Tag = Categorie;
                        CategorieLijst.Items.Add(CategorieItem);
                    }
                    
                    break;
                case NotifyCollectionChangedAction.Remove:

                    foreach (Notitie.Categorie cat in e.NewItems)
                    {
                        CategorieLijst.Items.Remove(CategorieLijst.SelectedItem);
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
            }
        }

        private void btnNewCategory_Click(object sender, RoutedEventArgs e)
        {
            Notitie.Categorie categorie = new Notitie.Categorie("Test");
            LijstCategorie.Add(categorie);
        }

        private void btnDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedcategory = (TreeViewItem)CategorieLijst.SelectedItem;
            Notitie.Categorie categorie = (Notitie.Categorie)selectedcategory.Tag;
            LijstCategorie.Remove(categorie);
        }


        private void btnNewNote_Click(object sender, RoutedEventArgs e)
        {

            TreeViewItem noteItem = new TreeViewItem();
            noteItem.Header = "test";
            Notitie.Notitie notitie = new Notitie.Notitie(noteItem.Header.ToString(), "test");
            noteItem.Tag = notitie;

            TreeViewItem selectedcategory = (TreeViewItem)CategorieLijst.SelectedItem;
            try
            {
                Notitie.Categorie categorie = (Notitie.Categorie)selectedcategory.Tag;
                categorie.NotitieLijst.Add(notitie);

                NotesLijst.Items.Add(noteItem);
            }
            catch (System.Exception)
            {
                MessageBox.Show("Gelieve eerst de folder aan te duiden");
            }
           
            
        }

        private void CategorieLijst_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            NotesLijst.Items.Clear();

            TreeViewItem selectedcategory = (TreeViewItem)CategorieLijst.SelectedItem;

            Notitie.Categorie categorie = (Notitie.Categorie)selectedcategory.Tag;
            if (categorie.NotitieLijst != null)
            {
                foreach (Notitie.Notitie notitie in categorie.NotitieLijst)
                {
                    NotesLijst.Items.Add(notitie.Titel);
                }
            }

        }

        private void NotesLijst_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            NotitieTextBox.IsEnabled = true;
            TreeViewItem selectednote = (TreeViewItem)NotesLijst.SelectedItem;
            Notitie.Notitie notitie = (Notitie.Notitie)selectednote.Tag;
            if (notitie.Text != null)
            {
                NotitieTextBox.Text = notitie.Text;
            }
          
            
        }

        private void btnDeleteNote_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NotitieTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TreeViewItem selectednote = (TreeViewItem)NotesLijst.SelectedItem;
            Notitie.Notitie notitie = (Notitie.Notitie)selectednote.Tag;

            notitie.Text = NotitieTextBox.Text;
        }


    }
}
