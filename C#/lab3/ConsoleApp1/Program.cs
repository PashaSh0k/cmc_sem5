using System;
using System.Runtime.InteropServices;
internal class Program
{
    public class VML_Test
    {
        
        public VML_Test(double[] grid1)
        {
            grid = new double[grid1.Length];
            rezHA = new double[grid1.Length];
            rezEP = new double[grid1.Length];
            for (int i = 0; i < grid1.Length; i++)
            {
                grid[i] = grid1[i];
            }
        }
        public VML_Test(int n)
        {
            grid = new double[n];
            rezHA = new double[n];
            rezEP = new double[n];
            Random random = new Random();
            for (int i = 0; i < n; i++) {
                grid[i] =  random.NextDouble() * 10;
            };
        }
        public double[] grid { get; private set; }
        public double[] rezHA { get; private set; }
        public double[] rezEP { get; private set; }

        public int position = -1;
        public double max_value {
            get
            {
                double max_value = -1;
                for (int i = 0; i < grid.Length; i++)
                    if (Math.Abs(rezHA[i] - rezEP[i]) > max_value)
                    {
                        max_value = Math.Abs(rezHA[i] - rezEP[i]);
                        position = i;
                    }
                return max_value;
            }
        }
        public double max_point
        {
            get
            {
                double max_value = -1;
                for (int i = 0; i < grid.Length; i++)
                    if (Math.Abs(rezHA[i] - rezEP[i]) > max_value)
                    {
                        max_value = Math.Abs(rezHA[i] - rezEP[i]);
                        position = i;
                    }
                return grid[position];
            }
        }

        public double timeHA { get; private set; }
        public double timeEP { get; private set; }

        public double k {
            get
            {
                return timeHA / timeEP;
            }
        }
        public override string ToString()
        {
            string result;
            result = grid.Length.ToString() + " " + max_point.ToString() + " " + max_value.ToString() + " " + timeHA.ToString() + " " + timeEP.ToString() + " " + k.ToString();
            return result;
        }
        public string ToLongString()
        {
            string result = this.ToString();
            result += "\n";
            for (int i = 0; i < grid.Count(); i++)
            {
                result += i.ToString() + ": point = " + String.Format("{0,6:F4}", grid[i]) + " VML_HA value = " + String.Format("{0,6:F4}", rezHA[i]) + " and VML_EP value = " + String.Format("{0,6:F4}", rezEP[i]) + "\n";
            }
            return result;
        }
        public bool Save(string filename)
        {
            StreamWriter fs = null;
            try
            {
                fs = new StreamWriter(filename, true);
                string str = ToString();
                fs.Write(str);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }
        public void F()
        {
            int n = grid.Length;
            double timeHA = 0;
            double timeEP = 0;
            try
            {
                timeHA = func(n, grid, rezHA, 1);
                timeEP = func(n, grid, rezEP, 0);
                Save("test.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
    private static void Main(string[] args)
    {
        VML_Test test1 = new VML_Test(500);
        Console.WriteLine(test1.ToLongString());
        test1.F();
        Console.WriteLine(test1.ToLongString());
        double[] for_test = new double[] { 0.1, 1.57, 0.314, 0.00001, -1.56 };
        VML_Test test2 = new VML_Test(for_test);
        Console.WriteLine(test2.ToLongString());
        test2.F();
        Console.WriteLine(test2.ToLongString());
    }
    [DllImport("Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern double func(int n, double[] a, double[] y, int mode);
}