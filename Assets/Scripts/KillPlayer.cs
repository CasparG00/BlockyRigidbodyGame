using UnityEngine;

public class KillPlayer : MonoBehaviour
{ 
    GameObject[] players;

    public float knockoutRadius = 100f;

    public ShakeTransform st;
    public CameraShakeEvent data;

    public ParticleSystem knockoutParticles;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Update()
    {
        foreach (var player in players)
        {
            if (player.transform.position.sqrMagnitude > knockoutRadius * knockoutRadius)
            {
                player.transform.position = Vector3.zero;
                player.transform.rotation = Quaternion.identity;
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                
                knockoutParticles.Play();
                st.AddShakeEvent(data);
            }
        }
    }
}
