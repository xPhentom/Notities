using System.Collections.Generic;

namespace Notitie
{
    public class Categorie
    {

        public List<Notitie> NotitieLijst = new List<Notitie>();

        private string _titel;

        public string Titel
        {
            get { return _titel; }
            set { _titel = value; }
        }

        private string _wijzigingsdatum;

        public string WijzigingsDatum
        {
            get { return _wijzigingsdatum; }
            set { _wijzigingsdatum = value; }
        }
       


        public Categorie(string titel)
        {
            Titel = titel;
            
        }

    }
}
