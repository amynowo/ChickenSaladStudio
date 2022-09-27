using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Touchbox : MonoBehaviour
{
    public SpriteRenderer birdSpriteRender;
    public Sprite[] birdSprites;
    
    public Animator birdAnimator;
    
    [Range(1, 4)]
    public int laneNumber;

    public static int currentLane;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    IEnumerator BirdDelay()
    {
        yield return new WaitForSeconds(0.25f);
        birdSpriteRender.sprite = birdSprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase is TouchPhase.Began or TouchPhase.Stationary or TouchPhase.Moved)
            {
                birdAnimator.SetBool("Eat", true);
                var wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                var touchPosition = new Vector2(wp.x, wp.y);
 
                if (GetComponent<BoxCollider2D>() == Physics2D.OverlapPoint(touchPosition))
                {
                    birdSpriteRender.sprite = birdSprites[1];
                    currentLane = laneNumber;
                }
                else if (GetComponent<BoxCollider2D>() != Physics2D.OverlapPoint(touchPosition))
                {
                    birdSpriteRender.sprite = birdSprites[0];
                }
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase is TouchPhase.Ended)
            {
                birdAnimator.SetBool("Eat", false);
                //StartCoroutine(nameof(BirdDelay));
                
                //Debug.Log($"Let go of bird {currentLane}");
            }
            /*else
            {
                birdSpriteRender.sprite = birdSprites[0];
            }*/
        }
    }
}
