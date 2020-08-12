using System;

namespace Web_V.Controllers
{
    class Resolve
    {
        //-------------------------------------Вычитаю из массива ymenshaemoe массив vichitaemoe, записываю в массив raznost-----------------------------
        public double?[,] metod_raznost(int razmer, double?[,] ymenshaemoe, double?[,] vichitaemoe, double? radius, double?[,] sourse)
        {
            double?[,] raznost = new double?[razmer, razmer];
            for (int i = 0; i < razmer; i++)
            {
                for (int j = 0; j < razmer; j++)
                {
                    double? znachenie_raznosti = ymenshaemoe[i, j] - vichitaemoe[i, j];
                    if (Math.Abs((double)znachenie_raznosti) > radius)
                    {
                        raznost[i, j] = null;

                    }
                    else
                    {
                        raznost[i, j] = sourse[i, j];
                    }
                }
            }
            return raznost;
        }

        //---------------------------------------среднее---------------------
        public double?[,] metod_srednee(int razmer, double?[,] raznost)
        {
            double?[,] sr = new double?[razmer, razmer];
            //меняю в масиве sr null на нули
            for (int i = 0; i < razmer; i++)
            {
                sr[i,0] = 0;
            }

            for (int i = 0; i < razmer; i++)
            {
                int n = 0;

                for (int j = 0; j < razmer; j++)
                {
                    double? chislo = raznost[j, i];
                    if (chislo != null)
                    {
                        sr[i,0] = sr[i,0] + chislo;
                        n++;
                    }
                    else
                    {
                        sr[i,0] = sr[i,0];
                    }
                }
                sr[i,0] = sr[i,0] / n;
            }
            return sr;
        }
        //------------------------------------------------------------------

        //---------------------------------------Проверка---------------------
        public double?[] metod_proverka(int razmer, double?[,] start, double?[,] sr, int T_number)
        {
            double?[] conec_iterachii = new double?[razmer];
            for (int i = 0; i < razmer; i++)
            {
                conec_iterachii[i] = start[i, T_number] - sr[i,0];
            }
            return conec_iterachii;
        }
        //------------------------------------------------------------------

    }
}
