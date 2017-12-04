using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    UnitControler unitControler;
    UnitControler.UnitStates states;
	// Use this for initialization
	void Start () {
        unitControler = gameObject.GetComponent<UnitControler>();
        states = unitControler.State;
        //Debug.Log(states);
    }
	
	// Update is called once per frame
	void Update () {
        /*if (states != unitControler.State)
        {
            states = unitControler.State;
            Debug.Log(states);
        }*/
        Debug.Log(unitControler.State);
    }
}
