using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParticleModules : MonoBehaviour
{
    [SerializeField] ParticleSystem _myParticle;

    private void Awake()
    {
        var emissionModule = _myParticle.emission;

        emissionModule.rateOverTime = 99;
    }
}
