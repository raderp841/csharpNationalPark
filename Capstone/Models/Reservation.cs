using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Reservation
    {
        public int Reservation_id { get; set; }
        public int Site_id { get; set; }
        public string Name { get; set; }
        public int MyProperty { get; set; }
        public DateTime From_date { get; set; }
        public DateTime To_date { get; set; }
        public DateTime Create_date { get; set; }
    }
}
