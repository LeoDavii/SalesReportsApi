namespace SalesReports.Domain.Services;
public class MedianCalculatorService : IMedianCalculatorService
{
    private readonly PriorityQueue<decimal, decimal> _maxHeap;
    private readonly PriorityQueue<decimal, decimal> _minHeap;

    private long _count = 0;
    public long Count => _count;

    public MedianCalculatorService()
    {
        _maxHeap = new PriorityQueue<decimal, decimal>();
        _minHeap = new PriorityQueue<decimal, decimal>();
    }

    public void AddValue(decimal value)
    {
        _count++;

        if (_maxHeap.Count == 0)
        {
            _maxHeap.Enqueue(value, -value);
            return;
        }

        if (value <= _maxHeap.Peek())
        {
            _maxHeap.Enqueue(value, -value);
        }
        else
        {
            _minHeap.Enqueue(value, value);
        }

        RebalanceHeaps();
    }

    private void RebalanceHeaps()
    {
        if (_maxHeap.Count > _minHeap.Count + 1)
        {
            var value = _maxHeap.Dequeue();
            _minHeap.Enqueue(value, value);
        }
        else if (_minHeap.Count > _maxHeap.Count)
        {
            var value = _minHeap.Dequeue();
            _maxHeap.Enqueue(value, -value);
        }
    }

    public decimal? GetMedian()
    {
        if (_count == 0) return null;

        if (_maxHeap.Count > _minHeap.Count)
        {
            return _maxHeap.Peek();
        }
        else
        {
            return (_maxHeap.Peek() + _minHeap.Peek()) / 2;
        }
    }

}

