using Icelane_TechTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Icelane_TechTest.Models;
using System.Text.RegularExpressions;

namespace Icelane_TechTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;


        public IActionResult Index()
        {
            ViewBag.Items = new List<string>();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public ActionResult Index(FoodItem foodItemString)
        {
            var foodListString = foodItemString.TextAreaString;

            string[] sep = new string[] { "\r\n" };
            string[] items = foodListString.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            List<FoodItem> foodList = new List<FoodItem>();

            FoodItem foodItem = new FoodItem();

            formatItems(items, foodList);
            validateItems(foodList);
            updateItems(foodList);
            sendUpdatedItems(foodList);

            return View(foodItem.FoodItems);
        }
        public void sendUpdatedItems(List<FoodItem> foodList)
        {

            try
            {
                ViewBag.item = foodList[0].Name + " " + foodList[0].SellinValue + " " + foodList[0].Quality;
                ViewBag.item1 = foodList[1].Name + " " + foodList[1].SellinValue + " " + foodList[1].Quality;
                ViewBag.item2 = foodList[2].Name + " " + foodList[2].SellinValue + " " + foodList[2].Quality;
                ViewBag.item3 = foodList[3].Name + " " + foodList[3].SellinValue + " " + foodList[3].Quality;
                ViewBag.item4 = foodList[4].Name + " " + foodList[4].SellinValue + " " + foodList[4].Quality;
                ViewBag.item5 = foodList[5].Name + " " + foodList[5].SellinValue + " " + foodList[5].Quality;
                ViewBag.item6 = foodList[6].Name + " " + foodList[6].SellinValue + " " + foodList[6].Quality;
                ViewBag.item7 = foodList[7].Name + " " + foodList[7].SellinValue + " " + foodList[7].Quality;
                ViewBag.item8 = foodList[8].Name + " " + foodList[8].SellinValue + " " + foodList[8].Quality;
            }
            catch
            {
                ViewBag.item = "Error with results";
            }

        }

        public void updateItems(List<FoodItem> foodList)
        {
            for (int i = 0; i < foodList.Count; i++)
            {
                if (foodList[i].Name == "Aged Brie")
                {
                    if(foodList[i].Quality <= 50)
                    {
                        foodList[i].Quality++;
                    }                    
                    foodList[i].SellinValue--;                   
                }

                if (foodList[i].Name == "Christmas Crackers")
                {
                    if(foodList[i].Quality <= 50){
                        if (foodList[i].SellinValue <= 5)
                        {
                            foodList[i].Quality = foodList[i].Quality + 3;
                        }
                        if (foodList[i].SellinValue <= 10 && foodList[i].SellinValue > 5)
                        {
                            foodList[i].Quality = foodList[i].Quality + 2;
                        }
                    }

                    foodList[i].SellinValue--;
                }

                if (foodList[i].Name == "Frozen Item")
                {
                    if (foodList[i].SellinValue < 0)
                    {
                        foodList[i].Quality = foodList[i].Quality - 2;
                    }
                    else
                    {
                        foodList[i].Quality--;
                    }
                    foodList[i].SellinValue--;
                }

                if (foodList[i].Name == "Soap")
                {
                    //Nothing to be done
                }

                if (foodList[i].Name == "Fresh Item")
                {
                    if(foodList[i].SellinValue < 0)
                    {
                        foodList[i].Quality = foodList[i].Quality - 4;
                    }
                    else
                    {
                        foodList[i].Quality = foodList[i].Quality - 2; ;
                    }
                    foodList[i].SellinValue--;
                }

                if (foodList[i].Quality < 0)
                {
                    foodList[i].Name = "Invalid Item - Quality Too Low";
                    foodList[i].Quality = null;
                    foodList[i].SellinValue = null;
                }
                if (foodList[i].Name == "NO SUCH ITEM")
                {
                    foodList[i].Quality = null;
                    foodList[i].SellinValue = null;
                }
            }
        }

        public void formatItems(string[] items, List<FoodItem> foodList)
        {
            for(int i = 0; i < items.Length; i++)
            {
                //Quality control
                var foodItem = new FoodItem();
                
                string temp = items[i];
                const string pattern = @"\s*(?<foodName>([^\d+]+))\s*(?<isNegative>([^\w]))\s*(?<SellinValue>\d+)\s(?<Quality>\d+)";
                Regex r = new Regex(pattern, RegexOptions.Compiled);

                Match m = r.Match(temp);

                try {
                    string foodName = m.Groups["foodName"].Value;
                    int quality = int.Parse(m.Groups["Quality"].Value);
                    int sellinValue = int.Parse(m.Groups["SellinValue"].Value);
                    string isNegative = m.Groups["isNegative"].Value;

                    if (isNegative == "-")
                    {
                        sellinValue = sellinValue * -1;
                    }

                    foodItem.Name = foodName.TrimEnd();
                    foodItem.SellinValue = sellinValue;
                    foodItem.Quality = quality;
                    
                } 
                catch
                {
                    string foodName = "Invalid";
                    int? quality = null;
                    int? sellinValue = null;
                    string isNegative = null;
                }

                foodList.Add(foodItem);

            }


            return;
        }

        public void validateItems(List<FoodItem> foodList)
        {
            //Current items in stock - would be in a database 
            string[] avalibleItems = new string[] { "Aged Brie", "Christmas Crackers", "Frozen Item", "Soap" , "Fresh Item"};
            

            for(int i = 0; i < foodList.Count; i++)
            {
                bool itemMatch = false;

                for(int j = 0; j < avalibleItems.Length; j++)
                {
                    string temp = avalibleItems[j] + " ";
                    if(foodList[i].Name == temp || foodList[i].Name == avalibleItems[j])
                    {
                        itemMatch = true;
                        break;
                    }                                                                      
                }
                if(itemMatch == false)
                {                
                    foodList[i].Name = "NO SUCH ITEM";
                }
            }
        }

    }
}

