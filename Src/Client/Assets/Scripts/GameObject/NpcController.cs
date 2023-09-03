using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Models;

public class NpcController : MonoBehaviour
{
    public int npcID;
    Animator animator;

    NpcDefine npc;

    SkinnedMeshRenderer render;
    Color originColor;

    private bool inInteractive = false;

    void Start()
    {
        render = GetComponentInChildren<SkinnedMeshRenderer>();
        originColor = render.sharedMaterial.color;
        animator = gameObject.GetComponent<Animator>();
        npc = NpcManager.Instance.GetNpcDefine(npcID);
        StartCoroutine(Actions());
    }

    IEnumerator Actions()
    {
        while (true)
        {
            if (inInteractive)
            {
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
            Relax();
        }
    }

    void Update()
    {

    }

    void Relax()
    {
        animator.SetTrigger("Relax");
    }

    private void OnMouseDown()
    {
        Interactive();
    }

    void Interactive()
    {
        if (!inInteractive)
        {
            inInteractive = true;
            StartCoroutine(DoInteractive());
        }
    }

    IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();
        if (NpcManager.Instance.Interactive(npc))
        {
            animator.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inInteractive = false;
    }

    IEnumerator FaceToPlayer()
    {
        Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - transform.position).normalized;
        while (Mathf.Abs(Vector3.Angle(gameObject.transform.forward, faceTo)) > 5)
        {
            gameObject.transform.forward = Vector3.Lerp(gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }

    private void OnMouseOver()
    {
        Highlight(true);
    }

    private void OnMouseEnter()
    {
        Highlight(true);
    }

    private void OnMouseExit()
    {
        Highlight(false);
    }

    private void Highlight(bool highlight)
    {
        if (highlight)
        {
            if (render.sharedMaterial.color != Color.white)
            {
                render.sharedMaterial.color = Color.white;
            }
        }
        else
        {
            if (render.sharedMaterial.color != originColor)
            {
                render.sharedMaterial.color = originColor;
            }
        }
    }
}
