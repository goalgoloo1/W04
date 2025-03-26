using UnityEditor;
using UnityEngine;

public class Reset3DTo2D : EditorWindow
{
    [MenuItem("Tools/Reset 3D Objects to 2D")]
    public static void ConvertAll()
    {
        // 모든 게임 오브젝트 중 선택 가능한 것들 필터링
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // X축 Position과 Rotation을 0으로 설정
            Vector3 pos = obj.transform.position;
            pos.z = 0;
            obj.transform.position = pos;

            Vector3 rot = obj.transform.eulerAngles;
            rot.x = 0; rot.y = 0; rot.z = 0;
            obj.transform.eulerAngles = rot;
        }

        Debug.Log("모든 3D 오브젝트의 z축 Position/Rotation을 0으로 리셋 완료!");
    }
}