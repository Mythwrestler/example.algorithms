using Microsoft.AspNetCore.Mvc;
using Examples.Algorithms.Utility.Helpers;
using Examples.Algorithms.Utility.Models;

namespace Examples.Algorithms.Utility.Controllers
{
    [Route("api/editdistance")]
    public class EditDistanceController : Controller
    {

        [Route("simple")]
        [HttpPost]
        public IActionResult CalcIntegerArray([FromBody]EditDistanceCalcBody request)
        {

            if (string.IsNullOrEmpty(request.String01) || string.IsNullOrEmpty(request.String02)) return BadRequest();

            EditDistanceSet[] returnSet = new EditDistanceSet[]{
                EditDistance.OptimalDistanceFromString01ToString02(request.String01, request.String02, request.CaseInsensitive),
                EditDistance.TrueDamerauLevenshteinDistance(request.String01, request.String02, request.CaseInsensitive)
            };

            return Ok(returnSet);
        }
    }
}