using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    public Image[] images; // 컷신 이미지 배열
    private int currentImageIndex = 0; // 현재 이미지 인덱스

    private void Start()
    {
        // 시작 이미지
        ShowCurrentImage();
    }

    private void Update()
    {
        // 마우스 왼쪽 버튼 클릭하면 다음이미지로 넘어가게 함 아마도?
        if (Input.GetMouseButtonDown(0))
        {
            currentImageIndex++;
            if (currentImageIndex < images.Length)
            {
                ShowCurrentImage();
            }
            else
            {
                // 모든 이미지가 표시되었을 때, 다음 씬 로드
                SceneManager.LoadScene("StageScene");
            }
        }
    }

    private void ShowCurrentImage()
    {
        // 현재 이미지 표시
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(i == currentImageIndex);
        }
    }
}
