using Apartaments.Infrastructure;

namespace Apartaments.Models.PageModels
{
    public class IndexModel
    {
        private const string apartamentsQuery = @"
        SELECT
        _id
        ,count_rooms
        ,url_str
        
        ,(SELECT price FROM history_apartment_prices WHERE id_apartment = _id ORDER BY date_update_price DESC LIMIT 1)
        AS actual_price
        
        FROM Apartaments";

        public LinkedList<Apartament> Apartaments { get; set; } = new();

        public IndexModel() : this(byRooms: 0) { }

        public IndexModel(int byRooms)
        {
            using (var dr = SqlLiteManager.GetReader(string.Concat(apartamentsQuery, byRooms > 0 ? $"\nWHERE count_rooms = {byRooms}" : "")))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Apartaments.AddLast(new Apartament 
                        {
                            Id = (Int64)dr["_id"],
                            CountRooms = (Int64)dr["count_rooms"],
                            UrlStr = (string)dr["url_str"],
                            ActualPrice = (Int64)dr["actual_price"]
                        });
                    }
                }
            }
        }

        public static List<DataChart> GetDataChart(int id)
        {
            var data = new List<DataChart>();

            using (var dr = SqlLiteManager.GetReader($"SELECT date_update_price, price FROM history_apartment_prices WHERE id_apartment = {id} ORDER BY date_update_price"))
            {
                if (dr.HasRows)
                    while (dr.Read())
                    {
                        data.Add(new DataChart
                        {
                            Month = DateTime.Parse((string)dr["date_update_price"]).ToString("yyyy.MM"),
                            Price = (Int64)dr["price"]
                        });
                    }
            }

            return data;
        }
    }
}
