using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    public int value = 5;  
    public float fadeDuration = 0.5f;

    private bool collected = false;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        if (other.CompareTag("Car")) 
        {
            collected = true;

            GameManager.instance.AddScore(value);

          
            StartCoroutine(FadeAndDestroy());
        }
    }

    IEnumerator FadeAndDestroy()
    {
        float t = 0;
        Color c = sr.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            sr.color = c;
            yield return null;
        }
        Destroy(gameObject);
    }
}
