using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Examples.Algorithms.Utility.Helpers;
using Examples.Algorithms.Utility.Models;

namespace Examples.Algorithms.Utility.Controllers
{ 
    [Route("api/qsort")]
    public class QSortController : Controller
    {

        [Route("getrandomarray")]
        [HttpGet]
        public IActionResult GetIntegerArray(int maxValue = 100, int arraySize = 10)
        {
            var list = new List<int>();
            Random rand = new Random();

            for (int i = 0; i < arraySize; i++)
            {
                list.Add(rand.Next(0, maxValue));
            }

            int[] arrayForReturn = list.ToArray();

            return Ok(arrayForReturn);
        }


        [Route("pivotarray")]
        [HttpPost]
        public IActionResult SortIntegerArray([FromBody] int[] arrayForSort)
        {
            
            QsortArraySet sortSetForReturn = QSort.SortIntegerArray(arrayForSort);

            return Ok(new {
                InitialArray = sortSetForReturn.InitialArray,
                EndingArray = sortSetForReturn.EndingArray,
                SortSets = sortSetForReturn.Pivots.OrderBy(p => p.Iteration).ToArray()
                });
        }
        
    }
}