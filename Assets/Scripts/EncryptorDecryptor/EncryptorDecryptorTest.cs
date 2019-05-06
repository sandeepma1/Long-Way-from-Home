using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncryptorDecryptorTest : MonoBehaviour
{
    private IItem item;
    public string text;
    private void Start()
    {
        item = GetComponent<IItem>();
        print(item.GetItemId() + " " + item.GetItemName() + " " + item.GetItemType());
        print(EncryptorDecryptor.EncryptDecrypt(text));
    }
}