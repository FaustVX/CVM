using System.Threading.Channels;
using CVM;

// https://www.quantamagazine.org/computer-scientists-invent-an-efficient-new-way-to-count-20240516/

static class Program
{
    private static async Task Main()
    {
        Console.WriteLine(Run(Iterative, Generator()));
        Console.WriteLine(await Run(ChannelAsync, Generator()));
        Console.WriteLine(Run(Enumerable, Generator()));

        static int Iterative<T>(CVM<T> cvm, IEnumerable<T> rng)
        {
            foreach (var item in rng)
                cvm.Process(item);

            return cvm.Count;
        }

        static async Task<int> ChannelAsync<T>(CVM<T> cvm, IEnumerable<T> rng)
        {
            var _channel = Channel.CreateBounded<T>(new BoundedChannelOptions(1)
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
                foreach (var item in rng)
                    await _channel.Writer.WriteAsync(item);
                _channel.Writer.Complete();
            }
        }

        static int Enumerable<T>(CVM<T> cvm, IEnumerable<T> rng)
        {
            cvm.Process(rng);
            return cvm.Count;
        }

        static IEnumerable<int> Generator()
        => new Random(0)
            .Generate(static rng => rng.Next(1_000_000))
            .Take(1_000_000);

        static TResult Run<TItem, TResult>(Func<CVM<TItem>, IEnumerable<TItem>, TResult> process, IEnumerable<TItem> generator)
        => process(new(1_000, new(0)), generator);
    }
}
