using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMouseRayCast : MonoBehaviour
{
    public string wallTag = "Wall"; // Тег стены
    public GameObject television; // Ссылка на телевизор

    void Update()
    {
        // Проверяем нажатие кнопки мыши
        if (Input.GetMouseButtonDown(0)) // 0 означает левую кнопку мыши, 1 - правую, 2 - среднюю
        {
            // Пуск луча от позиции курсора в момент нажатия
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            // Проверяем столкновение луча с объектами
            if (Physics.Raycast(ray, out hitInfo))
            {
                // Проверяем, является ли объект стеной по тегу
                if (hitInfo.collider.CompareTag(wallTag))
                {
                    // Получаем компонент угла поворота стены
                    Wall wallRotation = hitInfo.collider.GetComponent<Wall>();

                    if (wallRotation != null)
                    {
                        // Получаем угол поворота стены и устанавливаем его для телевизора
                        float rotationAngle = wallRotation.televisionRotationAngle;
                        television.transform.position = hitInfo.point;
                        television.transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
                    }
                }
            }
        }
    }
}
