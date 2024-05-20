using System.Threading.Channels;
using CVM;

// https://www.quantamagazine.org/computer-scientists-invent-an-efficient-new-way-to-count-20240516/

static class Program
{
    private static async Task Main()
    {
        Console.WriteLine(Iterative(new(1_000, new(0))));
        Console.WriteLine(await ChannelAsync(new(1_000, new(0))));
        Console.WriteLine(Enumerable(new(1_000, new(0))));

        static int Iterative(CVM<int> cvm)
        {
            var rng = new Random(0);

            for (var i = 0; i < 1_000_000; i++)
                cvm.Process(rng.Next(1_000_000));

            return cvm.Count;
        }

        static async Task<int> ChannelAsync(CVM<int> cvm)
        {
            var _channel = Channel.CreateBounded<int>(new BoundedChannelOptions(1)
            {
                SingleReader = true,
                SingleWriter = true,
                FullMode = BoundedChannelFullMode.Wait,
            });

            _ = Task.Run(GenerateAsync);
            await cvm.ProcessAsync(_channel);
            return cvm.Count;

            async Task GenerateAsync()
            {
                var rng = new Random(0);

                for (var i = 0; i < 1_000_000; i++)
                    await _channel.Writer.WriteAsync(rng.Next(1_000_000));
                _channel.Writer.Complete();
            }
        }

        static int Enumerable(CVM<int> cvm)
        {
            var rng = new Random(0);

            cvm.Process(rng.Generate(static rng => rng.Next(1_000_000)).Take(1_000_000));

            return cvm.Count;
        }
    }
}
