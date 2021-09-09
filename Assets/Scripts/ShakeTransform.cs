using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShakeTransform : MonoBehaviour
{
    private class ShakeEvent
    {
        private readonly float duration;
        private float timeRemaining;

        private readonly CameraShakeEvent data;

        public CameraShakeEvent.Target target => data.target;

        private Vector3 noiseOffset;
        public Vector3 noise;

        public ShakeEvent(CameraShakeEvent data)
        {
            this.data = data;

            duration = data.duration;
            timeRemaining = duration;

            const float rand = 32f;

            noiseOffset.x = Random.Range(0f, rand);
            noiseOffset.y = Random.Range(0f, rand);
            noiseOffset.z = Random.Range(0f, rand);
        }

        public void Update()
        {
            var deltaTime = Time.deltaTime;

            timeRemaining -= deltaTime;

            var noiseOffsetDelta = deltaTime * data.frequency;

            noiseOffset.x += noiseOffsetDelta;
            noiseOffset.y += noiseOffsetDelta;
            noiseOffset.z += noiseOffsetDelta;

            noise.x = Mathf.PerlinNoise(noiseOffset.x, 0f);
            noise.y = Mathf.PerlinNoise(noiseOffset.y, 1f);
            noise.z = Mathf.PerlinNoise(noiseOffset.z, 2f);

            noise -= Vector3.one * 0.5f;

            noise *= data.amplitude;
            
            var agePercent = 1f - (timeRemaining / duration);
            noise *= data.blendOverLifetime.Evaluate(agePercent);
        }

        public bool IsAlive()
        {
            return timeRemaining > 0f;
        }
    }

    private readonly List<ShakeEvent> shakeEvents = new List<ShakeEvent>();

    public void AddShakeEvent(CameraShakeEvent data)
    {
        shakeEvents.Add(new ShakeEvent(data));
    }

    public void AddShakeEvent(float amplitude, float frequency, float duration, AnimationCurve blendOverLifetime, CameraShakeEvent.Target target)
    {
        var data = ScriptableObject.CreateInstance<CameraShakeEvent>();
        data.Init(amplitude, frequency, duration, blendOverLifetime, target);
        
        AddShakeEvent(data);
    }

    private void LateUpdate()
    {
        var positionOffset = Vector3.zero;
        var rotationOffset = Vector3.zero;

        for (var i = shakeEvents.Count - 1; i != -1; i--)
        {
            var se = shakeEvents[i]; se.Update();

            if (se.target == CameraShakeEvent.Target.Position)
            {
                positionOffset += se.noise;
            }
            else
            {
                rotationOffset += se.noise;
            }

            if (!se.IsAlive())
            {
                shakeEvents.RemoveAt(i);
            }
        }
        
        transform.localPosition = positionOffset;
        transform.localEulerAngles = rotationOffset;
    }
}
