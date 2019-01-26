using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController mInstance;
    
    public static GameController Instance
    {
        get
        {
            if(mInstance == null)
            {
                mInstance = new GameController();
            }
            return mInstance;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
