using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XZWeather
{
    public class WeatherInfo
    {
        public List<resultsList> results { get; set; }
    }
    //public class Result
    //{
    //    public List<resultsList> zero { get; set; }
    //}
    public class resultsList
    {
        
        public locationList location { get; set; }
        public nowList now { get; set; }
        public string last_update { get; set; }
    }
    public struct locationList
    {
        public string id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public string path { get; set; }
        public string timezone { get; set; }
        public string timezone_offset { get; set; }
    }
    public class nowList
    {
        public string text { get; set; }
        public string code { get; set; }
        public string temperature { get; set; }
    }
    
}
