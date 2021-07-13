using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public GameObject[] objects;
    public GameObject gameobj;
    public Component[] hingeJoints;
    public int countChild;
    public int countVertex = -1;
    public Transform[] childs;
    public float[,] newMatrix;
    public bool waitFlag = true;
    public int[] shortestPath;
    //public int[] path;
    List<int> path = new List<int>() {};

    private void Start()
    {
        //hingeJoints = GetComponentsInChildren<HingeJoint>();
        countChild = transform.childCount;
        childs = new Transform[countChild];
        //string name = "Edge";
        for (int i = 0; i < countChild; i++)
        {
            string index = "Edge (" + i + ")";
            Transform child = transform.Find(index);
            if (child != null)
            {
                childs.SetValue(child, i);        
            }
            string pointIndex = "Point (" + i + ")";
            Transform pointChild = transform.Find(pointIndex);
            if(pointChild != null)
            {
                countVertex++;
            }
        }
    }




    void Update()
    {
        if(waitFlag == true)
        {
            newMatrix = new float[countVertex, countVertex];
            byte vertexX, vertexY;
            for (int i = 0; i < childs.Length; i++)
            {
                if (childs[i] != null)
                {
                    EdgeScript edge = childs[i].GetComponent<EdgeScript>();
                    vertexX = Convert.ToByte(edge.points[0].name.Substring(7).TrimEnd(')'));
                    vertexY = Convert.ToByte(edge.points[1].name.Substring(7).TrimEnd(')'));
                    newMatrix[vertexX, vertexY] = edge.lenght;
                    newMatrix[vertexY, vertexX] = edge.lenght;
                }
            }


            File.Create("Assets/Matrix.txt").Close();
            StreamWriter writer = new StreamWriter("Assets/Matrix.txt", true);

            for (int i = 0; i < newMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < newMatrix.GetLength(1); j++)
                {
                    writer.Write(newMatrix[i, j] + " ");
                }
                writer.WriteLine();

            }
            writer.Close();
            Djekstra();
            waitFlag = false;
        }
    }

    
    int nowPoint;
    private void Djekstra()
    {
        int pathCounter = 0;
        const int vertex = 7;
        int infinity = 1000;
        float[,] matrix = newMatrix;
        
        // Будем искать путь из вершины s в вершину g
        int start = 4;                      // Номер исходной вершины
        int goal = 0;                      // Номер конечной вершины

        int[] shortToPointFlag = new int[vertex]; //Массив, содержащий единицы и нули для каждой вершины,
                                                  // x[i]=0 - еще не найден кратчайший путь в i-ю вершину,
                                                  // x[i]=1 - кратчайший путь в i-ю вершину уже найден
        float[] shortLenghtFromStart = new float[vertex];  //t[i] - длина кратчайшего пути от вершины s в i
        int[] prevPointOnShort = new int[vertex];  //h[i] - вершина, предшествующая i-й вершине
                                                   // на кратчайшем пути

        // Инициализируем начальные значения массивов
        int vertexCounter;          // Счетчик вершин
        for (vertexCounter = 0; vertexCounter < vertex; vertexCounter++)
        {
            shortLenghtFromStart[vertexCounter] = infinity; //Сначала все кратчайшие пути из s в i 
                                                            //равны бесконечности
            shortToPointFlag[vertexCounter] = 0;        // и нет кратчайшего пути ни для одной вершины
        }
        prevPointOnShort[start] = 0; // s - начало пути, поэтому этой вершине ничего не предшествует
        shortLenghtFromStart[start] = 0; // Кратчайший путь из s в s равен 0
        shortToPointFlag[start] = 1; // Для вершины s найден кратчайший путь
        nowPoint = start;    // Делаем s текущей вершиной

        while (true)
        {
            // Перебираем все вершины, смежные v, и ищем для них кратчайший путь
            for (vertexCounter = 0; vertexCounter < vertex; vertexCounter++)
            {
                if (matrix[nowPoint, vertexCounter] == 0) continue; // Вершины u и v несмежные
                if (shortToPointFlag[vertexCounter] == 0 && shortLenghtFromStart[vertexCounter] > shortLenghtFromStart[nowPoint] + matrix[nowPoint, vertexCounter]) //Если для вершины u еще не 
                                                                                                                                                                    //найден кратчайший путь
                                                                                                                                                                    // и новый путь в u короче чем 
                                                                                                                                                                    //старый, то
                {
                    shortLenghtFromStart[vertexCounter] = shortLenghtFromStart[nowPoint] + matrix[nowPoint, vertexCounter];  //запоминаем более короткую длину пути в
                                                                                                                             //массив t и
                    prevPointOnShort[vertexCounter] = nowPoint;   //запоминаем, что v->u часть кратчайшего 
                                                                  //пути из s->u
                }
            }

            // Ищем из всех длин некратчайших путей самый короткий
            float shortLenght = infinity;  // Для поиска самого короткого пути
            nowPoint = -1;            // В конце поиска v - вершина, в которую будет 
                                      // найден новый кратчайший путь. Она станет 
                                      // текущей вершиной
            for (vertexCounter = 0; vertexCounter < vertex; vertexCounter++) // Перебираем все вершины.
            {
                if (shortToPointFlag[vertexCounter] == 0 && shortLenghtFromStart[vertexCounter] < shortLenght) // Если для вершины не найден кратчайший 
                                                                                                               // путь и если длина пути в вершину u меньше
                                                                                                               // уже найденной, то
                {
                    nowPoint = vertexCounter; // текущей вершиной становится u-я вершина
                    shortLenght = shortLenghtFromStart[vertexCounter];
                }
            }
            if (nowPoint == -1)
            {
                Debug.Log("Нет пути из вершины " + start + " в вершину " + goal + ".");
                break;
            }
            if (nowPoint == goal) // Найден кратчайший путь,
            {        // выводим его
                //Debug.Log("Кратчайший путь из вершины " + start + " в вершину " + goal + ": ");
                vertexCounter = goal;
                path.Add(vertexCounter);
                pathCounter++;
                while (vertexCounter != start)
                {
                    //Debug.Log(" " + vertexCounter);
                    vertexCounter = prevPointOnShort[vertexCounter];
                    path.Add(vertexCounter);
                    pathCounter++;
                }
                shortestPath = new int[pathCounter];
                for (int i = 0; i < path.Count; i++)
                {
                    shortestPath[i] = path[path.Count - i - 1];
                }
                Debug.Log("Длина пути - " + shortLenghtFromStart[goal] + "\n Количество элементов: " + shortestPath.Length);
                foreach(var item in shortestPath)
                {
                    print(item);
                }
                break;
            }
            shortToPointFlag[nowPoint] = 1;
        }
    }
}
