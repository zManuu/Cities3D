using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadData : MonoBehaviour
{

    [Serializable]
    public class Data
    {
        public int X;
        public int Y;
        public int Type;
        public int Rotation;

        public Data(int X, int Y, int Type, int Rotation)
        {
            this.X = X;
            this.Y = Y;
            this.Type = Type;
            this.Rotation = Rotation;
        }
    }



    public Data data;

    public void SetData(int x, int y, int type, int rotation)
    {
        this.data = new Data(x, y, type, rotation);
    }
   

    

}
