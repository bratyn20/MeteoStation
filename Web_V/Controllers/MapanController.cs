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
            string path = Server.MapPath("~/Files/" + fileName);

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

            Dictionary<int, double> testmin = new Dictionary<int, double>();
            Dictionary<int, double> testmax = new Dictionary<int, double>();
            Dictionary<int, double> test2min = new Dictionary<int, double>();
            Dictionary<int, double> test2max = new Dictionary<int, double>();
            MinMax(Clusters, 1, 0, testmin, testmax);
            MinMax(Clusters, 1, 1, test2min, test2max);

            return View();
        }

        public static void MinMax(List<List<double>> file, int k, int mas, Dictionary<int,double> testmin, Dictionary<int, double> testmax)
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

            testmin.Add(k, one.Min());
            testmax.Add(k, one.Max());

            MinMax(file, k + 1, mas, testmin, testmax);
        }

    }
}