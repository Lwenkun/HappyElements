using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Security.Cryptography;
using UnityEngine.UI;
using System.Net.Mail;
using UnityEngineInternal;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
#endif
using System.Text;
using System.Linq.Expressions;
using System.Security.Policy;

using UnityEngine.Rendering;

public class GameArea : MonoBehaviour {

    private Element first;
    private Element second;

    public GameObject selector;

    private const int CHECK_VERTICAL = 0x00;
    private const int CHECK_HORIZONTAL = 0x01;

    public static GameArea instance;

    private Element[,] map;

    public GameObject[] elements;

    private const int areaSize = 9;

    private int[] elementsCountInColumn;

    //每一轮各元素消除的个数
    private int[] numOfEli;

    void Awake() {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        numOfEli = new int[5];

        //data structure of our game object
        map = new Element[areaSize, areaSize];

        //used to record element num in every column
        elementsCountInColumn = new int[9];

        //instantiate the seletors
        selector = (GameObject) Instantiate(selector, new Vector3(0f, 0f, 0f), Quaternion.identity);
        MoveSelector(new Vector3(-2, -2), false);
      
        for (int i = 0; i < elementsCountInColumn.Length; i++)
        {
            elementsCountInColumn[i] = 9;
        }

        LayoutElements();
    }

    public Element First {
        get { 
            return first;
        }
        set{ 
            first = value;
        }
    }

    public Element Second
    {
        get
        { 
            return second;

        }
        set
        {
            second = value;
        }
    }


    //check if this two elements are neighboured with each other
    public bool CheckForSwapingEarly(Element first, Element second) {

        if (first == null || second == null)
            return false;

        if ((first.Pos.x == second.Pos.x && System.Math.Abs(first.Pos.y - second.Pos.y) == 1)
            || (first.Pos.y == second.Pos.y && System.Math.Abs(first.Pos.x - second.Pos.x) == 1))
        {
            return true;
        }

        return false;

    }

    public void LayoutElements() {
        for (int i = 0; i < areaSize; i++)
        {
            for (int j = 0; j < areaSize; j++)
            {
                Vector3 position = new Vector3(i, j, 0f);
                GameObject randomElement = RandomElement();
                Element element = LayoutElement(position, randomElement);
                setMapPosition(element, i, j);
            }
        }
    }

    public void MoveSelector(Vector3 target, bool isActive) {
        selector.SetActive(isActive);
        selector.transform.position = target;
    }

    public bool AnalyzeIfSwap() {
        return Analyzer.AnalyzeIfSwap(map);
    }

    public bool AnalyzeForCurrent() {
        return Analyzer.AnalyzerForCurrent(map);
    }

    public void setMapPosition(Element element, int x, int y) {
        map[x, y] = element;
        map[x, y].Pos = new Element.Position(x, y);
    }

         
    private Element LayoutElement(Vector2 position, GameObject prefab) {
        GameObject instance = (GameObject)Instantiate(prefab, position, Quaternion.identity);
        return instance.GetComponent<Element>();
       
    }

    private GameObject RandomElement() {
        int index = Random.Range(0, elements.Length);
        return elements[index];
    }


    public void Check() {
        CheckByLine(CHECK_VERTICAL);
        CheckByLine(CHECK_HORIZONTAL);
    }

    private bool IsFinish() {
        return isStatic();
    }


    public void Adjust() {
        int emptyCount = 0;
        //清空上一次的计数
        for (int i = 0; i < 5; i++)
        {
            numOfEli[i] = 0;
        }
        for (int i = 0; i < areaSize; i++) {
            emptyCount = 0;
            for (int j = 0; j < elementsCountInColumn[i]; j++) {

                if (map[i, j].Marked == true)
                {

                    if(map [i, j].tag == Element.TAG_CPU)
                        numOfEli[Element.TYPE_CPU] ++;

                    if(map [i, j].tag == Element.TAG_GPU)
                        numOfEli[Element.TYPE_GPU] ++;

                    if(map [i, j].tag == Element.TAG_DISPLAY)
                        numOfEli[Element.TYPE_DISPLAY] ++;

                    if(map [i, j].tag == Element.TAG_BATTERY)
                        numOfEli[Element.TYPE_BATTERY] ++;

                    if(map [i, j].tag == Element.TAG_RAM)
                        numOfEli[Element.TYPE_RAM] ++;
                    
                    emptyCount++;
                    DestroyElement(map[i, j]);
                    map[i, j] = null;
                }
                else if (emptyCount > 0)
                {
                    setMapPosition(map[i, j], i, j - emptyCount);
                }
                  
            }

            elementsCountInColumn[i] -= emptyCount;

        }
    }

