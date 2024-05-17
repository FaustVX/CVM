using CVM;

// https://www.quantamagazine.org/computer-scientists-invent-an-efficient-new-way-to-count-20240516/
var cvm = new CVM<int>(1_000);
var rng = new Random(0);
for (var i = 0; i < 1_000_000; i++)
{
    cvm.Process(rng.Next(100_000));
}
Console.WriteLine(cvm.Count);
