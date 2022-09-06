using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Buttontouch
    {
        public Button yourButton;

        void Start () {
            Debug.Log ("You have clicked the button!");
            Button btn = yourButton.GetComponent<Button>();
            btn.onClick.AddListener(TaskOnClick);
        }

        void TaskOnClick(){
            Debug.Log ("You have clicked the button!");
        }

        void Update()
        {
            Debug.Log ("You have clicked the button!");
        }
    }
}