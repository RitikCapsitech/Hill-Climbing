using UnityEngine;
using System.Collections;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Car"))
        {
            StartCoroutine(DelayedGameWon());
        }
    }

    IEnumerator DelayedGameWon()
    {
        yield return new WaitForSeconds(1f); 
        GameManager.instance.GameWon();
    }
}
