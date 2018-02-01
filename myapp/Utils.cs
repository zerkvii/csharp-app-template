using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp
{
    public class Utils : INotifyPropertyChanged
    {

        private String kind_type;
        public String Kind_type
        {
            get { return kind_type; }
            set
            {
                if (value != kind_type)
                {
                    kind_type = value;
                    Notify("Kind_type");
                }
            }
        }

        private String p_index;
        public String P_index
        {
            get { return p_index; }
            set
            {
                if (value != p_index)
                {
                    p_index = value;
                    Notify("P_index");
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void Notify(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
