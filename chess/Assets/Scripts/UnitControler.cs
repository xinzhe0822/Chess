using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControler : MonoBehaviour {
    public enum UnitStates
    {
        wait=0,
        walk,
        attack,
        underAttack,
        die
    }

    public UnitAttribute attribute;
    private UnitStates state = UnitStates.wait;
    private UnitAttribute.UnitAscription ascription;  
    private GridPosition uvPosition;
    private Pair UV;
    private int countLenth;
    private Stack<Pair> path=new Stack<Pair>();
    private Pair currentUV;
    public float moveSpeed = 3.0f;
    public float rotateSpeed = 8.0f;
    private float angle;
    private Quaternion targetRotarion;
    private float[] distance = new float[2];
    private int distanceflag = 0;
    private float currentDistance;
    private Pair attackUV;
    private bool attackFlag=false;
    private bool deadFalg = false;
    private Quaternion tempRotarion;
    private bool meleeFlag = true;
    // Use this for initialization
    void Start() {
        SetByGridPosition();
        UV = currentUV;
        distance[0] = uvPosition.Lenthh * uvPosition.Size;
        distance[1] = uvPosition.Lenthh * Mathf.Sqrt(2) * uvPosition.Size;
    }

    // Update is called once per frame
    void Update() {
        if (state == UnitStates.wait)
        {
            if (attackFlag)
            {
                state = UnitStates.attack;
                attackFlag = false;
                meleeFlag = false;
                tempRotarion = transform.rotation;
                targetRotarion = Quaternion.LookRotation(new GridPosition(attackUV).GetPosition() - transform.position);
            }
        }
        else if (state == UnitStates.walk)
        {
            if (currentUV.Equals(UV))
            {
                if (path.Count == 0)
                {
                    if (attackFlag)
                    {
                        state = UnitStates.attack; 
                        UV = attackUV;
                        angle = GetAngle();
                        targetRotarion = Quaternion.AngleAxis(angle, Vector3.up);
                        attackFlag = false;
                    }
                    else
                        state = UnitStates.wait;
                }
                else
                {
                    UV = path.Pop();
                    angle = GetAngle();
                    distanceflag = Mathf.Abs((int)angle / 45 % 2);
                    currentDistance = 0;
                    targetRotarion = Quaternion.AngleAxis(angle, Vector3.up);
                }
            }
            else
            {
                if (Mathf.Abs(Quaternion.Dot(transform.rotation, targetRotarion)) != 1.0f)  //由于相差180度是相差一周，故用这种方法比较
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotarion, rotateSpeed * Time.deltaTime); 
                }
                else
                {
                    transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                    currentDistance += moveSpeed * Time.deltaTime;
                    if (currentDistance > distance[distanceflag])
                    {
                        SetByGridPosition(); 
                        countLenth--;
                    }
                }
            }
        }
        else if (state == UnitStates.attack)
        {
            if (Mathf.Abs(Quaternion.Dot(transform.rotation, targetRotarion)) != 1.0f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotarion, rotateSpeed * Time.deltaTime);
                if(!meleeFlag&& transform.rotation == tempRotarion)
                {
                    meleeFlag = true;
                    state = UnitStates.wait;
                    countLenth = 0;
                }
            }
            else
            {
                if (meleeFlag)
                {
                    //设置并播放攻击动画

                    //在动画播放中，当要攻击到时发送信息，以便对方响应
                    GameObject.Find("Game Manager").SendMessage("ToAttackUnit");

                    state = UnitStates.wait;
                    countLenth = 0;
                    UV = currentUV;
                }
                else
                {
                    //远程攻击动画

                    //在动画播放中，当要攻击到时发送信息，以便对方响应
                    GameObject.Find("Game Manager").SendMessage("ToAttackUnit");

                    targetRotarion = tempRotarion;
                }
            }
        }
        else if (state == UnitStates.underAttack)
        {
            //播放被攻击动画
            
            if (deadFalg) 
                state = UnitStates.die;
            else
                state = UnitStates.wait;
        }
        else if (state == UnitStates.die)
        {
            //播放死亡动画

            GetComponent<MeshRenderer>().material.color = Color.black;
            state = UnitStates.wait;
        }
    }

    private float GetAngle()
    {
        GridPosition gridPosition = new GridPosition(currentUV);
        float angle = 0.0f;
        if (UV.Equals(gridPosition.N))
            angle = 0.0f;
        else if (UV.Equals(gridPosition.NE))
            angle = 45.0f;
        else if (UV.Equals(gridPosition.E))
            angle = 90.0f;
        else if (UV.Equals(gridPosition.SE))
            angle = 135.0f;
        else if (UV.Equals(gridPosition.S))
            angle = -180.0f;
        else if (UV.Equals(gridPosition.SW))
            angle = -135.0f;
        else if (UV.Equals(gridPosition.W))
            angle = -90.0f;
        else if (UV.Equals(gridPosition.NW))
            angle = -45.0f;
        return angle;
    }

    public void SetByGridPosition()
    {
        uvPosition = new GridPosition(transform.position);
        transform.position = uvPosition.GetPosition();
        currentUV = new Pair(uvPosition.U, uvPosition.V);
    }

    public void SetByUV()
    {
        uvPosition = new GridPosition(UV);
        transform.position = uvPosition.GetPosition();
        currentUV = new Pair(uvPosition.U, uvPosition.V);
    }

    public void SetUV(Pair p)
    {
        UV = p;
    }

    public Pair GetUV() {
        return currentUV;   //攻击时UV为目标UV，所以返回这个
    }

    public void MoveUnit(Pair[] p)
    {
        if (state == UnitStates.wait)
        {
            state = UnitStates.walk;
            foreach(Pair pair in p)
            {
                path.Push(pair);
            }
        }      
    }

    public void exceptMelee()
    {
        GameObject.Find("Game Manager").SendMessage(attribute.attackTypeName);   //发送相应的远程攻击函数名称
    }

    public void AttackUnit(Pair p)
    {
        if (state != UnitStates.attack)
        {
            attackUV = p;
            attackFlag = true;
        }
    }

    public void UnderAttack(int damage,bool deadFalg)
    {
        if (state == UnitStates.wait)
        {
            state = UnitStates.underAttack;
            attribute.HP -= damage;
            this.deadFalg = deadFalg;
        }
    }

    public void SetCountLenth()
    {
        countLenth = attribute.moveLenth;
    }

    public int CountLenth
    {
        get { return countLenth; }
    }

    public UnitStates State
    {
        get { return state; }
    }

    public UnitAttribute.UnitAscription Ascription
    {
        set { ascription = value; }
        get { return ascription; }
    }

    private int HP_BAR_WIDTH = 64;
    private int HP_BAR_HEIGHT = 16;
    void OnGUI()
    {   //TODO: Get rid of magic numbers.
        Vector3 coordinates = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0) + 0.5f * Camera.main.transform.up);   //TODO: Make this some kind of constant.
        coordinates.y = Screen.height - coordinates.y;
        //print (coordinates);
        Texture2D red = new Texture2D(1, 1);
        red.SetPixel(0, 0, Color.red);
        red.wrapMode = TextureWrapMode.Repeat;
        red.Apply();
        Texture2D green = new Texture2D(1, 1);
        green.SetPixel(0, 0, Color.green);
        green.wrapMode = TextureWrapMode.Repeat;
        green.Apply();
        //GUI.Box (new Rect(coordinates.x - 10, coordinates.y - 5, 20, 10), "test");
        GUI.DrawTexture(new Rect(coordinates.x - HP_BAR_WIDTH / 2, coordinates.y + HP_BAR_HEIGHT / 2, HP_BAR_WIDTH, HP_BAR_HEIGHT), red);
        GUI.DrawTexture(new Rect(coordinates.x - HP_BAR_WIDTH / 2, coordinates.y + HP_BAR_HEIGHT / 2, HP_BAR_WIDTH * attribute.HP / 10, HP_BAR_HEIGHT), green);
        GUIStyle centered = new GUIStyle();
        centered.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(coordinates.x - HP_BAR_WIDTH / 2, coordinates.y + HP_BAR_HEIGHT / 2, HP_BAR_WIDTH, HP_BAR_HEIGHT), attribute.HP.ToString(), centered);
    }
}
