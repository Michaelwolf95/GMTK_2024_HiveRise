using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{ 
    public GameObject Panel;
    public GameObject UIPanel;
    public GameObject Cog;

    public float xAngle, yAngle, zAngle;

    public void openPanel()
    {
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;

            Panel.SetActive(!isActive);
        }
        if (UIPanel != null)
        {
            bool isActive = UIPanel.activeSelf;

            UIPanel.SetActive(!isActive);
        }

        Cog.transform.Rotate(xAngle, yAngle, zAngle, Space.Self);
    }
    
}
