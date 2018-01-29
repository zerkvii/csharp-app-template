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
