using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    [SerializeField]GameObject _gameObject;
    [SerializeField] GameObject Target;
    ParticleSystem _particleSystem;
    float time;
    float delay = 0.1f;


    // Update is called once per frame
    void Update()
    {
        PlayParticle();
    }

    private void PlayParticle()
    {
        if (time + delay < Time.time)
        {
            Instantiate(_gameObject,Target.transform.position, Quaternion.identity);
            time = Time.time;
        }
    }
}
