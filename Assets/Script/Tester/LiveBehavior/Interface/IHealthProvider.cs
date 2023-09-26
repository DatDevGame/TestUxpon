using UniRx;
public interface IHealthProvider
{
    public ReactiveProperty<int> Health { get; }
}
