using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    public Image[] images; // �ƽ� �̹��� �迭
    private int currentImageIndex = 0; // ���� �̹��� �ε���

    private void Start()
    {
        // ���� �̹���
        ShowCurrentImage();
    }

    private void Update()
    {
        // ���콺 ���� ��ư Ŭ���ϸ� �����̹����� �Ѿ�� �� �Ƹ���?
        if (Input.GetMouseButtonDown(0))
        {
            currentImageIndex++;
            if (currentImageIndex < images.Length)
            {
                ShowCurrentImage();
            }
            else
            {
                // ��� �̹����� ǥ�õǾ��� ��, ���� �� �ε�
                SceneManager.LoadScene("StageScene");
            }
        }
    }

    private void ShowCurrentImage()
    {
        // ���� �̹��� ǥ��
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(i == currentImageIndex);
        }
    }
}
