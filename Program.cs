using System.Threading.Channels;
using CVM;

// https://www.quantamagazine.org/computer-scientists-invent-an-efficient-new-way-to-count-20240516/

static class Program
{
    private static readonly Channel<int> _channel = Channel.CreateBounded<int>(new BoundedChannelOptions(1)
    {
        SingleReader = true,
        SingleWriter = true,
        FullMode = BoundedChannelFullMode.Wait,
    });

    private static async Task Main()
    {
        var cvm = new CVM<int>(1_000, new(0));

        _ = Task.Run(GenerateAsync);

        await cvm.ProcessAsync(_channel.Reader);

        Console.WriteLine(cvm.Count);

        cvm = new CVM<int>(1_000, new(0));
        var rng = new Random(0);

        for (var i = 0; i < 1_000_000; i++)
            cvm.Process(rng.Next(1_000_000));

        Console.WriteLine(cvm.Count);
    }

    private static async Task GenerateAsync()
    {
        var rng = new Random(0);

        for (var i = 0; i < 1_000_000; i++)
            await _channel.Writer.WriteAsync(rng.Next(1_000_000));
        _channel.Writer.Complete();
    }
}
