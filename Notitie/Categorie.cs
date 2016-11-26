using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common_DLL;

namespace Notitie
{
    public class Categorie : ObservableObject
    {

        public int Id { get; set; }

        //public bool Created { get; set; }//Verschil maken tussen het aanmaken van een object en het veranderen van een object

        public ObservableCollection<Notitie> NotitieLijst = new ObservableCollection<Notitie>();

        private string _titel;

        public string Titel {
            get { return _titel; }
            set
            {
                if (Titel != value)
                {
                    onPropertyChanged(Id, value, "categorietitel");
                }
            }
        }

        public string WijzigingsDatum { get; set; }


        public Categorie(int id, string titel)
        {
            _titel = titel;
            Id = id;
        }

        public Categorie(string titel)
        {
            _titel = titel;
        }

    }
}
