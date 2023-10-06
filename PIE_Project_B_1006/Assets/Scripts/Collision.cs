using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public string itemTag = "Metal";
    public string itemTag1 = "Skrew";
    public string itemTag2 = "Gear";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag(itemTag))
        {
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag(itemTag1))
        {
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag(itemTag2))
        {
            Destroy(collision.gameObject);
        }
    }
}
