using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAttribute : MonoBehaviour {
    public bool isObstacle = false;
    private int countLenth = -1;
    private Pair parentUV;
    public bool isUnit = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int CountLenth
    {
        get{ return countLenth; }
        set{ countLenth = value; }
    }

    public Pair ParentUV
    {
        get { return parentUV; }
        set { parentUV = value; }
    }
}
