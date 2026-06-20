using System;
using UnityEngine;

namespace RwbyAP.Models;

public class GameObjectStateOverride : MonoBehaviour
{
    public Action<GameObject> OnUpdate;

    private void Update()
    {
        if (OnUpdate != null) OnUpdate(this.gameObject);
    }
}
