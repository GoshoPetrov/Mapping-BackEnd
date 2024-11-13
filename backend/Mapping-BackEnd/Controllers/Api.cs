using Microsoft.AspNetCore.Mvc;

namespace Mapping_BackEnd.Controllers
{
    public class Api : Controller
    {
        [HttpPost]
        public IActionResult Map()
        {
            var dict = new Dictionary<string, object>();

            dict["coordinates"] = new double[] { 42.6977, 23.3219 };
            dict["radius"] = 13;
            dict["message"] = "Sofia";

            return Json(dict);
        }

        public IActionResult Map2()
        {
            var dict = new Dictionary<string, object>();

            dict["coordinates"] = new double[] { 42.6361668, 23.3698051 };
            dict["radius"] = 18;
            dict["message"] = "Softuni";

            return Json(dict);
        }

    }
}
