using UnityEngine;
using UnityEngine.EventSystems;

public class Touchbox : MonoBehaviour
{
    public Animator birdAnimator;
    
    [Range(1, 4)]
    public int laneNumber;

    [SerializeField] public Lane lane;
    
    public static int currentLane;
    public static int currentIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase is TouchPhase.Began or TouchPhase.Stationary)
            {
                var wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                var touchPosition = new Vector2(wp.x, wp.y);
 
                if (GetComponent<BoxCollider2D>() == Physics2D.OverlapPoint(touchPosition))
                {
                    currentIndex = lane.inputIndex;
                    birdAnimator.SetBool("Eat", true);
                    currentLane = laneNumber;
                }
                else if (GetComponent<BoxCollider2D>() != Physics2D.OverlapPoint(touchPosition))
                {
                    birdAnimator.SetBool("Eat", false);
                }
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase is TouchPhase.Ended)
            {
                birdAnimator.SetBool("Eat", false);
            }
        }
    }
}
