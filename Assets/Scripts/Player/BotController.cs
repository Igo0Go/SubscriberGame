using System.Collections;
using UnityEngine;

public class BotController : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Animator lightAnim;
    [SerializeField, Min(0), Space(10)]
    private float moveSpeed;
    [SerializeField]
    private Vector3 specPointOffset;

    private Transform myTransform;
    private bool gameplayPosUsed;

    private const string talk = "Talk";
    private const string talkType = "TalkType";
    private const string lightActive = "Active";

    private void Start()
    {
        GameCenter.Bot = this;
        myTransform = transform;
        gameplayPosUsed = false;
    }

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
    public void Talk_Tired()
    {
        anim.SetBool(talk, true);
        StartCoroutine(DelayedChangeTalkTimeCoroutine((int)AnimType.Tired));
    }
    public void Talk_Force()
    {
        anim.SetBool(talk, true);
        StartCoroutine(DelayedChangeTalkTimeCoroutine((int)AnimType.Force));
    }
    public void StopTalk()
    {
        anim.SetBool(talk, false);
        anim.SetInteger(talkType, 0);
    }
    public void LightOn()
    {
        lightAnim.SetBool(lightActive, true);
        if(!gameplayPosUsed)
        {
            StopCoroutine(MoveToTargetCoroutine(Vector3.zero));
            StartCoroutine(MoveToTargetCoroutine(specPointOffset));
        }
    }
    public void LightOff()
    {
        lightAnim.SetBool(lightActive, false);
        if (!gameplayPosUsed)
        {
            StopCoroutine(MoveToTargetCoroutine(specPointOffset));
            StartCoroutine(MoveToTargetCoroutine(Vector3.zero));
        }
    }
    public void ToSpecPoint()
    {
        StopCoroutine(MoveToTargetCoroutine(Vector3.zero));
        StartCoroutine(MoveToTargetCoroutine(specPointOffset));
        gameplayPosUsed = true;
    }
    public void ToDefaultPoint()
    {
        StopCoroutine(MoveToTargetCoroutine(specPointOffset));
        StartCoroutine(MoveToTargetCoroutine(Vector3.zero));
        gameplayPosUsed = false;
    }

    private IEnumerator MoveToTargetCoroutine(Vector3 targetLocalPosition)
    {
        while (true)
        {
            Vector3 direction = targetLocalPosition - myTransform.localPosition;

            float step = moveSpeed * Time.deltaTime;

            if(direction.magnitude < 2*step)
            {
                myTransform.localPosition = targetLocalPosition;
                myTransform.forward = myTransform.parent.forward;
                break;
            }
            else
            {
                myTransform.localPosition += step * direction.normalized;
            }

            yield return null;
        }
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
    No = 7,
    Tired = 8,
    Force = 9
}
