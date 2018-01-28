using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionsMethods;

public class PrefabLink : MonoBehaviour {

    public GameObject prefab;

    public bool Revert(GameObject prefabObject)
    {
        bool revertSuccessful = false;

        if ((prefab == gameObject || prefab == null) && prefabObject != null)
        {
            prefab = prefabObject;
        }

        if (prefab != null)
        {
            //Remove children
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                transform.GetChild(i).gameObject.BlowUp();
            }

            //Remove components
            Component[] components = gameObject.GetComponents<Component>();

            for (int i = components.Length - 1; i >= 0; i--)
            {
                Component component = components[i];

                if (component != this && component != transform)
                {
                    component.BlowUp();
                }
            }

            //Copy prefab components
            foreach (Component component in prefab.GetComponents<Component>())
            {
                if (component.GetType() != GetType() && component.GetType() != typeof(Transform))
                {
                    gameObject.CopyComponent(component);
                }
            }

            //Copy prefab children
            foreach (Transform child in prefab.transform)
            {
                Transform newChild = Instantiate(child, transform, false);
                newChild.name = child.name;
            }

            gameObject.name = prefab.name;

            revertSuccessful = true;
        }

        return revertSuccessful;
    }
}
