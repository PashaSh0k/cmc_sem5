using System;
using System.Numerics;
using System.Collections;

/*
FComplex F = ToComplex.func;
// 1
Console.WriteLine("*First task*");
Console.WriteLine("-----------------------------------------------------");
V1DataList List = new ("first", DateTime.Now);
Complex cmplx = new Complex(10, 20);
List.Add(5.0, cmplx);
List.Add(6.0, cmplx*cmplx);
Console.WriteLine(List.ToLongString("{0,4:F2}"));
V1DataNUGrid NUGrid = (V1DataNUGrid)List;
Console.WriteLine(NUGrid.ToLongString("{0,4:F2}"));
Console.WriteLine("-----------------------------------------------------");

// 2
Console.WriteLine("*Second task*");
Console.WriteLine("-----------------------------------------------------");
V1DataCollection Collection = new V1DataCollection();
Collection.AddDefaults();
Console.WriteLine(Collection.ToLongString("{0,4:F2}"));
Console.WriteLine("-----------------------------------------------------");

// 3
Console.WriteLine("*Third task*");
Console.WriteLine("-----------------------------------------------------");
for (int i = 0; i < Collection.Count; i++)
{
    Console.WriteLine("{0,4:F2}", Collection[i].MaxMagnitude);
}
Console.WriteLine("-----------------------------------------------------");
*/

// 1
FComplex F = ToComplex.func;
Console.WriteLine("*First task*");
Console.WriteLine("-----------------------------------------------------");
static void SaveToLoad(V1DataUGrid tosave, V1DataUGrid toload, string path){
    bool saved = tosave.Save(path);
    if (saved){
        bool loaded = V1DataUGrid.Load(path, ref toload);
        if (!loaded){
            Console.WriteLine("NOT LOADING");
        }
    } else {
        Console.WriteLine("NOT SAVING");
    }
}
/*
UniformGrid test1grid = new UniformGrid(-5, 5, 6);
V1DataUGrid test1savedata = new V1DataUGrid("SAVEDATA", DateTime.Now, test1grid, F);
V1DataUGrid test1loaddata = new V1DataUGrid("LOADDATA", new DateTime(1,1,1));

Console.WriteLine(test1savedata.ToLongString("{0,4:F2}"));
SaveToLoad(test1savedata, test1loaddata, "test.txt")
Console.WriteLine(test1loaddata.ToLongString("{0,4:F2}"));

Console.WriteLine("-----------------------------------------------------");
*/
//2
Console.WriteLine("*Second task*");
Console.WriteLine("-----------------------------------------------------");
V1DataCollection Collection = new();
Collection.AddDefaults();
Collection.Add(new V1DataList("EmptyList", DateTime.Now));
Collection.Add(new V1DataUGrid("EmptyUgrid", DateTime.Now));
Collection.Add(new V1DataNUGrid("EmptyNUgrid", DateTime.Now));
Console.WriteLine(Collection.ToLongString("{0,4:F2}"));

Console.WriteLine($"Average Magnitude =  {Collection.AverageLINQ}");
Console.WriteLine($"Max variation from avg: {Collection.devLINQ}");
Console.Write("Points meet at least in two different collectrions: ");
foreach (var item in Collection.repeatLINQ){
    Console.Write(item);
    Console.Write(" ");
}
Console.WriteLine();
Console.WriteLine("-----------------------------------------------------");

static class ToComplex
{
    public static Complex func(double x)
    {
        return new Complex(x, 0);
    }
}
public struct DataItem
{
    public double x { get; set; }
    public System.Numerics.Complex value { get; set; }
    public DataItem(double a, Complex b)
    {
        x = a;
        value = b;
    }
    public string ToLongString(string format)
    {
        string result = string.Format(format, x) + string.Format(format, value);
        return result;
    }
    public override string ToString()
    {
        string result =  x.ToString() + " " + value.ToString();
        return result;
    }
}

