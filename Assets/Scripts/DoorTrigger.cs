using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject Door;
    [SerializeField] private GameObject SecretLocation;
    [SerializeField] private GameObject[] RequiredItems;
    [SerializeField] private float openSpeed;
    [SerializeField] private float openHeight;
    [SerializeField] private bool removeAfterOpen;

    private List<GameObject> PlayerBag;

    private float doorStartY;
    private bool openStart;

    void Start()
    {
        PlayerBag = GameObject.Find("Player").gameObject.GetComponent<Bag>().bag;

        doorStartY = Door.transform.position.y;
        openStart = false;
    }

    void FixedUpdate()
    {
        if(openStart)
        {
            OpenDoor();

            if(Door.transform.position.y >= doorStartY + openHeight)
            {
                openStart = false;
                Destroy(this.gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player" && PlayerHasRequiredItems())
        {
            GetComponent<BoxCollider2D>().enabled = false;

            if (RequiredItems.Length > 0 && removeAfterOpen == true)
            {
                for (int i = PlayerBag.Count - 1; i > -1; i--)
                {
                    for(int j = RequiredItems.Length - 1; j > -1; j--)
                    {
                        if(PlayerBag[i] == RequiredItems[j])
                        {
                            GameObject item = RequiredItems[j];
                            PlayerBag.RemoveAt(i);
                            Destroy(RequiredItems[j]);
                        }
                    }
                }
            }
            openStart = true;

            if (SecretLocation != null)
            {
                SecretLocation.gameObject.SetActive(true);
            }
        }
    }

    void OpenDoor()
    {
        Door.transform.position += new Vector3(0f, openSpeed * Time.deltaTime, 0f);
    }

    bool PlayerHasRequiredItems()
    {
        if(RequiredItems.Length > 0)
        {
            int counterItems = 0;
            foreach (GameObject item in PlayerBag)
            {
                foreach (GameObject requiredItem in RequiredItems)
                {
                    if (item == requiredItem)
                    {
                        counterItems++;
                    }
                }
            }

            if (counterItems != RequiredItems.Length)
            {
                return false;
            }
        }

        return true;
    }
}
