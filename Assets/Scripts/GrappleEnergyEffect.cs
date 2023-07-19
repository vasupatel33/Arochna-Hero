using UnityEngine;

public class GrappleEnergyEffect : MonoBehaviour
{
    [Header("States")]
    private float startRate;

    [Header("References")]
    private ParticleSystem particles;
    private ParticleSystem.EmissionModule emissions;
    private ParticleSystem.ShapeModule shape;
    private LineRenderer line;
    private AudioSource sfx;

    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        line = transform.parent.parent.GetComponent<LineRenderer>();
        sfx = GetComponent<AudioSource>();

        emissions = particles.emission;
        shape = particles.shape;
    }

    void Start()
    {
        startRate = emissions.rateOverTime.constant;
    }

    void Update()
    {
        Render();
    }

    private void Render()
    {
        ParticleSystem.EmissionModule emissions = particles.emission;
        
        if (line.positionCount < 2) 
        { 
            particles.Stop();
            sfx.Stop();
            particles.Clear();
            return; 
        }
        
        Vector3 pos = line.GetPosition(1);
        float distance = Vector3.Distance(transform.parent.position, pos);

        ParticleSystem.ShapeModule shape = particles.shape;

        float x = shape.scale.x;
        float y = shape.scale.y;

        float scale = distance * (1.0f/transform.localScale.z);
        shape.scale = new Vector3(x, y, scale);
        
        Vector3 newPosition = Vector3.Lerp(transform.parent.position, pos, 0.5f);
        transform.position = newPosition;

        Vector3 direction = pos - transform.parent.position;
        transform.parent.forward = direction;

        ParticleSystem.MinMaxCurve curve = emissions.rateOverTime;
        curve.constant = startRate * scale;
        emissions.rateOverTime = curve;

        particles.Play();
        if (!sfx.isPlaying)
            sfx.Play();
    }
}