public struct UniformGrid
{
    public double a { get; set; }
    public double b { get; set; }
    public int count { get; set; }
    public double step 
    {
        get { return Math.Abs(b - a) / count; }
    }
    public UniformGrid(double a1, double b1, int c)
    {
        a = a1;
        b = b1;
        count = c; 
    }
    public string ToLongString(string format)
    {
        string result = string.Format(format, a) + string.Format(format, b) + string.Format(format, count);
        return result;
    }
    public override string ToString()
    {
        string result =  a.ToString() + " " + b.ToString() + " " + count.ToString();
        return result;
    }
}

public delegate Complex FComplex(double x);

public abstract class V1Data : IEnumerable<DataItem>
{
    List<DataItem> node;
    public string str { set; get; }
    public DateTime date { set; get; }
    virtual public double MaxMagnitude{ get; set; }
    public V1Data(string s, DateTime dt)
    {
        str = s;
        date = dt;
    }
    public abstract string ToLongString(string format);
    public override string ToString()
    {
        string result =  str + " " + date.ToString();
        return result;
    }
    public abstract IEnumerator<DataItem> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator(){
        return node.GetEnumerator();
    }
} 

public class V1DataList : V1Data
{
    List<DataItem> node { get; set; }
    public V1DataList(string s, DateTime dt)  : base(s, dt)
    {
        node = new List<DataItem>();
    }
    public bool Add(double x, Complex field)
    {
        for (int i = 0; i < node.Count(); i++)
        {
            if(node[i].x == x)
            {
                return false;
            }
        }
        DataItem addelem = new DataItem(x, field);
        node.Add(addelem);
        return true;
    }
    public void AddDefaults(int nItems, FComplex F)
    {
        for(int i = 0; i < nItems; i++)
        {
            Random randomize = new Random();
            double n = randomize.Next(0, 10);
            Add(n, F(n)); 
        }
    }
    public override double MaxMagnitude
    {
        get
        {
            Complex maxim = 0;
            for(int i = 0; i < node.Count(); i++)
            {
                if(node[i].value.Magnitude > maxim.Magnitude)
                {
                    maxim = node[i].value;
                }
            }
            return maxim.Magnitude;
        }
    }
    public static explicit operator V1DataNUGrid(V1DataList source)
    {
        V1DataNUGrid result = new V1DataNUGrid(source.str, source.date);
        result.mas = new double[source.node.Count];
        for (int i = 0; i < source.node.Count; i++)
        {
            result.mas[i] = source.node[i].x;
        }
        result.array = new Complex[source.node.Count];
        for (int i = 0; i < source.node.Count; i++)
        {
            result.array[i] = source.node[i].value;
        }
        return result;
    }
    public override string ToString()
    {
        string result = System.ComponentModel.TypeDescriptor.GetClassName(this);
        result += " " + str + " " + date.ToString() + " " + string.Format("{0:F2}", MaxMagnitude) + " " + (node.Count).ToString();
        return result;
    }
    public override string ToLongString(string format)
    {
        //string result = str.ToString() + " " + date.ToString() + " " + string.Format("{0:F2}", MaxMagnitude) + " " + (node.Count).ToString() + "\n";
        string result = ToString();
        result += "\n";
        for(int i = 0; i < node.Count; i++)
        {
            result += i.ToString() + ": point = " + string.Format(format, node[i].x) + " with value = ";
            result += string.Format(format, node[i].value) + "\n";
        }
        return result;
    }
    public override IEnumerator<DataItem> GetEnumerator()
    {
        return node.GetEnumerator();
    }
}

