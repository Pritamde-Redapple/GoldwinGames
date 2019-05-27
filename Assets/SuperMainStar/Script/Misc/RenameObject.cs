using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenameObject : MonoBehaviour {

    public string formatType = "0";

    public Sprite greenImage;
    public Sprite orangeImage;

    public Sprite greenHoverImage;
    public Sprite orangeHoverImage;


    public int totalChild = 100;
    public int noOfColoum = 10;

    public int startingValue = 5;
    public int gap = 0;


    public GameObject prefab;
    public bool onlyRenameObject;

    public string buttonType = "";
    public void RenameAllChild(){


        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).name = (startingValue + (i * gap)).ToString(formatType);
            if (transform.GetChild(i).childCount > 0 && !onlyRenameObject)
            {
                if (buttonType == "Block")
                {
                    transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = (startingValue + (i * gap)).ToString(formatType);
                    transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = (startingValue + (i * gap)).ToString(formatType);
                }
                else
                {
                    transform.GetChild(i).GetChild(0).GetComponent<Text>().text = (startingValue + (i * gap)).ToString(formatType);
                }
            }
        }
    }

    public void CreateChild()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        Sprite dummySprite = orangeImage;
        for (int i = 0; i < totalChild; i++)
        {
            GameObject gOb = Instantiate(prefab);
            gOb.transform.SetParent(this.transform);
            gOb.transform.localScale = Vector3.one;

            if( !(i > noOfColoum - 1 && i % noOfColoum == 0))
            {
                if(dummySprite == greenImage)
                {
                    dummySprite = orangeImage;
                }
                else
                {
                    dummySprite = greenImage;
                }
            }

            gOb.transform.GetChild(0).GetComponent<Image>().sprite = dummySprite;
        }

        RenameAllChild();
    }

    public void SetHoverImage()
    {
        Sprite dummySprite = orangeHoverImage;
        for (int i = 0; i < totalChild; i++)
        {
            Block _block = transform.GetChild(i).GetComponent<Block>();

            if (!(i > noOfColoum - 1 && i % noOfColoum == 0))
            {
                if (dummySprite == greenHoverImage)
                {
                    dummySprite = orangeHoverImage;
                }
                else
                {
                    dummySprite = greenHoverImage;
                }
            }

            _block.sHighliteSprite = dummySprite;

           
        }
    }

}
