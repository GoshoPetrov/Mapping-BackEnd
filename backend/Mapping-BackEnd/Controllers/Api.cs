using Mapping_BackEnd.Data;
using Microsoft.AspNetCore.Mvc;

namespace Mapping_BackEnd.Controllers
{
    public class Model
    {
        public double Lat { get; set; }
        public double Longtitude { get; set; }
        public string Caption { get; set; }
        public int Radius { get; set; }
    }
    public class Api : Controller
    {
        [HttpPost]
        public IActionResult Map()
        {
            var db = new PensaClubContext();

            var list = new List<Dictionary<string, object>>();

            foreach (var item in db.Places)
            {
                var dict = new Dictionary<string, object>();

                dict["coordinates"] = new double?[] { item.Lat, item.Long };
                dict["radius"] = item.Radius;
                dict["message"] = item.Caption;

                list.Add(dict);
            }

            return Json(list);
        }

        public IActionResult Map2()
        {
            var dict = new Dictionary<string, object>();

            dict["coordinates"] = new double[] { 42.6361668, 23.3698051 };
            dict["radius"] = 18;
            dict["message"] = "Softuni";

            return Json(dict);
        }

        public IActionResult Save([FromBody] Model model)
        {
            var db = new PensaClubContext();
            var n = new Place();

            n.Lat = model.Lat;
            n.Long = model.Longtitude;
            n.Caption = model.Caption;
            n.Radius = 13;

            db.Places.Add(n);
            db.SaveChanges();

            // You can now use model.Var1 and model.Var2
            return Ok(new { model.Lat, model.Longtitude });
        }

    }
}
