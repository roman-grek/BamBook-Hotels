using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using ProjectHotels.Models;
using Xamarin.Forms;

namespace ProjectHotels
{
    public class HotelItemDatabase
    {
        readonly SQLiteConnection database;
        public HotelItemDatabase (string dbPath)
        {
            database = new SQLiteConnection(dbPath);
            database.CreateTable<Hotel>();
        }

        public Hotel GetHotel (int id)
        {
            return database.Table<Hotel>().Where(i => i.ID == id).FirstOrDefault();
        }

        public List<Hotel> GetHotels()
        {
            return database.Table<Hotel>().ToList();
        }

        public int SaveHotel(Hotel hotel)
        {
            if (hotel.ID != 0)
            {
                return database.Update(hotel);
            }
            else
            {
                return database.Insert(hotel);
            }
        } 

        public int DeleteHotel(Hotel hotel)
        {
            return database.Delete(hotel);
        }
    }
}