public class V1DataUGrid : V1Data
{
    public List<DataItem> node = new List<DataItem>();
    public UniformGrid grid { get; set; }
    public Complex[] array { get; set; }
    public V1DataUGrid(string s, DateTime dt) : base(s, dt)
    {
        array = new Complex[0];
    }
    public V1DataUGrid(string s, DateTime dt, UniformGrid grid1, FComplex F) : base(s, dt)
    {
        grid = grid1;
        array = new Complex[grid.count];
        for(int i = 0; i < array.Length; i++)
        {
            array[i] = F(grid.a + grid.step * i);
        }
        double pointer = grid.a;
        double step = grid.step;
        int count = 0;
        while(count < array.Count()){
            node.Add(new DataItem(pointer, array[count]));
            count += 1;
            pointer += step;
        }
    }
    public override double MaxMagnitude
    {
        get
        {
            Complex maxim = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Magnitude > maxim.Magnitude)
                {
                    maxim = array[i];
                }
            }
            return maxim.Magnitude;
        }
    }
    public override string ToString()
    {
        string result = System.ComponentModel.TypeDescriptor.GetClassName(this);
        result += " " + str + " " + date.ToString() + " " + string.Format("{0:F2}", MaxMagnitude) + " " + grid.ToString();
        return result;
    }
    public override string ToLongString(string format)
    {
        //string result = str + " " + date.ToString() + " " + string.Format("{0:F2}", MaxMagnitude) + " " + grid.ToString() + "\n";
        string result = ToString();
        result += "\n";
        for (int i = 0; i < array.Length; i++)
        {
            result += i.ToString() + ": point = " + string.Format(format, grid.a + grid.step * i) + " with value = ";
            result += string.Format(format, array[i]) + "\n";
        }
        return result;
    }
    public override IEnumerator<DataItem> GetEnumerator()
    {
        return node.GetEnumerator();
    }
    public bool Save(string filename)           
    {
        FileStream fs = null;
        try
        {
            fs = new FileStream(filename, FileMode.OpenOrCreate);
            BinaryWriter writer = new BinaryWriter(fs);
            writer.Write(str);
            writer.Write(date.Year);
            writer.Write(date.Month);
            writer.Write(date.Day);
            writer.Write(date.Hour);
            writer.Write(date.Minute);
            writer.Write(date.Second);
            writer.Write(MaxMagnitude);
            writer.Write(grid.a);
            writer.Write(grid.b);
            writer.Write(grid.count);
            for (int i = 0; i < array.Count(); i++){
                writer.Write(array[i].Real);
                writer.Write(array[i].Imaginary);
            }
            writer.Close(); 
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        finally
        {
            if (fs != null){
                fs.Close();
            }
        }
    }
    public static bool Load(string filename, ref V1DataUGrid v1)
    {
        FileStream fs = null;
        try
        {
            fs = new FileStream(filename, FileMode.Open);
            BinaryReader reader = new BinaryReader(fs);
            v1.str = reader.ReadString();
            v1.date = new DateTime(reader.ReadInt32(),
                                reader.ReadInt32(),
                                reader.ReadInt32(),
                                reader.ReadInt32(),
                                reader.ReadInt32(),
                                reader.ReadInt32());
            v1.MaxMagnitude = reader.ReadDouble();
            v1.grid = new UniformGrid(reader.ReadDouble(),
                                            reader.ReadDouble(),
                                            reader.ReadInt32());
            v1.array = new Complex[v1.grid.count];
            for (int i = 0; i < v1.grid.count; i++){
                double real = reader.ReadDouble();
                double image = reader.ReadDouble();
                v1.array[i] = new Complex(real, image);
            }
            reader.Close();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        finally
        {
            if (fs != null){
                fs.Close();
            }
        }
    }
}

public class V1DataNUGrid : V1Data
{
    public List<DataItem> node = new List<DataItem>();
    public double[] mas { get; set; }
    public Complex[] array { get; set; }
    public V1DataNUGrid(string s, DateTime dt) : base(s, dt)
    {
        mas = new double[0];
        array = new Complex[0];
    }
    public V1DataNUGrid(string s, DateTime dt, double[] grid1, FComplex F) : base(s, dt)
    {
        mas = new double[grid1.Length];
        array = new Complex[grid1.Length];
        for (int i = 0; i < mas.Length; i++)
        {
            mas[i] = grid1[i];
            array[i] = F(mas[i]);
            node.Add(new DataItem(mas[i], array[i]));
        }
    }
    public override double MaxMagnitude
    {
        get
        {
            Complex maxim = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Magnitude > maxim.Magnitude)
                {
                    maxim = array[i];
                }
            }
            return maxim.Magnitude;
        }
    }
    public override string ToString()
    {
        string result = System.ComponentModel.TypeDescriptor.GetClassName(this);
        result += " " + str + " " + date.ToString() + " " + string.Format("{0:F2}", MaxMagnitude);
        return result;
    }
    public override string ToLongString(string format)
    {
        //string result = str + " " + date.ToString() + " " + string.Format("{0:F2}", MaxMagnitude) + "\n";
        string result = ToString();
        result += "\n";
        for (int i = 0; i < array.Length; i++)
        {
            result += i.ToString() + ": point = " + string.Format(format, mas[i]) + " with value = ";
            result += string.Format(format, array[i]) + "\n";
        }
        return result;

    }
    public override IEnumerator<DataItem> GetEnumerator()
    {
        return node.GetEnumerator();
    }
}

