using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VCA : MonoBehaviour
{

    public FMOD.Studio.VCA vcaController;
    public string vcaName;

    private Slider slider;

    private void Start()
    {
        vcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + vcaName);
        slider = GetComponent<Slider>();
    }
    
    public void SetVolume(float volume)
    {
        vcaController.setVolume(volume);
    }

}
