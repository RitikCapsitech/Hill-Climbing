using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public int checkpointNumber = 1; 

    private GameManager manager;
    private bool hasBeenCrossed = false;

    void Start()
    {
        manager = GameManager.instance; 

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
     
        if (other.CompareTag("Car") && !hasBeenCrossed)
        {
            hasBeenCrossed = true;
          
            if (manager != null)
            {
                manager.CheckpointCrossed(checkpointNumber);
            }
           
        }
    }

    
}