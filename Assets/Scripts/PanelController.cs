using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelController : MonoBehaviour
{

    public RectTransform panel;

    public void AbrirPanel()
    {
        panel.DOAnchorPos(new Vector2(0, 0), 0.5f);
    }


    public void OcultarPanel()
    {
        panel.DOAnchorPos(new Vector2(812, 0), 0.5f);
    }
}
