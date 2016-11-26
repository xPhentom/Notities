using Common_DLL;

namespace Notitie
{
    public class Notitie : ObservableObject
    {

        #region properties

        public int id { get; set; }
        public int CatNummer { get; set; }

        private string _text;

        public string Text
        {
            get { return _text; }
            set
            {
                if (Text != value)
                {
                    onPropertyChanged(id, value, "notitietekst");
                }
            }
        }


        private string _titel;

        public string Titel
        {
            get { return _titel; }
            set
            {
                if (Text != value)
                {
                    onPropertyChanged(id, value, "notitietitel");
                }
            }
        }


        #endregion

        #region Constructors

        public Notitie(int id,string titel, string text, int nummer)
        {
            this.id = id;
            _text = text;
            _titel = titel;
            CatNummer = nummer;
        }

        public Notitie(string text, string titel)
        {
            _text = text;
            _titel = titel;
        }

        #endregion

        #region Methods

        #endregion

    }
}
