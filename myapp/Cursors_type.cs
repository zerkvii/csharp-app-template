using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myapp
{
   public class Cursors_type
    {
        public Boolean Hole { get; set; }

        public Boolean Bomb_s { get; set; }
        public Boolean Bomb_a { get; set; }
        public Boolean Draft { get; set; }
        public Boolean Eraser { get; set; }
        public Boolean Gp { get; set; }
        public Boolean Gx { get; set; }
        public Boolean Gm { get; set; }
        public Boolean Move { get; set; }

        public Cursors_type()
        {
            Hole = false;
            Bomb_s = false;
            Bomb_a = false;
            Draft = false;
            Eraser = false;
            Gp = false;
            Gx = false;
            Gm = false;
            Move = false;
        }
        public void set_attr_val(String attr)
        {
            foreach(System.Reflection.PropertyInfo p in this.GetType().GetProperties())
            {
                if (p.Name != attr) p.SetValue(this, false);
                else p.SetValue(this,true);
            }
        }
    }
}
