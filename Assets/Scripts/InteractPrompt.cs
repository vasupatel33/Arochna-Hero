using UnityEngine;
using TMPro;

public class InteractPrompt : MonoBehaviour
{
    [Header("References")]
    private Transform player;
    private TextMeshPro WHY;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        WHY = GetComponent<TextMeshPro>();
    }

    void Start()
    {
        WHY.text = "";
        //gameObject.SetActive(false);
    }

    void Update()
    {
        transform.LookAt(player);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            WHY.text = "F";
            //gameObject.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            WHY.text = "";
            //gameObject.SetActive(false);
    }
}
