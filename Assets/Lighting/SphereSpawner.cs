using UnityEngine;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

public sealed class SphereSpawner : MonoBehaviour
{
    [field:SerializeField] SphereController Prefab { get; set; } = null;
    [field:SerializeField] float Interval { get; set; } = 1;
    [field:SerializeField] float Velocity  { get; set; } = 1;
    [field:SerializeField] float Randomness { get; set; } = 0.2f;
    [field:SerializeField] uint Seed { get; set; } = 1;

    float BiasRandom(float r) => 1 - r * Randomness;

    async void Start()
    {
        var rng = new Random(Seed);
        rng.NextUInt4(); // Warm up

        while (true)
        {
            var pos = transform.position;
            pos.y *= BiasRandom(rng.NextFloat());

            var i = Instantiate(Prefab, pos, transform.rotation);
            i.Setup(Velocity, BiasRandom(rng.NextFloat()), rng.NextFloat());

            var sec = Interval * BiasRandom(rng.NextFloat());
            await Awaitable.WaitForSecondsAsync(sec);
        }
    }
}
