using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_V.Controllers
{
    public class MapanController : Controller
    {
        // GET: Mapan
        public ActionResult GetFile1()
        {
            return View();
        }

        public static string fileName;
        public static string fileName2;
        public static Dictionary<string, double> shirmin = new Dictionary<string, double>();
        public static Dictionary<string, double> shirmax = new Dictionary<string, double>();
        public static Dictionary<string, double> dolgmin = new Dictionary<string, double>();
        public static Dictionary<string, double> dolgmax = new Dictionary<string, double>();
        [HttpPost]
        public ActionResult GetFile1(HttpPostedFileBase upload, HttpPostedFileBase upload2)
        {
            if (upload != null && upload2 != null)
            {
                // получаем имя файла
                fileName = System.IO.Path.GetFileName(upload.FileName);
                fileName2 = System.IO.Path.GetFileName(upload2.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Files/" + fileName));
                upload2.SaveAs(Server.MapPath("~/Files/" + fileName2));
            }


            return RedirectToAction("Coordinate", "Mapan");
        }


        public ActionResult Coordinate()
        {
            string path = Server.MapPath("~/Files/" + "1031+1.txt");//fileName); //+ "Vse n5 08-1 (1955-1985).txt");//

            List<List<Double>> Clusters = new List<List<Double>>();
            List<Double> Cluster = new List<double>();

            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {

                    string[] arg = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string z in arg)
                    {
                            Cluster.Add(Convert.ToDouble(z, CultureInfo.InvariantCulture));    
                    }
                    //Console.WriteLine(line);

                    Clusters.Add(new List<double>(Cluster));
                    Cluster.Clear();
                }
            }

            //Dictionary<string, double> shirmin = new Dictionary<string, double>();
            //Dictionary<string, double> shirmax = new Dictionary<string, double>();
            //Dictionary<string, double> dolgmin = new Dictionary<string, double>();
            //Dictionary<string, double> dolgmax = new Dictionary<string, double>();
            MinMax(Clusters, 1, 0, dolgmin, dolgmax);
            MinMax(Clusters, 1, 1, shirmin, shirmax);

            dolgmin.Add("23", 1);
            dolgmax.Add("23", 20);
            shirmin.Add("23", 1);
            shirmax.Add("23", 10);

            ViewBag.i = dolgmax.Count;
            ViewBag.y = 23;
            ViewBag.shirmin = shirmin;
            ViewBag.shirmax = shirmax;
            ViewBag.dolgmin = dolgmin;
            ViewBag.dolgmax = dolgmax;

            return View();
        }

        [HttpPost]
        public ActionResult Coordinate(int y)
        {
            ViewBag.shirmin = shirmin;
            ViewBag.shirmax = shirmax;
            ViewBag.dolgmin = dolgmin;
            ViewBag.dolgmax = dolgmax;
            ViewBag.y = y;
            ViewBag.i = dolgmax.Count;
            return View();
        }

        public static void MinMax(List<List<double>> file, int k, int mas, Dictionary<string,double> testmin, Dictionary<string, double> testmax)
        {
            List<double> one = new List<double>();
            foreach(List<double> i in file)
            {
                if(i[2] == k)
                one.Add(i[mas]);
            }

            if(one.Count == 0)
            {
                return;
            }

            testmin.Add(k.ToString(), one.Min());
            testmax.Add(k.ToString(), one.Max());

            MinMax(file, k + 1, mas, testmin, testmax);
        }

    }
}