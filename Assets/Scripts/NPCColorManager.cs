using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCColorManager : MonoBehaviour
{
   public void ChangeColor(Material material)
    {
        GetComponent<MeshRenderer>().material = material;
    }
}
