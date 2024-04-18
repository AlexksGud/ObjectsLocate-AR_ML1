using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMouseRayCast : MonoBehaviour
{
    public string wallTag = "Wall"; // ��� �����
    public GameObject television; // ������ �� ���������

    void Update()
    {
        // ��������� ������� ������ ����
        if (Input.GetMouseButtonDown(0)) // 0 �������� ����� ������ ����, 1 - ������, 2 - �������
        {
            // ���� ���� �� ������� ������� � ������ �������
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            // ��������� ������������ ���� � ���������
            if (Physics.Raycast(ray, out hitInfo))
            {
                // ���������, �������� �� ������ ������ �� ����
                if (hitInfo.collider.CompareTag(wallTag))
                {
                    // �������� ��������� ���� �������� �����
                    Wall wallRotation = hitInfo.collider.GetComponent<Wall>();

                    if (wallRotation != null)
                    {
                        // �������� ���� �������� ����� � ������������� ��� ��� ����������
                        float rotationAngle = wallRotation.televisionRotationAngle;
                        television.transform.position = hitInfo.point;
                        television.transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
                    }
                }
            }
        }
    }
}
