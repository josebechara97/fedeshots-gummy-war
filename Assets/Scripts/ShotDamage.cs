using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotDamage : MonoBehaviour
{
    public float damage = 1f;
    public AudioClip sfx;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<ExamplePlayerController>().drunkPercentage += damage;
            AudioSource.PlayClipAtPoint(sfx, transform.position);
        }
            
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyBehavior>().drunkPercentage += damage;
            AudioSource.PlayClipAtPoint(sfx, transform.position);
        }
        if (other.CompareTag("EnemySpawner"))
        {
            other.gameObject.GetComponent<EnemySpawnerBehavior>().drunkPercentage += damage;
            AudioSource.PlayClipAtPoint(sfx, transform.position);
        }

        Destroy(gameObject);
    }
}
