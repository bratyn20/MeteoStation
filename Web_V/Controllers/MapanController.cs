using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        public static Dictionary<string, double> centredolg = new Dictionary<string, double>();
        public static Dictionary<string, double> centreshir = new Dictionary<string, double>();

        public static Dictionary<string, double> shirmin2 = new Dictionary<string, double>();
        public static Dictionary<string, double> shirmax2 = new Dictionary<string, double>();
        public static Dictionary<string, double> dolgmin2 = new Dictionary<string, double>();
        public static Dictionary<string, double> dolgmax2 = new Dictionary<string, double>();
        public static Dictionary<string, double> centredolg2 = new Dictionary<string, double>();
        public static Dictionary<string, double> centreshir2 = new Dictionary<string, double>();

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
            string path2 = Server.MapPath("~/Files/" + "1031+2.txt");

            List<List<Double>> Clusters = new List<List<Double>>();
            List<Double> Cluster = new List<double>();

            List<List<Double>> Clusters2 = new List<List<Double>>();
            List<Double> Cluster2 = new List<double>();

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


            using (StreamReader sr = new StreamReader(path2, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {

                    string[] arg = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string z in arg)
                    {
                        Cluster2.Add(Convert.ToDouble(z, CultureInfo.InvariantCulture));
                    }
                    //Console.WriteLine(line);

                    Clusters2.Add(new List<double>(Cluster2));
                    Cluster2.Clear();
                }
            }

            //Dictionary<string, double> shirmin = new Dictionary<string, double>();
            //Dictionary<string, double> shirmax = new Dictionary<string, double>();
            //Dictionary<string, double> dolgmin = new Dictionary<string, double>();
            //Dictionary<string, double> dolgmax = new Dictionary<string, double>();
            //Task task = Task.Factory.StartNew(() => 
            MinMax(Clusters, 1, 0, dolgmin, dolgmax);
            //Task task2 = Task.Factory.StartNew(() => 
            MinMax(Clusters, 1, 1, shirmin, shirmax);

            //Task task3 = task.ContinueWith((t) => 
            centredolg = Centre(dolgmin, dolgmax);
            //Task task4 = task2.ContinueWith((t) => 
            centreshir= Centre(shirmin, shirmax);

            //Task task5 = Task.Factory.StartNew(()=> 
            MinMax(Clusters2, 1, 0, dolgmin2, dolgmax2);
            //Task task6 = Task.Factory.StartNew(()=> 
            MinMax(Clusters2, 1, 1, shirmin2, shirmax2);

            //Task task7 = task5.ContinueWith((t) => 
            centredolg2 = Centre(dolgmin2, dolgmax2);
            //Task task8 = task6.ContinueWith((t) => 
            centreshir2 = Centre(shirmin2, shirmax2);

            Dictionary<string, Dictionary<string,double>> rsearch = Rsearch(centredolg,centredolg2,centreshir,centreshir2);
            Dictionary<string, Dictionary<string, double>> rsearchFinal = new Dictionary<string, Dictionary<string, double>>();
            //task4.Wait();task3.Wait();task7.Wait();task8.Wait();
          //  dolgmin.Add("23", 1);
          //  dolgmax.Add("23", 20);
          //  shirmin.Add("23", 1);
          //  shirmax.Add("23", 10);

            for (int i = 1; i <= rsearch.Count; i++)
            {
                Dictionary<string, double> rest = new Dictionary<string, double>();
                double test = (rsearch[i.ToString()]).Values.Min();
                string minkey = rsearch[i.ToString()].First(x => x.Value == (rsearch[i.ToString()]).Values.Min()).Key;
                rest.Add(minkey, test);
                rsearchFinal.Add(i.ToString(), new Dictionary<string, double>(rest));
                rest.Clear();
                //var q = rsearchFinal["1"].ElementAt(0).Key;
            }

            ViewBag.i = dolgmax.Count;
            ViewBag.y = 22;
            ViewBag.shirmin = shirmin;
            ViewBag.shirmax = shirmax;
            ViewBag.dolgmin = dolgmin;
            ViewBag.dolgmax = dolgmax;
            ViewBag.rsearch = rsearch;
            ViewBag.rsearchFinal = rsearchFinal;

            return View();
        }

        [HttpPost]
        public ActionResult Coordinate(int y)
        {
            if (y == 1)
            {
                ViewBag.shirmin = shirmin;
                ViewBag.shirmax = shirmax;
                ViewBag.dolgmin = dolgmin;
                ViewBag.dolgmax = dolgmax;
                ViewBag.y = dolgmax.Count;
               

            }

            if (y == 2)
            {
                ViewBag.shirmin = shirmin2;
                ViewBag.shirmax = shirmax2;
                ViewBag.dolgmin = dolgmin2;
                ViewBag.dolgmax = dolgmax2;
                ViewBag.y = dolgmax2.Count;
            }


            ViewBag.i = dolgmax.Count;
            return View();
        }

        [HttpPost]
        public ActionResult Supa(string f, string s)
        {
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

        public static Dictionary<string,double> Centre(Dictionary<string,double> min , Dictionary<string, double> max)
        {
            Dictionary<string, double> rez = new Dictionary<string, double>();
            if(min.Count == max.Count)
            for (int i = 1; i <= min.Count; i++)
            {
                    rez.Add(i.ToString(),(min[i.ToString()] + max[i.ToString()])/2); 
            }
            return rez;
        }

        public static Dictionary<string, Dictionary<string,double>> Rsearch(Dictionary<string,double> centredolg, Dictionary<string,double> centredolg2, Dictionary<string, double> centreshir, Dictionary<string, double> centreshir2)
        {
            Dictionary<string, Dictionary<string,double>> res = new Dictionary<string, Dictionary<string,double>>();
            Dictionary<string, double> preres = new Dictionary<string, double>(); 
            for (int i = 1; i <= centredolg.Count; i++)
            {
                for (int y = 1; y <= centredolg2.Count; y++)
                {
                    double a = Math.Pow((centredolg[i.ToString()] - centredolg2[y.ToString()]), 2);
                    double b = Math.Pow((centreshir[i.ToString()] - centreshir2[y.ToString()]), 2);
                    double R = Math.Round(Math.Pow(a + b, 0.5), 2);
                    preres.Add(y.ToString(),R);
                }

                res.Add(i.ToString(), new Dictionary<string, double>(preres));
                preres.Clear();
            }

            return res;
        }

    }
}