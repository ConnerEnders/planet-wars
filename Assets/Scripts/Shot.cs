using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField] GameObject explosion;

    private void OnTriggerEnter(Collider collision)
    {
        explosion.transform.position = transform.position;
        GameObject newExplosion = Instantiate(explosion);
        newExplosion.transform.position = transform.position;
        Destroy(transform.parent.gameObject);
    }
}
