using UnityEngine;

public class Touchbox : MonoBehaviour
{
    [Range(1, 4)]
    public int laneNumber;

    public static int currentLane;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
 
            var wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            var touchPosition = new Vector2(wp.x, wp.y);
 
            if (GetComponent<PolygonCollider2D>() == Physics2D.OverlapPoint(touchPosition))
            {
                currentLane = laneNumber;
                //Debug.Log($"Lane {laneNumber} HIT!");
            }
            else{
                //Debug.Log("MISS");
            }
        }
    }
}
