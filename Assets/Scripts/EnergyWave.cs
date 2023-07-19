using System.Collections;
using UnityEngine;

public class EnergyWave : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float growSpeed;
    [SerializeField] private float growScale;
    [SerializeField] private float damage;
    [SerializeField] private float knockback;
    [SerializeField] private float lifetime;

    [Header("References")]
    private Transform player;
    private new Transform camera;
    private Energy energy;
    private new Rigidbody rigidbody;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        camera = Camera.main.transform;
        energy = player.GetComponent<Energy>();
        rigidbody = player.GetComponent<Rigidbody>();
    }

    void Start()
    {
        StartCoroutine(Grow());
        StartCoroutine(Die());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            energy.UseEnergy(damage);
            Vector3 force = (player.position - transform.position).normalized * knockback;
            //Vector3 force = -player.forward * knockback;
            force.y = knockback*.5f;
            rigidbody.AddForce(force);

            Destroy(gameObject);
        }
    }

    private IEnumerator Grow()
    {
        float time = 0;

        for (float t = 0; t <= 1; t += Time.deltaTime/growSpeed)
        {
            time += Time.deltaTime;
            transform.localScale = new Vector3(growScale * t, growScale * t, growScale * t);
            yield return null;

            Debug.Log(time);
        }
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(lifetime);

        Destroy(gameObject);
    }
}
