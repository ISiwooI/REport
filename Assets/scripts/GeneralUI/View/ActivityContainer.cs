using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActivityContainer : MonoBehaviour
{
    public TMP_Text activityName;
    public TMP_Text activityDescription;
    public TMP_Text costMoney;
    public TMP_Text costStamina;
    public TMP_Text costMental;
    public TMP_Text costTime;

    public ActivityView view;
    public ActivityKind activityKind = ActivityKind.None;
    public void Init(ActivityView view)
    {
        this.view = view;
    }

    public void SetActivity(Activity activity)
    {
        activityKind = activity.Kind;
        if (activityKind == ActivityKind.None) this.gameObject.SetActive(false);
        else
        {
            activityName.text = activity.name;
            activityDescription.text = activity.simpleDescription;
            costMoney.text = (activity.rewardMoney - activity.costMoney).ToString();
            costStamina.text = (activity.rewardStamina - activity.costStamina).ToString();
            costMental.text = (activity.rewardMental - activity.costMental).ToString();
            costTime.text = activity.timeDuration.ToString();
        }
    }
    public void OnPushButton()
    {
        view.OnActivityButton(this.activityKind);
    }
}
