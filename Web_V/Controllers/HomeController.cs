using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Web_V.Models;
using System.Globalization;
using System.Linq;
using MathNet.Numerics.Statistics;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Collections.Concurrent;
using System.Web.Routing;

namespace Web_V.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Solve()
        {
            return View();
        }

        public ActionResult Map()
        {
            return View();
        }

        public ActionResult Fileupload()
        {
            return View();
        }

        public static string fileName;
        public static string fileName2;

        [HttpPost]
        public ActionResult Fileupload(HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                // получаем имя файла
                fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Files/" + fileName));
            }
            return RedirectToAction("Test1");
        }

        public ActionResult Fileupload2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Fileupload2(HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                // получаем имя файла
                fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Files/" + fileName));
            }
            return RedirectToAction("Test2");
        }

        public ActionResult Fileupload3()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Fileupload3(HttpPostedFileBase upload, HttpPostedFileBase upload2)
        {
            if (upload != null && upload2 != null )
            {
                // получаем имя файла
                fileName = System.IO.Path.GetFileName(upload.FileName);
                fileName2 = System.IO.Path.GetFileName(upload2.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Files/" + fileName));
                upload2.SaveAs(Server.MapPath("~/Files/" + fileName2));
            }


            return RedirectToAction("Test3", "Home");
        }

        public ActionResult Fileupload4()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Fileupload4(HttpPostedFileBase upload, HttpPostedFileBase upload2)
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


            return RedirectToAction("Test4", "Home");
        }



        static List<List<Double>> Clusters_new = new List<List<Double>>();
        static List<List<Double>> Clusters_new2 = new List<List<Double>>();
        [HttpGet()]
        public ActionResult Test1()
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
                    foreach(string z in arg)
                    {
                        if (z.IndexOf('e') != -1)
                        {
                            int ind = z.IndexOf('e');
                            string z1 = z.Remove(ind);
                            string z2 = z.Substring(ind + 1);
                            double sq = Convert.ToDouble(z2, CultureInfo.InvariantCulture);
                            double res = Convert.ToDouble(z1, CultureInfo.InvariantCulture) * Math.Pow(10, sq);
                            Cluster.Add(res);
                        }
                        else
                        {
                            // double test = Convert.ToDouble(z, CultureInfo.InvariantCulture);
                            Cluster.Add(Convert.ToDouble(z, CultureInfo.InvariantCulture));
                        }
                    }
                    //Console.WriteLine(line);

                    Clusters.Add(new List<double>(Cluster));
                    Cluster.Clear();
                }
            }


            
            List<Double> Cluster_new = new List<double>();
            for (int i = 0; i < 20; i++)
            {
                for(int y = 0; y<Clusters.Count; y++ )
                {
                    Cluster_new.Add(Clusters[y][i]);
                }

                Clusters_new.Add(new List<double>(Cluster_new));
                Cluster_new.Clear();
            }

            //List<int> arr = new List<int> { 12, 36, 48, 999 };
            Double[] arr = Clusters_new[0].ToArray();
            Double[] arr2 = Clusters_new[1].ToArray();

            Statistics s = new Statistics();
            Trendline t = s.CalculateLinearRegression(arr.Select(x => Convert.ToDouble(x)).ToArray(), arr2.Select(x => Convert.ToDouble(x)).ToArray());

            ViewBag.x1 = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(arr2.Min());
            ViewBag.x2 = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(arr2.Max());

            var y1 = t.Slope * arr2.Min() + t.Intercept;
            var y2 = t.Slope * arr2.Max() + t.Intercept;

            ViewBag.y1 = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(y1);
            ViewBag.y2 = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(y2);

            ViewBag.Trendline1 = Convert.ToInt32(t.Start);
            ViewBag.Trendline2 = Convert.ToInt32(t.End);
            ViewBag.Countries = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(arr);
            ViewBag.Countries2 = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(arr2);
            ViewBag.Clusters_new = Clusters_new;
            //ViewBag.test1 = 99;
            ViewBag.y = "";
            return View();
        }

        [HttpPost]
        public ActionResult Test1(string y,string p)
        {
            //List<int> arr = new List<int> { 12, 36, 48, 999 };
            //Double[] arr = { 33, 22, 44, 33, 55, 44, 66 };


            //List<List<Double>> Clusters = new List<List<Double>>();
           
            //Clusters.Add(new List<double> { 0.003895, 0.001301, -0.01205, 0.003486, -0.0006084, 0.003397, 0.0004202, -0.01925, 0.01072, 0.007259, 0.007427, 0.00686, -0.0002649, 0.01117, -0.0002307, -0.01634, 0.006021, -0.01626, 0.00274, 0.0005167, -0.01175, 0.01438 });
            //Clusters.Add(new List<double> { 0.01104, 0.002862, -0.03278, 0.01367, -0.00151, 0.009817, 0.001038, -0.04535, 0.02633, 0.01618, 0.01891, 0.02395, -0.009159, 0.02729, -0.000565, -0.04118, 0.01597, -0.03414, 0.005393, 0.001285, -0.01049, 0.03742 });
            //Clusters.Add(new List<double> { 0.01886, 0.003551, -0.05612, 0.02891, -0.002407, 0.01714, 0.001647, -0.0661, 0.04038, 0.02087, 0.0295, 0.04804, -0.02833, 0.04166, -0.0008885, -0.06554, 0.02661, -0.04395, 0.005192, 0.002055, 0.01638, 0.05986 });
            //Clusters.Add(new List<double> { 0.02617, 0.003039, -0.08086, 0.0479, -0.003299, 0.02464, 0.002247, -0.07968, 0.05172, 0.01968, 0.038, 0.07684, -0.05687, 0.05318, -0.0012, -0.08828, 0.0374, -0.04542, 0.001545, 0.002823, 0.07026, 0.07907 });
            //Clusters.Add(new List<double> { 0.03176, 0.001194, -0.1054, 0.06908, -0.004187, 0.03175, 0.002838, -0.0853, 0.0595, 0.01209, 0.0437, 0.1072, -0.09263, 0.06104, -0.001503, -0.1085, 0.04789, -0.03864, -0.005351, 0.003592, 0.1488, 0.09296 });
            //Clusters.Add(new List<double> { 0.03455, -0.001909, -0.1282, 0.09088, -0.005069, 0.03812, 0.003421, -0.0831, 0.06318, -0.001179, 0.04636, 0.1358, -0.1323, 0.06473, -0.001793, -0.1255, 0.05769, -0.02412, -0.0147, 0.00436, 0.2448, 0.1002 });
            //Clusters.Add(new List<double> { 0.03375, -0.005962, -0.1474, 0.1117, -0.005945, 0.04355, 0.003993, -0.07402, 0.06266, -0.01835, 0.04617, 0.1595, -0.1718, 0.06414, -0.002074, -0.1391, 0.06652, -0.002811, -0.02535, 0.005127, 0.3464, 0.1005 });
            //Clusters.Add(new List<double> { 0.02899, -0.01042, -0.1619, 0.1305, -0.006818, 0.04804, 0.004558, -0.05933, 0.05827, -0.03699, 0.04356, 0.1768, -0.2073, 0.05953, -0.002343, -0.1489, 0.07411, 0.02372, -0.03604, 0.005895, 0.4409, 0.09435 });
            //Clusters.Add(new List<double> { 0.02032, -0.01453, -0.1708, 0.1461, -0.007686, 0.05168, 0.005114, -0.04056, 0.05077, -0.05448, 0.03918, 0.187, -0.2358, 0.05151, -0.002601, -0.1553, 0.0803, 0.05339, -0.04548, 0.006662, 0.5187, 0.08355 });
            //Clusters.Add(new List<double> { 0.008158, -0.01746, -0.1738, 0.158, -0.008548, 0.05459, 0.00566, -0.01913, 0.04119, -0.06857, 0.03375, 0.1905, -0.255, 0.04099, -0.002848, -0.1589, 0.08493, 0.08378, -0.05247, 0.007428, 0.5755, 0.07057 });
            //Clusters.Add(new List<double> { -0.006716, -0.01841, -0.1712, 0.1659, -0.009405, 0.05695, 0.006197, 0.003557, 0.03077, -0.07737, 0.02805, 0.1885, -0.2641, 0.02906, -0.003085, -0.1601, 0.08787, 0.1123, -0.0559, 0.008193, 0.6103, 0.05837 });
            //Clusters.Add(new List<double> { -0.02329, -0.01685, -0.1633, 0.1699, -0.01026, 0.0589, 0.006725, 0.02624, 0.02078, -0.07964, 0.02284, 0.183, -0.2632, 0.01681, -0.003311, -0.1598, 0.08908, 0.1366, -0.05476, 0.008959, 0.6249, 0.05004 });
            //Clusters.Add(new List<double> { -0.04035, -0.01266, -0.1506, 0.1702, -0.0111, 0.06055, 0.007245, 0.0478, 0.01234, -0.07464, 0.01875, 0.1761, -0.2529, 0.005306, -0.003526, -0.1585, 0.08849, 0.1545, -0.04814, 0.009725, 0.6213, 0.04821 });
            //Clusters.Add(new List<double> { -0.05662, -0.006138, -0.1337, 0.1676, -0.01194, 0.0619, 0.007756, 0.06722, 0.006309, -0.06211, 0.01635, 0.1697, -0.2345, -0.004678, -0.003731, -0.1566, 0.08613, 0.1649, -0.0355, 0.01049, 0.6019, 0.05461 });
            //Clusters.Add(new List<double> { -0.0708, 0.002025, -0.1131, 0.163, -0.01277, 0.06293, 0.008257, 0.08364, 0.003145, -0.04232, 0.01593, 0.166, -0.2094, -0.01253, -0.003923, -0.1539, 0.08218, 0.1669, -0.01652, 0.01126, 0.5687, 0.06983 });
            //Clusters.Add(new List<double> { -0.08165, 0.01098, -0.08912, 0.1572, -0.0136, 0.06348, 0.008752, 0.09638, 0.002914, -0.01621, 0.01754, 0.1664, -0.1795, -0.01795, -0.004107, -0.1499, 0.07687, 0.1606, 0.008501, 0.01202, 0.5242, 0.09315 });
            //Clusters.Add(new List<double> { -0.08819, 0.01992, -0.06219, 0.1514, -0.01443, 0.06344, 0.009231, 0.1049, 0.005271, 0.01465, 0.02091, 0.1722, -0.1466, -0.02087, -0.00428, -0.1439, 0.0705, 0.1464, 0.03858, 0.01278, 0.4705, 0.1226 });
            //Clusters.Add(new List<double> { -0.08969, 0.02818, -0.03289, 0.1464, -0.01525, 0.06259, 0.009707, 0.1091, 0.00959, 0.04791, 0.02554, 0.1838, -0.1131, -0.02148, -0.004439, -0.1345, 0.06351, 0.1251, 0.07201, 0.01354, 0.4105, 0.1555 });
            //Clusters.Add(new List<double> { -0.0859, 0.03534, -0.001928, 0.1429, -0.01606, 0.0608, 0.01018, 0.109, 0.01503, 0.08063, 0.03067, 0.2005, -0.08128, -0.02012, -0.004591, -0.1209, 0.05631, 0.09824, 0.1063, 0.0143, 0.3469, 0.1884 });
            //Clusters.Add(new List<double> { -0.07708, 0.04121, 0.02965, 0.1413, -0.01686, 0.05797, 0.01064, 0.105, 0.02071, 0.1096, 0.03552, 0.2208, -0.05314, -0.01725, -0.004731, -0.1019, 0.04928, 0.06721, 0.1383, 0.01506, 0.283, 0.2186 });



            Double[] arr = Clusters_new[Convert.ToInt32(y)].ToArray();
            Double[] arr2 = Clusters_new[Convert.ToInt32(p)].ToArray();




            Statistics s = new Statistics();
            Trendline t = s.CalculateLinearRegression(arr.Select(x => Convert.ToDouble(x)).ToArray(), arr2.Select(x => Convert.ToDouble(x)).ToArray());

            ViewBag.x1 = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(arr2.Min());
            ViewBag.x2 = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(arr2.Max());

            var y1 = t.Slope * arr2.Min() + t.Intercept;
            var y2 = t.Slope * arr2.Max() + t.Intercept;

            ViewBag.y1 = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(y1);
            ViewBag.y2 = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(y2);



            ViewBag.Slope = t.Slope;
            ViewBag.Intercept = t.Intercept;
            ViewBag.Trendline1 = Convert.ToInt32(t.Start);
            ViewBag.Trendline2 = Convert.ToInt32(t.End);
            ViewBag.Countries = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(arr);
            ViewBag.Countries2 = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(arr2);
            ViewBag.Clusters_new = Clusters_new;
            //ViewBag.test1 = 99;
            y = Convert.ToString(Convert.ToInt32(y) + 1);
            p = Convert.ToString(Convert.ToInt32(p) + 1);
            ViewBag.y = "График рассеяния для станций " + y + ", " + p +" и прямая регрессии";
            return View();
        }

        [HttpGet]
        public ActionResult Test2()
        {
            //List<Double> t1 = new List<double> {1,2,3,4,5};
            //List<List<Double>> Clusters = new List<List<Double>>();
            //Clusters1.Add(new List<double> { 0.003895, 0.001301, -0.01205, 0.003486, -0.0006084, 0.003397, 0.0004202, -0.01925, 0.01072, 0.007259, 0.007427, 0.00686, -0.0002649, 0.01117, -0.0002307, -0.01634, 0.006021, -0.01626, 0.00274, 0.0005167, -0.01175, 0.01438 });
            //Clusters1.Add(new List<double> { 0.01104, 0.002862, -0.03278, 0.01367, -0.00151, 0.009817, 0.001038, -0.04535, 0.02633, 0.01618, 0.01891, 0.02395, -0.009159, 0.02729, -0.000565, -0.04118, 0.01597, -0.03414, 0.005393, 0.001285, -0.01049, 0.03742 });
            //Clusters1.Add(new List<double> { 0.01886, 0.003551, -0.05612, 0.02891, -0.002407, 0.01714, 0.001647, -0.0661, 0.04038, 0.02087, 0.0295, 0.04804, -0.02833, 0.04166, -0.0008885, -0.06554, 0.02661, -0.04395, 0.005192, 0.002055, 0.01638, 0.05986 });
            //Clusters1.Add(new List<double> { 0.02617, 0.003039, -0.08086, 0.0479, -0.003299, 0.02464, 0.002247, -0.07968, 0.05172, 0.01968, 0.038, 0.07684, -0.05687, 0.05318, -0.0012, -0.08828, 0.0374, -0.04542, 0.001545, 0.002823, 0.07026, 0.07907 });
            //Clusters1.Add(new List<double> { 0.03176, 0.001194, -0.1054, 0.06908, -0.004187, 0.03175, 0.002838, -0.0853, 0.0595, 0.01209, 0.0437, 0.1072, -0.09263, 0.06104, -0.001503, -0.1085, 0.04789, -0.03864, -0.005351, 0.003592, 0.1488, 0.09296 });
            //Clusters1.Add(new List<double> { 0.03455, -0.001909, -0.1282, 0.09088, -0.005069, 0.03812, 0.003421, -0.0831, 0.06318, -0.001179, 0.04636, 0.1358, -0.1323, 0.06473, -0.001793, -0.1255, 0.05769, -0.02412, -0.0147, 0.00436, 0.2448, 0.1002 });
            //Clusters1.Add(new List<double> { 0.03375, -0.005962, -0.1474, 0.1117, -0.005945, 0.04355, 0.003993, -0.07402, 0.06266, -0.01835, 0.04617, 0.1595, -0.1718, 0.06414, -0.002074, -0.1391, 0.06652, -0.002811, -0.02535, 0.005127, 0.3464, 0.1005 });
            //Clusters1.Add(new List<double> { 0.02899, -0.01042, -0.1619, 0.1305, -0.006818, 0.04804, 0.004558, -0.05933, 0.05827, -0.03699, 0.04356, 0.1768, -0.2073, 0.05953, -0.002343, -0.1489, 0.07411, 0.02372, -0.03604, 0.005895, 0.4409, 0.09435 });
            //Clusters1.Add(new List<double> { 0.02032, -0.01453, -0.1708, 0.1461, -0.007686, 0.05168, 0.005114, -0.04056, 0.05077, -0.05448, 0.03918, 0.187, -0.2358, 0.05151, -0.002601, -0.1553, 0.0803, 0.05339, -0.04548, 0.006662, 0.5187, 0.08355 });
            //Clusters1.Add(new List<double> { 0.008158, -0.01746, -0.1738, 0.158, -0.008548, 0.05459, 0.00566, -0.01913, 0.04119, -0.06857, 0.03375, 0.1905, -0.255, 0.04099, -0.002848, -0.1589, 0.08493, 0.08378, -0.05247, 0.007428, 0.5755, 0.07057 });
            //Clusters1.Add(new List<double> { -0.006716, -0.01841, -0.1712, 0.1659, -0.009405, 0.05695, 0.006197, 0.003557, 0.03077, -0.07737, 0.02805, 0.1885, -0.2641, 0.02906, -0.003085, -0.1601, 0.08787, 0.1123, -0.0559, 0.008193, 0.6103, 0.05837 });
            //Clusters1.Add(new List<double> { -0.02329, -0.01685, -0.1633, 0.1699, -0.01026, 0.0589, 0.006725, 0.02624, 0.02078, -0.07964, 0.02284, 0.183, -0.2632, 0.01681, -0.003311, -0.1598, 0.08908, 0.1366, -0.05476, 0.008959, 0.6249, 0.05004 });
            //Clusters1.Add(new List<double> { -0.04035, -0.01266, -0.1506, 0.1702, -0.0111, 0.06055, 0.007245, 0.0478, 0.01234, -0.07464, 0.01875, 0.1761, -0.2529, 0.005306, -0.003526, -0.1585, 0.08849, 0.1545, -0.04814, 0.009725, 0.6213, 0.04821 });
            //Clusters1.Add(new List<double> { -0.05662, -0.006138, -0.1337, 0.1676, -0.01194, 0.0619, 0.007756, 0.06722, 0.006309, -0.06211, 0.01635, 0.1697, -0.2345, -0.004678, -0.003731, -0.1566, 0.08613, 0.1649, -0.0355, 0.01049, 0.6019, 0.05461 });
            //Clusters1.Add(new List<double> { -0.0708, 0.002025, -0.1131, 0.163, -0.01277, 0.06293, 0.008257, 0.08364, 0.003145, -0.04232, 0.01593, 0.166, -0.2094, -0.01253, -0.003923, -0.1539, 0.08218, 0.1669, -0.01652, 0.01126, 0.5687, 0.06983 });
            //Clusters1.Add(new List<double> { -0.08165, 0.01098, -0.08912, 0.1572, -0.0136, 0.06348, 0.008752, 0.09638, 0.002914, -0.01621, 0.01754, 0.1664, -0.1795, -0.01795, -0.004107, -0.1499, 0.07687, 0.1606, 0.008501, 0.01202, 0.5242, 0.09315 });
            //Clusters1.Add(new List<double> { -0.08819, 0.01992, -0.06219, 0.1514, -0.01443, 0.06344, 0.009231, 0.1049, 0.005271, 0.01465, 0.02091, 0.1722, -0.1466, -0.02087, -0.00428, -0.1439, 0.0705, 0.1464, 0.03858, 0.01278, 0.4705, 0.1226 });
            //Clusters1.Add(new List<double> { -0.08969, 0.02818, -0.03289, 0.1464, -0.01525, 0.06259, 0.009707, 0.1091, 0.00959, 0.04791, 0.02554, 0.1838, -0.1131, -0.02148, -0.004439, -0.1345, 0.06351, 0.1251, 0.07201, 0.01354, 0.4105, 0.1555 });
            //Clusters1.Add(new List<double> { -0.0859, 0.03534, -0.001928, 0.1429, -0.01606, 0.0608, 0.01018, 0.109, 0.01503, 0.08063, 0.03067, 0.2005, -0.08128, -0.02012, -0.004591, -0.1209, 0.05631, 0.09824, 0.1063, 0.0143, 0.3469, 0.1884 });
            //Clusters1.Add(new List<double> { -0.07708, 0.04121, 0.02965, 0.1413, -0.01686, 0.05797, 0.01064, 0.105, 0.02071, 0.1096, 0.03552, 0.2208, -0.05314, -0.01725, -0.004731, -0.1019, 0.04928, 0.06721, 0.1383, 0.01506, 0.283, 0.2186 });
            //Clusters.Add(new List<double> { 3, -88, 77, 0, 5 });

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
                        if (z.IndexOf('e') != -1)
                        {
                            int ind = z.IndexOf('e');
                            string z1 = z.Remove(ind);
                            string z2 = z.Substring(ind+1);
                            double sq = Convert.ToDouble(z2, CultureInfo.InvariantCulture);
                            double res = Convert.ToDouble(z1, CultureInfo.InvariantCulture) * Math.Pow(10, sq);
                            Cluster.Add(res);
                        }
                        else
                        {
                            // double test = Convert.ToDouble(z, CultureInfo.InvariantCulture);
                            Cluster.Add(Convert.ToDouble(z, CultureInfo.InvariantCulture));
                        }
                    }
                    //Console.WriteLine(line);

                    Clusters.Add(new List<double>(Cluster));
                    Cluster.Clear();
                }
            }



            List<Double> Cluster_new = new List<double>();
            for (int i = 0; i < 20; i++)
            {
                for (int y = 0; y < Clusters.Count; y++)
                {
                    Cluster_new.Add(Clusters[y][i]);
                }

                Clusters_new.Add(new List<double>(Cluster_new));
                Cluster_new.Clear();
            }


            List<List<double>> pearsonResult = new List<List<double>>();
            List<List<double>> spearmenResult = new List<List<double>>();

            List<List<double>> pearsonResult2 = new List<List<double>>();
            List<List<double>> spearmenResult2 = new List<List<double>>();
            //Stopwatch test1 = new Stopwatch();
            //test1.Start();
            //Thread.Sleep(94);
            //test1.Stop();
            //Console.WriteLine("{0}",test1.ElapsedMilliseconds);

            //pearsonResult1.Add(new List<double>());
            //spearmenResult1.Add(new List<double>());


            //Console.WriteLine("qwerty");
            //int qwer = 5;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //Parallel.For(0, Clusters1.Count, trr => {

            //    for (int y = 0; y < Clusters1.Count; y++)
            //    {
            //        lock (pearsonResult1)
            //        {
            //            pearsonResult1.Add(Correlation.Pearson(Clusters1[trr], Clusters1[y]));
            //        }
            //        lock (spearmenResult1)
            //        {
            //            spearmenResult1.Add(Correlation.Spearman(Clusters1[trr], Clusters1[y]));
            //        }
            //    }
            //});

            
            stopwatch.Stop();
            Console.Error.WriteLine("Sequential loop time in milliseconds: {0}",
                                    stopwatch.ElapsedMilliseconds);

            //List<List<double>> pearsonResult2 = new List<List<double>>();
            //List<List<double>> spearmenResult2 = new List<List<double>>();

            //for (int i = 0; i < (pearsonResult1.Count/Clusters1.Count); i++)
            //{
            //    pearsonResult2.Add(new List<double>());
            //    spearmenResult2.Add(new List<double>());
            //    for (int y = 0; y < (pearsonResult1.Count / Clusters1.Count); y++)
            //    {
            //        ((List<double>)pearsonResult2[i]).Add(Math.Round(pearsonResult1[i*Clusters1.Count+y], 2));
            //        //((List<double>)spearmenResult2[i]).Add(Math.Round(Correlation.Spearman(Clusters1[i], Clusters1[y]), 2));
            //    }
            //}


            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();
            for (int i = 0; i < Clusters_new.Count; i++)
            {
                pearsonResult2.Add(new List<double>());
                spearmenResult2.Add(new List<double>());
                for (int y = 0; y < Clusters_new.Count; y++)
                {

                    ((List<double>)pearsonResult2[i]).Add(Math.Round(Correlation.Pearson(Clusters_new[i], Clusters_new[y]), 2));
                    ((List<double>)spearmenResult2[i]).Add(Math.Round(Correlation.Spearman(Clusters_new[i], Clusters_new[y]), 2));
                    //double pearson = Correlation.Pearson(Clusters[0], Clusters[1]);
                }
            }
            stopwatch2.Stop();
            Console.Error.WriteLine("Sequential loop time in milliseconds: {0}",
                                    stopwatch2.ElapsedMilliseconds);


            Stopwatch stopwatch3 = new Stopwatch();
            stopwatch3.Start();

            for (int i = 0; i < Clusters_new.Count; i++)
            {
                pearsonResult.Add(new List<double>());
                spearmenResult.Add(new List<double>());
                Thread myThread = new Thread(new ParameterizedThreadStart(tested));
                myThread.Start(i);

            }

            stopwatch3.Stop();
            Console.Error.WriteLine("Sequential loop time in milliseconds: {0}",
                                    stopwatch3.ElapsedMilliseconds);

            void tested(object l)
            {
                int i = (int)l;
               
                for (int y = 0; y < Clusters_new.Count; y++)
                {

                    ((List<double>)pearsonResult[i]).Add(Math.Round(Correlation.Pearson(Clusters_new[i], Clusters_new[y]), 2));
                    ((List<double>)spearmenResult[i]).Add(Math.Round(Correlation.Spearman(Clusters_new[i], Clusters_new[y]), 2));
                    //double pearson = Correlation.Pearson(Clusters[0], Clusters[1]);
                }
            }
            

           // Thread.Sleep(5000);


            //var t = ((List<Double>)Clusters[0])[0];



            ViewBag.Pearson = pearsonResult;
            ViewBag.Spearmen = spearmenResult;

            List<string> low = new List<string>();
            List<string> medium = new List<string>();
            List<string> highmed = new List<string>();
            List<string> hight = new List<string>();

            string testi = "Слабая связь: ";
            string testi2 = "Средняя связь: ";
            string testi3 = "Высокая связь: ";
            string testi4 = "Очень высокая связь: ";

            Thread.Sleep(1000);
            for (int i = 0; i < pearsonResult.Count; i++)
            {
                int k = pearsonResult.Count - i;
                for (int y = i; y <= pearsonResult.Count-1; y++)
                {
                    if (pearsonResult[i][y] > 0.3 && pearsonResult[i][y] < 0.5)
                    {
                        int x = i + 1;
                        int q = y + 1;
                        testi += x + " и " + q + ", ";
                    }

                    if (pearsonResult[i][y] > 0.5 && pearsonResult[i][y] < 0.7)
                    {
                        int x = i + 1;
                        int q = y + 1;
                        testi2 += x + " и " + q + ", ";
                    }

                    if (pearsonResult[i][y] > 0.7 && pearsonResult[i][y] < 0.9)
                    {
                        int x = i + 1;
                        int q = y + 1;
                        testi3 += x + " и " + q + ", ";
                    }

                    if (pearsonResult[i][y] > 0.9 && pearsonResult[i][y] <= 1)
                    {
                        int x = i + 1;
                        int q = y + 1;
                        testi4 += x + " и " + q + ", ";
                    }

                }

            }
            ViewBag.q = "";

            ViewBag.times = stopwatch3.ElapsedMilliseconds;
            ViewBag.times2 = stopwatch2.ElapsedMilliseconds;
            ViewBag.testi = testi;
            ViewBag.testi2 = testi2;
            ViewBag.testi3 = testi3;
            ViewBag.testi4 = testi4;

            return View();
        }

        static List<double> pearsonResult1 = new List<double>();
        static List<double> spearmenResult1 = new List<double>();
        static List<List<Double>> Clusters1 = new List<List<Double>>();

        //void Test()
        //{
        //    for (int i = 0; i < Clusters1.Count; i++)
        //    {
        //        pearsonResult.Add(new List<double>());
        //        spearmenResult.Add(new List<double>());
        //        for (int y = 0; y < Clusters1.Count; y++)
        //        {

        //            ((List<double>)pearsonResult[i]).Add(Math.Round(Correlation.Pearson(Clusters1[i], Clusters1[y]), 2));
        //            ((List<double>)spearmenResult[i]).Add(Math.Round(Correlation.Spearman(Clusters1[i], Clusters1[y]), 2));
        //            //double pearson = Correlation.Pearson(Clusters[0], Clusters[1]);
        //        }
        //    }
        //}


