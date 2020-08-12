
namespace Web_V.Controllers
{
    class Massiv
    {
        //-------------------------------------Заполняю массив ymenshaemoe-----------------------------
        public double?[,] metod_ymenshaemoe(int razmer, double?[,] start, int T_number)
        {
            double?[,] ymenshaemoe = new double?[razmer, razmer];
            for (int i = 0; i < razmer; i++)
            {
                int k = 0;
                for (int j = 0; j < razmer; j++)
                {
                    int var = j + i;
                    if (var >= razmer)
                    {
                        var = k;
                        k++;
                    }
                    ymenshaemoe[i, j] = start[var, T_number];
                }
            }
            return ymenshaemoe;
        }
        //------------------------------------------------------------------
        //-------------------------------------Заполняю массив vichitaemoe-----------------------------
        public double?[,] metod_vichitaemoe(int razmer, double?[,] start, int T_number)
        {
            double?[,] vichitaemoe = new double?[razmer, razmer];
            for (int i = 0; i < razmer; i++)
            {
                for (int j = 0; j < razmer; j++)
                {
                    int var = j;
                    vichitaemoe[i, j] = start[var, T_number];
                }
            }
            return vichitaemoe;
        }
        //------------------------------------------------------------------
    }
}