    private void DestroyElement(Element element) {
        StartCoroutine(element.DestroySelf());
    }

    public int[] GetEliNumList() {
        return (int[])numOfEli.Clone();
    }

    private int getElementsCountAtColumn(int columnIndex) {
        if (columnIndex > 0 || columnIndex < areaSize)
        {
            return elementsCountInColumn[columnIndex];
        }

        return -1;
    }

    public void Fall() {

        for (int i = 0; i < areaSize; i++)
        {
            for (int j = 0; j < areaSize; j++)
            {
                if (map[i, j] != null && map[i, j].gameObject.activeSelf)
                {
                    map[i, j].Fall();
                }
            }
        }

        OnFallFinished();

    }

    public void StartSwapAnimation(Element first, Element second, bool reverse) {
        
        Vector3 firstPosition = first.transform.position;
        Vector3 secondPostion = second.transform.position;

        StartCoroutine(first.SmoothMove(secondPostion, reverse));
        StartCoroutine(second.SmoothMove(firstPosition, reverse));
    }
        

    public void SwapMapPosition(Element first, Element second) 
    {
       
        int x = first.Pos.x;
        int y = first.Pos.y;
        first.Pos.x = second.Pos.x;
        first.Pos.y = second.Pos.y;
        second.Pos.x = x;
        second.Pos.y = y;

        map[first.Pos.x, first.Pos.y] = first;
        map[second.Pos.x, second.Pos.y] = second;
    }

    private void OnFallFinished() {
        ReFill();
    }


    private void ReFill() {
        
        for (int i = 0; i < 9; i++)
        {

            Debug.Log(getElementsCountAtColumn(i));

            int extraHeight = 0;

            while (getElementsCountAtColumn(i) < areaSize)
            {

                Vector3 position = new Vector3(i, areaSize + extraHeight, 0f);

                GameObject randomElement = RandomElement();

                Element element = LayoutElement(position, randomElement);

                setMapPosition(element, i, getElementsCountAtColumn(i));
               
                element.Marked = false;
                element.Fall();
                elementsCountInColumn[i]++;
                extraHeight++;
            }

        }

        Debug.Log((int)selector.transform.position.x + "," + (int)selector.transform.position.y);

    }

    public bool isStatic() {
        for (int x = 0; x < areaSize; x++)
        {
            for (int y = 0; y < areaSize; y++)
            {
                if (!map[x, y].IsStatic)
                    return false;
            }

        }

        return true;
    }

    //check either vertically or horizontally
    private void CheckByLine(int orientation) {

        for (int dimension1 = 0; dimension1 < areaSize; dimension1++)
        {
            int start = 0;
            int count = 0;
            bool check = false;
            string startTag = "";
            //if orientation is vertical, we just check dimension2 from 0 to count of elements at specific column
            int dimension2Limitation = orientation == CHECK_VERTICAL ? elementsCountInColumn[dimension1]
                : areaSize;
            for (int dimension2 = 0; dimension2 < dimension2Limitation; dimension2++)
            {
                Element current = null;
                if (orientation == CHECK_VERTICAL) {
                    //in this case, dimension1 represents x, dimension2 represents y
                    current = map[dimension1, dimension2];
                } else if (orientation == CHECK_HORIZONTAL) {
                    //in this case, dimension2 represents x, dimension1 represents y;
                    current = map[dimension2, dimension1];
                }
                if (current == null || current.gameObject.activeSelf == false)
                {
                    check = true; 
                }
                else
                {
                    if (startTag.Equals(""))
                    {
                        startTag = current.tag;
                    }

                    if (current.CompareTag(startTag))
                    {
                        count++;
                    }
                    else
                    {
                        check = true;
                    }
                }

                if ((orientation == CHECK_VERTICAL && dimension2 == elementsCountInColumn[dimension1] - 1)
                    || (orientation == CHECK_HORIZONTAL && dimension2 == areaSize - 1))
                    check = true;

                if (check)
                {
                    if (count >= 3)
                    {
                        int index;
                        for (index = start; index < start + count; index++)
                        {
                            Element toBeDestroy = null;
                            if (orientation == CHECK_VERTICAL)
                            {
                                toBeDestroy = map[dimension1, index];
                            } else if (orientation == CHECK_HORIZONTAL) {
                                toBeDestroy = map[index, dimension1];
                            }
                            if (toBeDestroy != null)
                            {
                                toBeDestroy.Marked = true;
                            }

                        }

                    }

                    if (current != null)
                    {
                        startTag = current.tag;
                        count = 1;
                        start = dimension2;
                    }
                    else
                    {
                        startTag = "";
                        count = 0;
                        start = dimension2 + 1;
                    }

                    check = false;

                }

            }
        }
    }


}