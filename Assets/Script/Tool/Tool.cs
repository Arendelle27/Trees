using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Tool
{
    //���ھ�̬����this����ʹSpriteRender����ֱ�ӵ���.IsClickDown
    public static bool IsTorch(this SpriteRenderer spriteRenderer,Vector3 mousePosition)//�ж������Ƿ�������
    {
        if (spriteRenderer.sprite == null)
            return false;
        var pos = new Vector3(mousePosition.x, mousePosition.y, spriteRenderer.transform.position.z);
        var bounds = spriteRenderer.bounds;
        return bounds.Contains(pos);//���������ͼ���ڲ�������true
    }
}
