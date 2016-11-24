namespace Notitie
{
    public class Notitie
    {

        #region properties

        public int NotNummer { get; set; }

        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }


        private string _titel;

        public string Titel
        {
            get { return _titel; }
            set { _titel = value; }
        }

        #endregion

        #region Constructors

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
