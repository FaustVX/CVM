using System.Threading.Channels;
using CVM;
using static ConsoleMenu.Helpers;

// https://www.quantamagazine.org/computer-scientists-invent-an-efficient-new-way-to-count-20240516/

static class Program
{
    private static async Task Main()
    {
        while (true)
        {
            Console.WriteLine(await Menu<Task<int>>("Select Method :",
                (nameof(Iterative), static () => Task.FromResult(Run(Iterative, Generator()))),
                (nameof(ChannelAsync), static async () => await Run(ChannelAsync, Generator())),
                (nameof(Enumerable), static () => Task.FromResult(Run(Enumerable, Generator())))));
            Console.ReadKey();
        }

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
