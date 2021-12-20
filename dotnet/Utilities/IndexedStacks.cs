using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Utilities;

public interface IIndexedValue
{
    public int? IndexedValue { get; }
}

public class IndexedStacks<TType> where TType : IIndexedValue
{
    private readonly SortedDictionary<int, Stack<TType>> _stacks = new(new ReverseComparer<int>());
    private int _currentIndex;
    
    public IndexedStacks(IEnumerable<TType> items)
    {
        items.GroupBy(item => item.IndexedValue).ToList().ForEach(g =>
        {
            var index = g.Key ?? int.MaxValue;
            _stacks.Add(index, new Stack<TType>());
            g.ToList().ForEach(_stacks[index].Push);
        });
    }

    public void Add(TType item, int value)
    {
        _stacks.TryAdd(value, new Stack<TType>());
        _stacks[value].Push(item);
    }

    public TType Pop()
    {
        try
        {
            return _stacks[_stacks.Keys.Last()].Pop();
        }
        catch (InvalidOperationException _)
        {
            _stacks.Remove(_stacks.Keys.Last());
            return Pop();
        }
    }
    
    public class ReverseComparer<T> : IComparer<T> where T : IComparable<T>
    {
        public int Compare(T obj1, T obj2)
        {
            return -(obj1.CompareTo(obj2));
        }
    }
}