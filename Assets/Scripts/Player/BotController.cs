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
    private Transform gameplayPoint;
    [SerializeField]
    private Light lamp;
    [SerializeField]
    private Transform botLampPoint;
    [SerializeField]
    private Transform playerLampPoint;

    private Transform myTransform;
    private bool gameplayPosUsed;

    private (float range, float intensity) savedLightSettings;

    private const string talk = "Talk";
    private const string talkType = "TalkType";
    private const string lightActive = "Active";

    private void Start()
    {
        GameCenter.Bot = this;
        myTransform = transform;
        gameplayPosUsed = false;

        savedLightSettings.intensity = lamp.intensity;
        savedLightSettings.range = lamp.range;

        lamp.range = lamp.intensity = 0;
        lamp.enabled = false;
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
        StartCoroutine(MoveLightToPlayerCoroutine());
        if (!gameplayPosUsed)
        {
            StopCoroutine(MoveToTargetCoroutine(Vector3.zero));
            StartCoroutine(MoveToTargetCoroutine(gameplayPoint.localPosition));
        }
    }
    public void LightOff()
    {
        StartCoroutine(MoveLightToBotCoroutine());
    }
    public void ToSpecPoint()
    {
        StopCoroutine(MoveToTargetCoroutine(Vector3.zero));
        StartCoroutine(MoveToTargetCoroutine(gameplayPoint.localPosition));
        gameplayPosUsed = true;
    }
    public void ToDefaultPoint()
    {
        StopCoroutine(MoveToTargetCoroutine(gameplayPoint.localPosition));
        StartCoroutine(MoveToTargetCoroutine(Vector3.zero));
        gameplayPosUsed = false;
    }

    private IEnumerator MoveLightToPlayerCoroutine()
    {
        yield return new WaitForSeconds(1);
        lamp.enabled = true;
        Transform lampTransform = lamp.transform;
        lampTransform.parent = playerLampPoint;
        float t = 0;
        Vector3 start = lampTransform.position;

        while (t < 1)
        {
            t += Time.deltaTime;
            lampTransform.position = Vector3.Lerp(start, playerLampPoint.position, t);
            lamp.intensity = Mathf.Lerp(0, savedLightSettings.intensity, t);
            lamp.range = Mathf.Lerp(0, savedLightSettings.range, t);
            yield return null;
        }
        lampTransform.position = Vector3.Lerp(start, playerLampPoint.position, 1);
    }
    private IEnumerator MoveLightToBotCoroutine()
    {
        Transform lampTransform = lamp.transform;
        lampTransform.parent = botLampPoint;
        float t = 0;
        Vector3 start = lampTransform.position;

        while (t < 1)
        {
            t += Time.deltaTime;
            lampTransform.position = Vector3.Lerp(start, botLampPoint.position, t);
            lamp.intensity = Mathf.Lerp(0, savedLightSettings.intensity, t);
            lamp.range = Mathf.Lerp(0, savedLightSettings.range, t);
            yield return null;
        }
        lampTransform.position = Vector3.Lerp(start, botLampPoint.position, 1);
        lamp.enabled = false;
        lightAnim.SetBool(lightActive, false);
        if (!gameplayPosUsed)
        {
            StopCoroutine(MoveToTargetCoroutine(gameplayPoint.localPosition));
            StartCoroutine(MoveToTargetCoroutine(Vector3.zero));
        }
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
