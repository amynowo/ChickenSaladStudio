using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Touchbox : MonoBehaviour
{
    public Animator[] birdAnimators;
    public Animator animator;
    
    [Range(1, 4)]
    public int laneNumber;

    [SerializeField] public Lane lane;
    
    public static int currentLane;
    public static int currentIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = birdAnimators[PlayerPrefs.GetInt(PlayerPrefs.GetString($"Bird{laneNumber}Skin"))];
        animator.gameObject.SetActive(true);
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
                    animator.SetBool("Eat", true);
                    currentLane = laneNumber;
                }
                else if (GetComponent<BoxCollider2D>() != Physics2D.OverlapPoint(touchPosition))
                {
                    animator.SetBool("Eat", false);
                }
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase is TouchPhase.Ended)
            {
                animator.SetBool("Eat", false);
            }
        }
    }
}