public class V1DataCollection : System.Collections.ObjectModel.ObservableCollection<V1Data>
{
    public V1Data this[int i]
    {
        get => Items[i];
    }
    public double AverageLINQ {
        get{
            var query = from item in Items
                        from elem in item
                        select elem;
            if (query.Count() == 0){
                return Double.NaN;
            }
            double res = query.Average(q => q.value.Magnitude);
            return res;
        }
    }
    public object devLINQ{
        get{
            var query = from item in Items
                        from elem in item
                        select elem;
            if (query.Count() == 0){
                return null;
            }
            double avg = AverageLINQ;
            double max = query.Max(q => (q.value.Magnitude - avg));
            var ans = from item in Items
                      from elem in item
                      where elem.value.Magnitude == max + avg
                      select elem;
            return ans.First();
        }
    }
    public IEnumerable<double> repeatLINQ{
        get{
            var query = from item in Items
                        from elem in item
                        select elem;
            if (query.Count() == 0){
                return null;
            }
            var res = from item1 in Items
                      from item2 in Items
                      from elem1 in item1
                      from elem2 in item2
                      where item1.str != item2.str
                      where elem1.x == elem2.x
                      select elem1.x;
            return res.Distinct();
        }
    }
    public bool Contains(string ID)
    {
        for(int i = 0; i < this.Count; i++)
        {
            if(this[i].str == ID)
            {
                return true;
            }
        }
        return false;
    }
    public bool Remove(string ID)
    {
        if(!Contains(ID))
        {
            return false;
        }
        for(int i = 0; i < this.Count; i++)
        {
            if(this[i].str == ID)
            {
                this.RemoveAt(i);
            }
        }
        return true;
    }
    public bool Add(V1Data v1Data)
    {
        if(Contains(v1Data.str))
        {
            return false;
        }
        this.Insert(0, v1Data);
        return true;
    }
    public void AddDefaults()
    {
        V1DataList ListDef = new V1DataList("-List-", DateTime.Now);
        ListDef.AddDefaults(2, ToComplex.func);
        UniformGrid Grid = new UniformGrid(-3, 18, 2);
        V1DataUGrid UGridDef = new V1DataUGrid("-UniformGrid-", DateTime.Now, Grid, ToComplex.func);
        double[] NUGrid = { -3.0, 11.0 };
        V1DataNUGrid NUGridDef = new V1DataNUGrid("-NotUniformGrid-", DateTime.Now, NUGrid, ToComplex.func);
        Add(ListDef);
        Add(UGridDef);
        Add(NUGridDef);
    }
    public string ToLongString(string format)
    {
        string result = "";
        for(int i = 0; i < this.Count; i++)
        {
            result += this[i].ToLongString(format);
            result += '\n';
        }
        return result;
    }
    public override string ToString()
    {
        string result = "";
        for(int i = 0; i < Count; i++)
        {
            result += this[i].ToString();
        } 
        return result;
    }
}