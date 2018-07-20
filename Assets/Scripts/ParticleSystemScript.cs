using UnityEngine;

public class ParticleSystemScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ps.GetComponent<Renderer>().sortingLayerName = "Particles";
    }
    
}
