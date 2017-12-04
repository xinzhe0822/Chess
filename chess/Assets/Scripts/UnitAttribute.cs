using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class UnitAttribute {
    public enum UnitAscription
    {
        red = 0,
        blue
    }

    public enum AttackType
    {
        melee=0,
        longRange
    }

    private string resourceName;
    private UnitAscription ascription;
    private Pair uv;
    private float higth;

    public int HP;
    public int speed;
    public int moveLenth;

    [SerializeField,HeaderAttribute("AttackType and value")]
    public AttackType attackType;
    public int attackStandard;
    public int attackDeviation;
    public int longRange;
    public int longRangeStandard;
    public int longRangeDeviation;
    public string attackTypeName;

    private float speedCount=0;
    public void AddSpeedCount()
    {
        speedCount += 100 / (float)speed;
    }

    public float SpeedCount
    {
        get { return speedCount; }
    }

    public string ResourceName
    {
        set { resourceName = value; }
        get { return resourceName; }
    }

    public UnitAscription Ascription
    {
        set { ascription = value; }
        get { return ascription; }
    }

    public float Higth
    {
        set { higth = value; }
        get { return higth; }
    }

    public Pair UV
    {
        set { uv = value; }
        get { return uv; }
    }
}
