
using UnityEngine;
using UnityEngine.UI;

public class VersionDisplayer : MonoBehaviour
{

    private string VersionText = "v 1.0.3";
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = VersionText;
    }

   
}
