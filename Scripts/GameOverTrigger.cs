using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bubble")
        {
            Vector2 bubbleVelocity = collision.GetComponent<Rigidbody2D>().velocity;
            if (bubbleVelocity == Vector2.zero)
            {
                Debug.Log("gameOver");
            }   
                
        }
    }
}
