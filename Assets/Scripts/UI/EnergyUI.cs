using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyUI : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float time;

    [Header("References")]
    private Energy energy;

    void Awake()
    {
        energy = GameObject.Find("Player").GetComponent<Energy>();
        energy.updateEnergy += EnergyAmountUpdate;
    }

    private void EnergyAmountUpdate()
    {
        StartCoroutine(UpdateUI());
    }

    private IEnumerator UpdateUI()
    {
        float max = energy.MaxEnergy;
        float current = energy.CurrentEnergy;

        float currentScale = transform.localScale.x;
        float goalScale = current/max;
        float change = currentScale - goalScale;

        for(float t = 0; t <= 1; t += Time.deltaTime/time)
        {
            transform.localScale = new Vector3(currentScale - change * t, transform.localScale.y, transform.localScale.z);
            yield return null;
        }

        transform.localScale = new Vector3(goalScale, transform.localScale.y, transform.localScale.z);
    }
}
