
using UnityEngine;
using UnityEngine.UI;

public class VersionDisplayer : MonoBehaviour
{

    public string VersionText = "v 1.0.1";
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = VersionText;
    }

   
}
