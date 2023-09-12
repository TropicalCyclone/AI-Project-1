using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraZoom : MonoBehaviour
{

    private CinemachineFreeLook cameraFreeLook;
    private CinemachineFreeLook.Orbit[] originalOrbits;

    public float minZoom = 0.5f;
    public float maxZoom = 1.0f;

    [AxisStateProperty]
    public AxisState zAxis = new AxisState(0,1,false,true,50f,0.1f,0.1f,"Mouse ScrollWheel",false);
    // Start is called before the first frame update
    void Start()
    {
        cameraFreeLook = GetComponent<CinemachineFreeLook>();
        if(cameraFreeLook != null)
        {
            originalOrbits = new CinemachineFreeLook.Orbit[cameraFreeLook.m_Orbits.Length];
            for(int i = 0; i < originalOrbits.Length; i++)
            { 
                originalOrbits[i].m_Height = cameraFreeLook.m_Orbits[i].m_Height;
                originalOrbits[i].m_Radius = cameraFreeLook.m_Orbits[i].m_Radius;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(originalOrbits != null)
        {
            zAxis.Update(Time.deltaTime);
            float zoomScale = Mathf.Lerp(minZoom, maxZoom, zAxis.Value);

            for(int i = 0; i <originalOrbits.Length-1; i++) 
            {
                cameraFreeLook.m_Orbits[i].m_Height = originalOrbits[i].m_Height * zoomScale;
                cameraFreeLook.m_Orbits[i].m_Radius = originalOrbits[i].m_Radius * zoomScale;
            }
        }
    }
}
