using UnityEngine;
using System.Collections;
using System.Net.Mail;
using System;
using System.Xml.Linq;
#if UNITY_EDITOR
using UnityEditorInternal;
using UnityEditor.VersionControl;
#endif
using System.Collections.Generic;

public class Analyzer : MonoBehaviour {

   
    public static bool AnalyzeIfSwap(Element[,] map) {

        string[,] tagMap = GetTagMap(map);

        for (int x = 0; x < tagMap.GetLength(0); x++)
        {
            for (int y = 0; y < tagMap.GetLength(1) - 1; y++)
            {

                string temp = tagMap[x, y];
                tagMap[x, y] = tagMap[x, y + 1];
                tagMap[x, y + 1] = temp;

                if (CheckVerticalSwap(tagMap, x, y, y + 1))
                {
                    Debug.Log("if you swap ("+ x + "," + y + ") with (" + x + "," + (y + 1) + "), you can get out" );
                    return true;
                }
                else
                {
                    tagMap[x, y + 1] = tagMap[x, y];
                    tagMap[x, y] = temp;
                }

            }

        }

        for (int y = 0; y < tagMap.GetLength(1); y++)
        {
            for (int x = 0; x < tagMap.GetLength(0) - 1; x++)
            {
                string temp = tagMap[x, y];
                tagMap[x, y] = tagMap[x + 1, y];
                tagMap[x + 1, y] = temp;

                if (CheckHorizontalSwap(tagMap, y, x, x + 1))
                {
                    Debug.Log("if you swap ("+ x + "," + y + ") with (" + (x + 1) + "," + y + "), you can get out" );
                    return true;
                }
                else
                {
                    tagMap[x + 1, y] = tagMap[x, y];
                    tagMap[x, y] = temp;
                }
            }
        }

        return false;
    }

    public static bool AnalyzerForCurrent(Element[,] map) {

        string[,] tagMap = GetTagMap(map);

        for (int x = 0; x < tagMap.GetLength(0); x++)
        {
            if (CheckColumn(tagMap, x)) return true;
        }
        for (int y = 0; y < tagMap.GetLength(1); y++)
        {
            if (CheckRow(tagMap, y))
                return true;
        }
        return false;
    }

    private static bool CheckHorizontalSwap(string[,] tagMap, int y, int x1, int x2) {
        return CheckRow(tagMap, y) || CheckColumn(tagMap, x1) || CheckColumn(tagMap, x2);
    }


    private static bool CheckVerticalSwap(string[,] tagMap, int x, int y1, int y2) {
      
        return CheckRow(tagMap, y1) || CheckRow(tagMap, y2) || CheckColumn(tagMap, x);
            
       
    }

    private static bool CheckColumn(string[,] tagMap, int column) {
        int count = 0;
        string temp = "";
        for (int y = 0; y < tagMap.GetLength(1); y++) {
            if (y == 0)
            {
                temp = tagMap[column, y];
                count++;
            }
            else if (tagMap[column, y].Equals(temp))
            {
                count++;
                if (count >= 3)
                {
                    return true;
                }
            }
            else
            {
                temp = tagMap[column, y];
                count = 1;
            }

        }

            return false;
    }

    private static bool CheckRow(string[,] tagMap, int rowNum) {
        int count = 0;
        string temp = "";
        for (int x = 0; x < tagMap.GetLength(0); x++) {
            if (x == 0)
            {
                temp = tagMap[x, rowNum];
                count++;
            }
            else if (tagMap[x, rowNum].Equals(temp))
            {
                count++;
                if (count >= 3)
                {
                    return true;
                }

                    
            }
            else
            {
                temp = tagMap[x, rowNum];
                count = 1;
            }
                    
                   
        }

        return false;
        
    }

    private static string[,] GetTagMap(Element[,] map) {
       
        string[,] tagMap = new string[map.GetLength(0), map.GetLength(1)];
        for (int x = 0; x < tagMap.GetLength(0); x++)
        {
            for (int y = 0; y < tagMap.GetLength(1); y++)
            {
                tagMap[x, y] = map[x, y].tag;
            }
        }

        return tagMap;
    }

}
