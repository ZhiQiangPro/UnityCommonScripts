/************************************************************
  Copyright (C), 2007-2017,BJ Rainier Tech. Co., Ltd.
  FileName: DrawTest.cs
  Author:       Version :1.0          Date: 
  Description:
************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public class DrawTest : MonoBehaviour
{
    private Transform beiJing;
    private Camera UIcamera;
    [SerializeField] private List<Vector2> pointList = new List<Vector2>();
    [SerializeField] private List<int> breakpointList = new List<int>();



    private List<Button> btnList = new List<Button>();

    private void Awake()
    {
        beiJing = transform.Find("背景");
        UIcamera = transform.Find("Camera").GetComponent<Camera>();
    }

    private void Start()
    {
        for (int i = 0; i < beiJing.childCount; i++)
        {
            var i1 = i;
            beiJing.GetChild(i).GetComponent<Button>().OnClickAsObservable().Subscribe(_ =>
            {
                if (!MyDrawGLine.canDraw)
                {
                    pointList.Add(UIcamera.WorldToScreenPoint(beiJing.GetChild(i1).position));
                              
                }

                MyDrawGLine.canDraw = true;
                pointList.Add(UIcamera.WorldToScreenPoint(beiJing.GetChild(i1).position));

                MyDrawGLine.PointList = pointList;
            });
        }


        this.UpdateAsObservable().Select(_ => Input.GetMouseButtonDown(1))
            .Where(_ => _)
            .Subscribe(_ =>
            {
                breakpointList.Add(pointList.Count-1);
                MyDrawGLine.BreakpointList = breakpointList;
            });
    }


    private void OnGUI()
    {
        /*if (GUI.Button(new Rect(0, 0, 75, 30), "确定"))
        {
           
        }*/


    }
}