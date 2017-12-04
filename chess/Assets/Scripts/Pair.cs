using System;

public class Pair
{
	private int first;
	private int second;

	public Pair(int first, int second)
	{
		this.first = first;
		this.second = second;
	}

	public Pair(Pair p){
		first = p.First;
		second = p.Second;
	}

    public Pair(GridPosition p)
    {
        first = p.U;
        second = p.V;
    }

	public int First { get { return this.first; } }

	public int Second { get { return this.second; } }
    //重写Equals和GetHashCode，否则Dictionary无法读出key
    public override bool Equals(object obj)
    {
        if (obj == this)
            return true;
        if (!(obj is Pair))
            return false;

        var pair = (Pair)obj;

        return first == pair.First && second == pair.Second;
    }

    public override int GetHashCode()
    {
        return first ^ second;
    }

    public int GetDistance(Pair p)
    {
        int u = Math.Abs(first - p.first);
        int v = Math.Abs(second - p.second);
        return u>v?u+1:v+1;
    }

}
