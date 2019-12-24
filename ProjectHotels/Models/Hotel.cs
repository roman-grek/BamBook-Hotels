using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SQLite;

namespace ProjectHotels.Models
{
    public class Hotel
    {
        [AutoIncrement, PrimaryKey]
        public int ID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int Stars { get; set; }
        public double Rating { get; set; }
        public string Address { get; set; }

        //it's not OK
        public string Picture1 { get; set; }
        public string Picture2 { get; set; }
        public string Picture3 { get; set; }
        public string Picture4 { get; set; }
    }
}
