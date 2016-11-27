using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.VR;
using System.Runtime.InteropServices.ComTypes;

public class Element : MonoBehaviour {

    public const string TAG_CPU = "CPU";
    public const string TAG_GPU = "GPU";
    public const string TAG_DISPLAY = "Display";
    public const string TAG_BATTERY = "Battery";
    public const string TAG_RAM = "RAM";

    public const int TYPE_CPU = 0x00;
    public const int TYPE_GPU = 0x01;
    public const int TYPE_DISPLAY = 0x02;
    public const int TYPE_BATTERY = 0x03;
    public const int TYPE_RAM = 0x04;

    public Sprite damage;

    public bool isStatic = true;

    public class Position {
        public int x;
        public int y;

        public Position(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }
   
    private Position pos;

    public LayerMask blockLayer;

    public float inverseMoveTime = 5.0f;

    private bool marked;

    void Awake() {
        pos = new Position(-1, -1);
    }

    public bool Marked
    {
        get
        {
            return marked;
        }
        set
        {
            this.marked = value;
        }
    }

    public Position Pos {
        get {
            return pos;
        } 
        set{ 
            this.pos = value;
        }
    }
        
    //销毁这个元素
    public IEnumerator DestroySelf() {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = damage;
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
        
    //此元素是否处于静止状态
    public bool IsStatic {
        get{
            return isStatic;
        }
        set { 
            isStatic = value;
        }
    }

    //监听鼠标点击事件
    void OnMouseDown() {
        
        if (GameArea.instance.AnalyzeForCurrent())
            return;

        if (GameArea.instance.First == null)
        {
            GameArea.instance.First = this;
        }
        else
        {
            GameArea.instance.Second = this;

        }

        Debug.Log("hit me " + gameObject.tag + "; " + "X = " + pos.x + ", Y = " + pos.y);
    }

    //调用此方法来播放下落的动画，注意，必须先设置好此元素的目的地坐标才能调用这个方法
    public void Fall() {
        Vector3 end = new Vector3(pos.x , pos.y, .0f);
        StartCoroutine(SmoothMove(end, false));
    } 

    //平滑的移动这个元素
    public IEnumerator SmoothMove(Vector3 end, bool reverse) {
        isStatic = false;
        Vector3 start = transform.position;
        float sqrRemainingDistance = (start - end).sqrMagnitude;

        float v = .0f;
        float gravity = 20f;
        while(sqrRemainingDistance > float.Epsilon) {
            v += gravity * Time.deltaTime;
            Vector3 newPosition = Vector3.MoveTowards(transform.position, end, v * Time.deltaTime + 0.5f * gravity * Time.deltaTime * Time.deltaTime);
            transform.position = newPosition;
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }

        transform.position = end;

        if (reverse)
        {
            StartCoroutine(SmoothMove(start, false));
        }
        else
        {
            isStatic = true;
        }

    }
        
}

