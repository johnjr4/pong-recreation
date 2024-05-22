using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateCameraWalls : MonoBehaviour
{

    private Camera cam;
    private float sizeX, sizeY, ratio;
    // public EdgeCollider2D topEdge;
    // public EdgeCollider2D bottomEdge;
    public BoxCollider2D topBox;
    public BoxCollider2D bottomBox;
    [SerializeField]
    private float boxHeight = 5f;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        sizeY = cam.orthographicSize * 2;
        ratio = (float)Screen.width / (float)Screen.height;
        sizeX = sizeY * ratio;

        //Vector2[] topPoints = topEdge.points;
        //Vector2[] bottomPoints = bottomEdge.points;

        //topPoints[0] = new Vector2(-sizeX / 2, sizeY / 2);
        //topPoints[1] = new Vector2(sizeX / 2, sizeY / 2);
        //bottomPoints[0] = new Vector2(-sizeX / 2, -sizeY / 2);
        //bottomPoints[1] = new Vector2(sizeX / 2, -sizeY / 2);

        //topEdge.points = topPoints;
        //bottomEdge.points = bottomPoints;

        topBox.size = new Vector2(sizeX, boxHeight);
        bottomBox.size = new Vector2(sizeX, boxHeight);
        topBox.offset = new Vector2(0, sizeY / 2 + boxHeight / 2);
        bottomBox.offset = new Vector2(0, -sizeY / 2 - boxHeight / 2);
    }
}
