using CVM;

var cvm = new CVM<int>(100);
var rng = new Random(0);
while (true)
{
    cvm.Process(rng.Next());
}
