using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityDeligator
{
    SortedList<ActivityKind, Activity> activities;

    public ActivityDeligator()
    {
        activities = new SortedList<ActivityKind, Activity>
        {
            //activityListInitStart
            { ActivityKind.Lesson, new ALesson() },
            { ActivityKind.PartTime, new APartTime() },
            { ActivityKind.Exercize, new AExercize() },
            { ActivityKind.JobSearch, new AJobSearch() },
            { ActivityKind.Meal, new AMeal() },
            { ActivityKind.ReadBook, new AReadBook() },
            { ActivityKind.Report, new AReport() },
            { ActivityKind.Rest, new ARest() },
            { ActivityKind.Sleep, new ASleep() },

        };
        /*
        temp = new GAFirstTestActivity();
        activities.Add(temp.GetKind(), temp);
        temp = new GASestActivity();
        activities.Add(temp.GetKind(), temp);
        temp = new GATTestActivity3();
        activities.Add(temp.GetKind(), temp);
        temp = new GAasdf123();
        activities.Add(temp.GetKind(), temp);
        temp = new GAdisableTest();
        activities.Add(temp.GetKind(), temp);
        */
        //activityLIstinitend

    }
    public void UpdateSelectable(InGameManager inGameManager)
    {
        foreach (Activity activity in activities.Values)
        {
            activity.UpdateSelectable(inGameManager);
        }
    }
    public void DoActivity(InGameManager inGameManager, ActivityKind activityKind)
    {
        GetActivity(activityKind).DoActivity(inGameManager);
    }
    public List<Activity> GetSelectavleActivity()
    {
        List<Activity> result = new List<Activity>();
        foreach (Activity activity in activities.Values)
        {
            if (activity.isSelectable)
                result.Add(activity);
        }
        return result;
    }
    public Activity GetActivity(ActivityKind activityKind)
    {
        if (activities.ContainsKey(activityKind))
            return activities[activityKind];
        else
        {
            Debug.LogError("Activity Not Found");
            return null;
        }

    }
}
