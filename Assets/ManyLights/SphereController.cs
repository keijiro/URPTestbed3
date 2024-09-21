using UnityEngine;
using Unity.Mathematics;

public sealed class SphereController : MonoBehaviour
{
    [field:SerializeField] float Life { get; set; } = 3;
    [field:SerializeField] float Fading { get; set; } = 0.2f;

    float _scale;
    float _intensity;

    public void Setup(float velocity, float scale, float hue)
    {
        var light = GetComponent<Light>();

        _scale = transform.localScale.x * scale;
        _intensity = light.intensity;

        GetComponent<Rigidbody>().linearVelocity = Vector3.right * velocity;

        var color = Color.HSVToRGB(hue, 1, 1);
        GetComponent<Renderer>().material.SetColor("_EmissionColor", color);
        light.color = color;
    }

    async void Start()
    {
        var light = GetComponent<Light>();

        for (var t = 0.0f; t < Life; t += Time.deltaTime)
        {
            var f_i = math.smoothstep(0, Fading, t);
            var f_o = math.smoothstep(Life - Fading, Life, t);
            var s = f_i - f_o;

            transform.localScale = Vector3.one * _scale * s;
            light.intensity = _intensity * s;

            await Awaitable.NextFrameAsync();
        }

        Destroy(gameObject);
    }
}
