using System;

namespace Web_V.Controllers
{
    class Number_group
    {
        public double?[,] method_number_group(int razmer, int num_string, double?[,] nums, int index_Tc)
        {
            double?[,] ddd = new double?[num_string, razmer + 1];
            int k = 1;
            for (int i = 0; i < num_string - 1; i++)
            {
                for (int j = 0; j < razmer; j++)
                {
                    ddd[i, j] = nums[i, j];
                    ddd[i + 1, j] = nums[i + 1, j];
                }

                if (Math.Round((double)nums[i, index_Tc], 4) == (Math.Round((double)nums[i + 1, index_Tc], 4)))
                {
                    ddd[i, razmer] = k;
                }
                else
                {
                    ddd[i, razmer] = k;
                    k++;
                }
                ddd[i + 1, razmer] = k;
            }
            return ddd;
        }
    }
}
