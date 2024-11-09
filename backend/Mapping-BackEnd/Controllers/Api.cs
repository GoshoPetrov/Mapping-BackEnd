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
    }
}
