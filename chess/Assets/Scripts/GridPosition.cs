using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPosition{
    public enum NeighbourGrid
    {
        M=0,
        N,
        NE,
        E,
        SE,
        S,
        SW,
        W,
        NW
    }

    private int u;
    private int v;
    private float lenth = 1;
    private float size = 1;                    //缩放大小
    private int chessWidth = 10;
    private int chessHigth = 10;
    private float higth = 0.2f;

    public GridPosition()
    {
        SetChessbord();
        u = 0;
        v = 0;
    }

    public GridPosition(int u,int v)
    {
        SetChessbord();
        this.u = u;
        this.v = v;
    }

    public GridPosition(Pair pair)
    {
        SetChessbord();
        u = pair.First;
        v = pair.Second;
    }

    public GridPosition(Pair pair,float higth)
    {
        SetChessbord();
        u = pair.First;
        v = pair.Second;
        this.higth = higth;
    }

    public GridPosition(Vector3 pos)         //全局点转换为uv点
    {
        SetChessbord();
        u = Mathf.FloorToInt(pos.x / lenth / size)+chessWidth/2;
        v = Mathf.FloorToInt(pos.z / lenth / size)+chessHigth/2;
        higth = pos.y;
    }

    public Vector3 GetPosition()
    {
        return new Vector3((u-chessWidth/2+0.5f)*lenth*size,higth,(v-chessHigth/2+0.5f)*lenth*size);
    }

    public void SetChessbord()
    {
        GameManager g = GameObject.Find("Game Manager").GetComponent<GameManager>();
        chessHigth = g.chessHigth;
        chessWidth = g.chessWidth;
        lenth = g.lenth;
        size = g.size;
    }

    public Pair N
    {
        get { return new Pair(u, v + 1); }
    }

    public Pair NE
    {
        get { return new Pair(u + 1, v + 1); }
    }

    public Pair E
    {
        get { return new Pair(u + 1, v); }
    }

    public Pair SE
    {
        get { return new Pair(u + 1, v - 1); }
    }

    public Pair S
    {
        get { return new Pair(u, v - 1); }
    }

    public Pair SW
    {
        get { return new Pair(u - 1, v - 1); }
    }

    public Pair W
    {
        get { return new Pair(u - 1, v); }
    }

    public Pair NW
    {
        get { return new Pair(u - 1, v + 1); }
    }

    public Pair GetNeighbour(NeighbourGrid neighbour)
    {
        switch (neighbour)
        {
            case NeighbourGrid.N:
                return N;
            case NeighbourGrid.NE:
                return NE;
            case NeighbourGrid.E:
                return E;
            case NeighbourGrid.SE:
                return SE;
            case NeighbourGrid.S:
                return S;
            case NeighbourGrid.SW:
                return SW;
            case NeighbourGrid.W:
                return W;
            case NeighbourGrid.NW:
                return NW;
            default:
                return new Pair(u, v);
        }
    }

    public int U {
		get {
			return this.u;
		}
		set {
			u = value;
		}
	}

	public int V {
		get {
			return this.v;
		}
		set {
			v = value;
		}
	}

    public float Lenthh
    {
        get { return lenth; }
    }

    public float Size
    {
        get { return size; }
    }

    public override bool Equals(object obj)
    {
        if (obj == this)
            return true;
        if (!(obj is GridPosition))
            return false;

        var p = (GridPosition)obj;

        return u == p.U && v == p.V;
    }

    public override int GetHashCode()
    {
        var hashCode = -1237110951;
        hashCode = hashCode * -1521134295 + u.GetHashCode();
        hashCode = hashCode * -1521134295 + v.GetHashCode();
        return hashCode;
    }
}
