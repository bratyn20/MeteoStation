
namespace Web_V.Controllers
{
    class Sort
    {
        public double?[,] method_sotrirovka(int quantity_column, int num_string, double?[,] nums, int index_Tc)
        {
            //Сортировка
            double?[] temp = new double?[quantity_column];
            for (int i = 0; i < num_string - 1; i++)
            {
                for (int j = i + 1; j < num_string; j++)
                {
                    if (nums[i, index_Tc] > nums[j, index_Tc])
                    {
                        for (int r = 0; r < quantity_column; r++)
                        {
                            temp[r] = nums[i, r];
                            nums[i, r] = nums[j, r];
                            nums[j, r] = temp[r];
                        }
                    }
                }
            }
            return nums;
        }
    }
}