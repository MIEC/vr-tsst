using UnityEngine;

[RequireComponent(typeof(Light))]
public class FocusLight : MonoBehaviour
{
    [SerializeField]
    private float _duration = 1.0f;

    private Light _light;

    void Start()
    {
        _light = gameObject.GetComponent<Light>();
    }

    void Update()
    {
        float phi = Time.time / _duration * 2 * Mathf.PI;
        float amplitude = Mathf.Cos(phi) * 0.5f + 0.5f;

        _light.intensity = amplitude;
    }

}
