public abstract class Hookable<T>
{
    public T Data
    {
        get
        {
            return GetData();
        }
    }
    public Hookable<T> DoHandle()
    {
        Exec();
        return this;
    }
    
    protected abstract T GetData();
    protected abstract Hookable<T> Exec();
}

public class Sequence<T>
{
    public Sequence(T data)
    {
        this.Data = data;
    }
    public Sequence(bool isRootNode)
    {
        if (isRootNode)
        {
            this.Previous = null;
            this.Next = null;
            this.Stale = true;
            this.Data = default(T);
        }
        else
        {
            throw new InvalidOperationException();
        }
        
    }
    public static Sequence<T> CreateRootNode()
    {
        return new Sequence<T>(isRootNode: true);
    }
    public T Data { get; set; }
    
    public bool Stale { get; set; } = false;
    public Sequence<T> Next { get; set; }
    public Sequence<T> Previous { get; set; } = null; //Only null should be the origin
    public Sequence<T> GetOrigin()
    {
        if (Previous == null)
            return this;
        return GetOriginRecur(this.Previous);
    }
    
    protected Sequence<T> GetOriginRecur(Sequence<T> current)
    {
        if (current.Previous == null)
            return current;
        return GetOriginRecur(current.Previous);
    }
    public Sequence<T> GetLast(Sequence<T> nextSequence)
    {
        if (nextSequence.Next == null)
            return nextSequence;
        return GetLast(this.Next);
    }
    public Sequence<T> PushNext(Sequence<T> newSequence)
    {
        newSequence.Previous = this;
        newSequence.Next = null;
        return newSequence;
    }
}
