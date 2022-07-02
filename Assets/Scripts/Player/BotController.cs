using System.Collections;
using UnityEngine;

public class BotController : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    private const string talk = "Talk";
    private const string talkType = "TalkType";

    public void Talk_Stay()
    {
        anim.SetBool(talk, true);
        anim.SetInteger(talkType, 0);
    }
    public void Talk_Default()
    {
        anim.SetBool(talk, true);
        StartCoroutine(DelayedChangeTalkTimeCoroutine((int)AnimType.SimpleTalk));
    }
    public void Talk_Joy()
    {
        anim.SetBool(talk, true);
        StartCoroutine(DelayedChangeTalkTimeCoroutine((int)AnimType.JoyTalk));
    }
    public void Talk_Sad()
    {
        anim.SetBool(talk, true);
        StartCoroutine(DelayedChangeTalkTimeCoroutine((int)AnimType.SadTalk));
    }
    public void Talk_Think()
    {
        anim.SetBool(talk, true);
        StartCoroutine(DelayedChangeTalkTimeCoroutine((int)AnimType.JustThink));
    }
    public void Talk_Decision()
    {
        anim.SetBool(talk, true);
        StartCoroutine(DelayedChangeTalkTimeCoroutine((int)AnimType.FindDecision));
    }
    public void Talk_Yes()
    {
        anim.SetBool(talk, true);
        StartCoroutine(DelayedChangeTalkTimeCoroutine((int)AnimType.Yes));
    }
    public void Talk_No()
    {
        anim.SetBool(talk, true);
        StartCoroutine(DelayedChangeTalkTimeCoroutine((int)AnimType.No));
    }
    public void StopTalk()
    {
        anim.SetBool(talk, false);
        anim.SetInteger(talkType, 0);
    }

    private IEnumerator DelayedChangeTalkTimeCoroutine(int talkTypeValue)
    {
        anim.SetInteger(talkType, 0);
        yield return new WaitForSeconds(0.5f);
        anim.SetInteger(talkType, talkTypeValue);
    }
}

public enum AnimType
{
    SimpleTalk = 1,
    JoyTalk = 2,
    SadTalk = 3,
    JustThink = 4,
    FindDecision = 5,
    Yes = 6,
    No = 7
}
