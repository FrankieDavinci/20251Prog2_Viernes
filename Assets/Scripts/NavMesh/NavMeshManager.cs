using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public static class NavMeshManager
{
    private static NavMeshSurface _surface;

    public static NavMeshSurface Surface
    {
        get
        {
            return _surface;
        }
        set
        {
            _surface = value;
            UpdateSurface();
        }
    }

    public static void UpdateSurface()
    {
        if (!_surface) return;
        
        _surface.BuildNavMesh();
    }
}
