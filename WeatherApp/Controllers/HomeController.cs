using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
           

            return View();
        }


        //this method will trigger when sumit button is clicked
        [HttpPost]
        public ActionResult Index(Weather w)
        {
            //trying to make a succesfull API call
            try
            {
                //storing a url with a name of a city and APPID to make a call
                string strUrl = String.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&APPID=7439a1642de23365c6cc4770e9a50464", w.name);
                //making a request
                WebRequest request = WebRequest.Create(strUrl);
                //assigning method for a request
                request.Method = "GET";

                //Getting a responce from our request
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string result;
                //opening stream
                using (Stream s = response.GetResponseStream())
                {
                    //creating new instance of a streamReader with our stream
                    StreamReader sr = new StreamReader(s);
                    //reading result and assigning it to a variable
                    result = sr.ReadToEnd();

                    //closing a stream
                    sr.Close();

                }

                //converting Json result to a Weather object
                Weather weather = JsonConvert.DeserializeObject<Weather>(result);

                //passing id to view
                ViewBag.id = weather.id;

                return View(weather);
            }
            catch(Exception ex)
            {
                //handilng error with apropriate messages
                if (ex.Message.Contains("404"))
                {
                    ViewBag.Res = "Sorry, we cannot find city with given name...\n Please try again";
                }
                else
                {
                    ViewBag.Res = "Sorry, something went wrong...\n Please try again";
                }
                return View();
            }

        }
    }
}