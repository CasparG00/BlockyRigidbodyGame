using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public ShakeTransform st;
    public CameraShakeEvent data;

    public float minVelocity = 100f;

    void OnCollisionEnter(Collision other)
    {
        if (other.relativeVelocity.magnitude > minVelocity)
        {
            st.AddShakeEvent(data);
        }
    }
}
