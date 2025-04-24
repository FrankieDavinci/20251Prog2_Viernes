using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent(typeof(NavMeshSurface))]
public class SurfaceReference : MonoBehaviour
{
    private void Start()
    {
        NavMeshManager.Surface = GetComponent<NavMeshSurface>();
    }
}