static void trr(int iter)
        {
            //for (int i = 0; i < iter; i++)
            //{
                //pearsonResult1.Add(new List<double>());
                //spearmenResult1.Add(new List<double>());
                for (int y = 0; y < Clusters1.Count; y++)
                {
                pearsonResult1.Add(Correlation.Pearson(Clusters1[iter], Clusters1[y]));
                spearmenResult1.Add(Correlation.Spearman(Clusters1[iter], Clusters1[y]));
                    //((List<double>)pearsonResult1[iter]).Add(Correlation.Pearson(Clusters1[iter], Clusters1[y]));
                    //((List<double>)spearmenResult1[iter]).Add(Correlation.Spearman(Clusters1[iter], Clusters1[y]));
                    //double pearson = Correlation.Pearson(Clusters[0], Clusters[1]);
                }
            //}
        }

        [HttpPost]
        public ActionResult Test2(int s, int s2)
        {
            //List<Double> t1 = new List<double> {1,2,3,4,5};
            List<List<Double>> Clusters = new List<List<Double>>();
            Clusters.Add(new List<double> { 0.003895, 0.001301, -0.01205, 0.003486, -0.0006084, 0.003397, 0.0004202, -0.01925, 0.01072, 0.007259, 0.007427, 0.00686, -0.0002649, 0.01117, -0.0002307, -0.01634, 0.006021, -0.01626, 0.00274, 0.0005167, -0.01175, 0.01438 });
            Clusters.Add(new List<double> { 0.01104, 0.002862, -0.03278, 0.01367, -0.00151, 0.009817, 0.001038, -0.04535, 0.02633, 0.01618, 0.01891, 0.02395, -0.009159, 0.02729, -0.000565, -0.04118, 0.01597, -0.03414, 0.005393, 0.001285, -0.01049, 0.03742 });
            Clusters.Add(new List<double> { 0.01886, 0.003551, -0.05612, 0.02891, -0.002407, 0.01714, 0.001647, -0.0661, 0.04038, 0.02087, 0.0295, 0.04804, -0.02833, 0.04166, -0.0008885, -0.06554, 0.02661, -0.04395, 0.005192, 0.002055, 0.01638, 0.05986 });
            Clusters.Add(new List<double> { 0.02617, 0.003039, -0.08086, 0.0479, -0.003299, 0.02464, 0.002247, -0.07968, 0.05172, 0.01968, 0.038, 0.07684, -0.05687, 0.05318, -0.0012, -0.08828, 0.0374, -0.04542, 0.001545, 0.002823, 0.07026, 0.07907 });
            Clusters.Add(new List<double> { 0.03176, 0.001194, -0.1054, 0.06908, -0.004187, 0.03175, 0.002838, -0.0853, 0.0595, 0.01209, 0.0437, 0.1072, -0.09263, 0.06104, -0.001503, -0.1085, 0.04789, -0.03864, -0.005351, 0.003592, 0.1488, 0.09296 });
            Clusters.Add(new List<double> { 0.03455, -0.001909, -0.1282, 0.09088, -0.005069, 0.03812, 0.003421, -0.0831, 0.06318, -0.001179, 0.04636, 0.1358, -0.1323, 0.06473, -0.001793, -0.1255, 0.05769, -0.02412, -0.0147, 0.00436, 0.2448, 0.1002 });
            Clusters.Add(new List<double> { 0.03375, -0.005962, -0.1474, 0.1117, -0.005945, 0.04355, 0.003993, -0.07402, 0.06266, -0.01835, 0.04617, 0.1595, -0.1718, 0.06414, -0.002074, -0.1391, 0.06652, -0.002811, -0.02535, 0.005127, 0.3464, 0.1005 });
            Clusters.Add(new List<double> { 0.02899, -0.01042, -0.1619, 0.1305, -0.006818, 0.04804, 0.004558, -0.05933, 0.05827, -0.03699, 0.04356, 0.1768, -0.2073, 0.05953, -0.002343, -0.1489, 0.07411, 0.02372, -0.03604, 0.005895, 0.4409, 0.09435 });
            Clusters.Add(new List<double> { 0.02032, -0.01453, -0.1708, 0.1461, -0.007686, 0.05168, 0.005114, -0.04056, 0.05077, -0.05448, 0.03918, 0.187, -0.2358, 0.05151, -0.002601, -0.1553, 0.0803, 0.05339, -0.04548, 0.006662, 0.5187, 0.08355 });
            Clusters.Add(new List<double> { 0.008158, -0.01746, -0.1738, 0.158, -0.008548, 0.05459, 0.00566, -0.01913, 0.04119, -0.06857, 0.03375, 0.1905, -0.255, 0.04099, -0.002848, -0.1589, 0.08493, 0.08378, -0.05247, 0.007428, 0.5755, 0.07057 });
            Clusters.Add(new List<double> { -0.006716, -0.01841, -0.1712, 0.1659, -0.009405, 0.05695, 0.006197, 0.003557, 0.03077, -0.07737, 0.02805, 0.1885, -0.2641, 0.02906, -0.003085, -0.1601, 0.08787, 0.1123, -0.0559, 0.008193, 0.6103, 0.05837 });
            Clusters.Add(new List<double> { -0.02329, -0.01685, -0.1633, 0.1699, -0.01026, 0.0589, 0.006725, 0.02624, 0.02078, -0.07964, 0.02284, 0.183, -0.2632, 0.01681, -0.003311, -0.1598, 0.08908, 0.1366, -0.05476, 0.008959, 0.6249, 0.05004 });
            Clusters.Add(new List<double> { -0.04035, -0.01266, -0.1506, 0.1702, -0.0111, 0.06055, 0.007245, 0.0478, 0.01234, -0.07464, 0.01875, 0.1761, -0.2529, 0.005306, -0.003526, -0.1585, 0.08849, 0.1545, -0.04814, 0.009725, 0.6213, 0.04821 });
            Clusters.Add(new List<double> { -0.05662, -0.006138, -0.1337, 0.1676, -0.01194, 0.0619, 0.007756, 0.06722, 0.006309, -0.06211, 0.01635, 0.1697, -0.2345, -0.004678, -0.003731, -0.1566, 0.08613, 0.1649, -0.0355, 0.01049, 0.6019, 0.05461 });
            Clusters.Add(new List<double> { -0.0708, 0.002025, -0.1131, 0.163, -0.01277, 0.06293, 0.008257, 0.08364, 0.003145, -0.04232, 0.01593, 0.166, -0.2094, -0.01253, -0.003923, -0.1539, 0.08218, 0.1669, -0.01652, 0.01126, 0.5687, 0.06983 });
            Clusters.Add(new List<double> { -0.08165, 0.01098, -0.08912, 0.1572, -0.0136, 0.06348, 0.008752, 0.09638, 0.002914, -0.01621, 0.01754, 0.1664, -0.1795, -0.01795, -0.004107, -0.1499, 0.07687, 0.1606, 0.008501, 0.01202, 0.5242, 0.09315 });
            Clusters.Add(new List<double> { -0.08819, 0.01992, -0.06219, 0.1514, -0.01443, 0.06344, 0.009231, 0.1049, 0.005271, 0.01465, 0.02091, 0.1722, -0.1466, -0.02087, -0.00428, -0.1439, 0.0705, 0.1464, 0.03858, 0.01278, 0.4705, 0.1226 });
            Clusters.Add(new List<double> { -0.08969, 0.02818, -0.03289, 0.1464, -0.01525, 0.06259, 0.009707, 0.1091, 0.00959, 0.04791, 0.02554, 0.1838, -0.1131, -0.02148, -0.004439, -0.1345, 0.06351, 0.1251, 0.07201, 0.01354, 0.4105, 0.1555 });
            Clusters.Add(new List<double> { -0.0859, 0.03534, -0.001928, 0.1429, -0.01606, 0.0608, 0.01018, 0.109, 0.01503, 0.08063, 0.03067, 0.2005, -0.08128, -0.02012, -0.004591, -0.1209, 0.05631, 0.09824, 0.1063, 0.0143, 0.3469, 0.1884 });
            Clusters.Add(new List<double> { -0.07708, 0.04121, 0.02965, 0.1413, -0.01686, 0.05797, 0.01064, 0.105, 0.02071, 0.1096, 0.03552, 0.2208, -0.05314, -0.01725, -0.004731, -0.1019, 0.04928, 0.06721, 0.1383, 0.01506, 0.283, 0.2186 });

            List<List<double>> pearsonResult = new List<List<double>>();
            List<List<double>> spearmenResult = new List<List<double>>();

            for (int i = 0; i < Clusters.Count; i++)
            {
                pearsonResult.Add(new List<double>());
                spearmenResult.Add(new List<double>());
                for (int y = 0; y < Clusters.Count; y++)
                {
                    ((List<double>)pearsonResult[i]).Add(Correlation.Pearson(Clusters[i], Clusters[y]));
                    ((List<double>)spearmenResult[i]).Add(Correlation.Spearman(Clusters[i], Clusters[y]));
                    //double pearson = Correlation.Pearson(Clusters[0], Clusters[1]);
                }
            }
            //var t = ((List<Double>)Clusters[0])[0];

            ViewBag.Pearson = pearsonResult;
            ViewBag.Spearmen = spearmenResult;
            ViewBag.q = "";
            ViewBag.s = s;
            ViewBag.s2 = s2;

            return View();
        }

        [HttpGet]
        public ActionResult Test3()
        {

            string path = Server.MapPath("~/Files/" + fileName);
            string path2 = Server.MapPath("~/Files/" + fileName2);

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
                        if (z.IndexOf('e') != -1)
                        {
                            int ind = z.IndexOf('e');
                            string z1 = z.Remove(ind);
                            string z2 = z.Substring(ind + 1);
                            double sq = Convert.ToDouble(z2, CultureInfo.InvariantCulture);
                            double res = Convert.ToDouble(z1, CultureInfo.InvariantCulture) * Math.Pow(10, sq);
                            Cluster.Add(res);
                        }
                        else
                        {
                            // double test = Convert.ToDouble(z, CultureInfo.InvariantCulture);
                            Cluster.Add(Convert.ToDouble(z, CultureInfo.InvariantCulture));
                        }
                    }
                    //Console.WriteLine(line);

                    Clusters.Add(new List<double>(Cluster));
                    Cluster.Clear();
                }
            }



            List<Double> Cluster_new = new List<double>();
            for (int i = 0; i < Clusters[0].Count; i++)
            {
                for (int y = 0; y < Clusters.Count; y++)
                {
                    Cluster_new.Add(Clusters[y][i]);
                }

                Clusters_new.Add(new List<double>(Cluster_new));
                Cluster_new.Clear();
            }

            Clusters.Clear();
            using (StreamReader sr = new StreamReader(path2, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {

                    string[] arg = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string z in arg)
                    {
                        if (z.IndexOf('e') != -1)
                        {
                            int ind = z.IndexOf('e');
                            string z1 = z.Remove(ind);
                            string z2 = z.Substring(ind + 1);
                            double sq = Convert.ToDouble(z2, CultureInfo.InvariantCulture);
                            double res = Convert.ToDouble(z1, CultureInfo.InvariantCulture) * Math.Pow(10, sq);
                            Cluster.Add(res);
                        }
                        else
                        {
                            // double test = Convert.ToDouble(z, CultureInfo.InvariantCulture);
                            Cluster.Add(Convert.ToDouble(z, CultureInfo.InvariantCulture));
                        }
                    }
                    //Console.WriteLine(line);

                    Clusters.Add(new List<double>(Cluster));
                    Cluster.Clear();
                }
            }

            List<Double> Cluster_new2 = new List<double>();
            for (int i = 0; i < Clusters[0].Count; i++)
            {
                for (int y = 0; y < Clusters.Count; y++)
                {
                    Cluster_new2.Add(Clusters[y][i]);
                }

                Clusters_new2.Add(new List<double>(Cluster_new2));
                Cluster_new2.Clear();
            }

            List<List<double>> pearsonResult = new List<List<double>>();
            List<List<double>> spearmenResult = new List<List<double>>();

            for (int i = 0; i < Clusters_new.Count; i++)
            {
                pearsonResult.Add(new List<double>());
                spearmenResult.Add(new List<double>());
                Thread myThread = new Thread(new ParameterizedThreadStart(tested));
                myThread.Start(i);

            }


            void tested(object l)
            {
                int i = (int)l;

                for (int y = 0; y < Clusters_new2.Count; y++)
                {

                    ((List<double>)pearsonResult[i]).Add(Math.Round(Correlation.Pearson(Clusters_new[i], Clusters_new2[y]), 2));
                    ((List<double>)spearmenResult[i]).Add(Math.Round(Correlation.Spearman(Clusters_new[i], Clusters_new2[y]), 2));
                    //double pearson = Correlation.Pearson(Clusters[0], Clusters[1]);
                }
            }

            Thread.Sleep(1000);
            ViewBag.Pearson = pearsonResult;
            ViewBag.Spearmen = spearmenResult;


            return View();
        }

            public ActionResult Upload(HttpPostedFileBase upload, string radius_str)
        {
            if (radius_str == "" || upload == null)//валидация
            {
                List<Error> err = new List<Error>();
                err.Add(new Error() { error = 0 });
                return Json(err, JsonRequestBehavior.AllowGet);
            }
                string CurrentDecimalSeparator = NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
                string output = "";
                double radius;

                //Проверяю как пользователь ввел радиус, через точку или запятую.
                if (radius_str.Contains("."))
                {
                    radius = Double.Parse(radius_str.Replace(",", CurrentDecimalSeparator).Replace(".", CurrentDecimalSeparator));
                }
                else
                {
                    radius = Double.Parse(radius_str.Replace(".", CurrentDecimalSeparator).Replace(",", CurrentDecimalSeparator));
                }

                List<Dano> RegisteredUsers = new List<Dano>();
                string Path = Server.MapPath("~/Files/inpup_file.txt");//Путь к файлу
                upload.SaveAs(Path); // сохраняю файл в проекте

                string[] separatingStrings = { " ", "\n" };//символы, по которым будет разбиваьтся считываемый фаил
                //double[] words; //массив, в который записываются разбитые значения
                double?[,] start;//массив, в который записываются разбитые значения т.е наши исходные данные
                double?[,] start_out;//массив, в котором записаны наши исходные данные (те, которые нужны для вывода)
                int quantity_line = System.IO.File.ReadAllLines(Path).Length;//Количество строк в считываемом файле
                int quantity_column = 4;//Количество столбцов в считываемом файле

                start = new double?[quantity_line, quantity_column];
                start_out = new double?[quantity_line, quantity_column];

                    using (StreamReader file = new StreamReader(Path))
                    {
                        int k = 0;
                        while ((output = file.ReadLine()) != null)
                        {
                            string[] split = output.Split(separatingStrings, StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < quantity_column; j++)
                            {
                                double var = Double.Parse(split[j].Replace(".", CurrentDecimalSeparator).Replace(",", CurrentDecimalSeparator));
                            
                                start[k, j] = var;
                                start_out[k, j] = var;
                            }
                            k++;
                        }
                
                    }

                /* using (StreamReader file = new StreamReader(Path))//записываю считываемые данные в массив start и start_out
                 {
                     words = Array.ConvertAll<string, double>(file.ReadToEnd().Split(separatingStrings, StringSplitOptions.RemoveEmptyEntries), Double.Parse);
                     quantity_column = words.Length / quantity_line;
                     start = new double?[quantity_line, quantity_column];
                     start_out = new double?[quantity_line, quantity_column];
                     int k = 0;
                     for (int i = 0; i < quantity_line; i++)
                     {
                         for (int j = 0; j < quantity_column; j++)
                         {

                             start[i, j] = Convert.ToDouble(words[k]);
                             start_out[i, j] = Convert.ToDouble(words[k]);
                             k++;
                         }
                     }
                 }*/

                int T_number = 0;//индекс температуры
                int T_sr_index = 4;//индекс средней температуры
                double?[,] ymenshaemoe;//массив, из которого буду вычитать(уменьшаемое)
                double?[,] source;//массив, из которого буду брать значения
                double?[,] vichitaemoe;//массив, который буду вычитать(вычитаеиое)
                double?[,] raznost;//разность массивов ymenshaemoe и chastnoe
                double?[,] sr;//сюда записываю среднее значение
                double?[] conec_iterachii;//проверка конца итерации

                Massiv massiv = new Massiv();
                Resolve res = new Resolve();
                Sort sort = new Sort();
                Number_group ng = new Number_group();

                source = massiv.metod_ymenshaemoe(quantity_line, start, T_number);// Массив, из которого берутся значения по умолчанию
                do
                {
                    ymenshaemoe = massiv.metod_ymenshaemoe(quantity_line, start, T_number);
                    vichitaemoe = massiv.metod_vichitaemoe(quantity_line, start, T_number);
                    raznost = res.metod_raznost(quantity_line, ymenshaemoe, vichitaemoe, radius, source);//Вычитаю из массива ymenshaemoe массив vichitaemoe, записываю в массив raznost
                    sr = res.metod_srednee(quantity_line, raznost);
                    conec_iterachii = res.metod_proverka(quantity_line, start, sr, T_number);
                    start = sr;

                    double endt = 0;
                    foreach (double j in conec_iterachii)
                    {
                        endt += Math.Abs(j);
                    }
                    if (endt == 0)
                    {
                        break;
                    }
                }
                while (true);

                double?[,] out_print = new double?[quantity_line, quantity_column + 1];
                for (int i = 0; i < quantity_line; i++)
                {
                    for (int j = 0; j < quantity_column; j++)
                    {
                        out_print[i, j] = start_out[i, j];
                    }
                    out_print[i, quantity_column] = sr[i, 0];
                }

                sort.method_sotrirovka(quantity_column + 1, quantity_line, out_print, T_sr_index);
                double?[,] out_print_group = new double?[quantity_line, quantity_column + 2];
                out_print_group = ng.method_number_group(quantity_column + 1, quantity_line, out_print, T_sr_index);

                //запись в фаил .txt
                string Path_answer = Server.MapPath("~/Files/answer.txt");
                using (StreamWriter sw = new StreamWriter(Path_answer, false, System.Text.Encoding.Default))
                {
                    for (int i = 0; i < quantity_line; i++)
                    {
                        for (int j = 0; j < quantity_column + 2; j++)
                        {
                            sw.Write(out_print_group[i, j] + " ");
                        }
                        sw.WriteLine();
                    }
                }
                //формирование фаила .json для отправки в представление
                for (int i = 0; i < quantity_line; i++)
                {
                    RegisteredUsers.Add(new Dano() { Temp = out_print_group[i, 0], lat = out_print_group[i, 1], long_ = out_print_group[i, 2], number_station = out_print_group[i, 3], Tc = out_print_group[i, 4], number_group = out_print_group[i, 5] });
                }
                return Json(RegisteredUsers, JsonRequestBehavior.AllowGet);
        }
    }
}