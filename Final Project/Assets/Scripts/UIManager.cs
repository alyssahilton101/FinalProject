using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //Handles the logic for showing and hiding UI panels, updating HUD elements, and managing user inputs.
    //Should listen to a bunch of stuff and update accordingly given an event (probably)

    [SerializeField] TextMeshProUGUI food;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateFood(int amount) {
        food.text = "Food: " + amount;
    }
}
