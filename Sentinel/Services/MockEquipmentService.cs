using Avalonia.Threading;
using Bogus;
using Sentinel.Models;

namespace Sentinel.Services;

public class MockEquipmentService : IEquipmentService, IDisposable
{
    public event EventHandler<TelemetryChangedEventArgs>? TelemetryChanged;
    
    private PeriodicTimer Timer { get; init; } = new(TimeSpan.FromSeconds(2));
    private CancellationTokenSource CancellationTokenSource { get; init; } = new();
    
    private List<EquipmentUnit> Units { get; init; }
    
    public MockEquipmentService()
    {
        var testUnits = new Faker<EquipmentUnit>()
            .CustomInstantiator(f => new EquipmentUnit
            {
                Id = f.Random.Guid(),
                Name = "SENTINEL-" + f.IndexFaker,
                Zone = f.Address.City(),
                Status = f.PickRandom<EquipmentStatus>(),
                Battery = f.Random.Number(0, 100),
                Signal = f.Random.Number(0, 100),
                Temperature = f.Random.Number(min:-273),
                LastSeen = DateTimeOffset.UtcNow
            });

        Units = testUnits.Generate(15);
        
        Task.Run(async () =>
        {
            while(await Timer.WaitForNextTickAsync(CancellationTokenSource.Token).ConfigureAwait(false))
            {
                foreach (var unit in Units)
                {
                    var newUnit = unit with
                    {
                        Battery = Random.Shared.NextDouble(),
                        Signal = Random.Shared.Next(),
                        Temperature = Random.Shared.NextDouble(),
                        LastSeen = DateTimeOffset.UtcNow
                    };
                    
                    Dispatcher.UIThread.Post(() => TelemetryChanged?.Invoke(this, new TelemetryChangedEventArgs(newUnit)));
                }
            }
        });
    }

    public async Task<IReadOnlyList<EquipmentUnit>> GetFleetAsync(CancellationToken cancellationToken = default)
    {
        await Task.Delay(2000, cancellationToken).ConfigureAwait(false);
        return Units;
    }

    public void Dispose()
    {
        CancellationTokenSource.Cancel();
        Timer.Dispose();
    }
}