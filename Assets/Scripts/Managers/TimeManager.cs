using System.Collections.Generic;
using UnityEngine;

public class TimeManager
{
    public bool IsTimeStopped { get; set; }

    public delegate void MyDelegate();

    private static TimeManager instance;

    public static TimeManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TimeManager();
            }
            return instance;
        }
    }

    private TimeManager()
    {

    }
    // public delegate string MyArgumentDelegate(string S);




    List<DelegateTimer> delegateTimerList;
    public void Initialize()
    {

        delegateTimerList = new List<DelegateTimer>();
    }
    public void Refresh()
    {
        for (int i = delegateTimerList.Count - 1; i >= 0; i--)
        {
            if (IsTimeStopped)
            {
                if (delegateTimerList[i].isRelatedToTime)
                {
                    if (delegateTimerList[i].timeToInvoke <= Time.time && delegateTimerList[i].isRelatedToTime)
                    {

                        try
                        {
                            delegateTimerList[i].delegateToInvoke();
                        }
                        catch (System.Exception)
                        {

                            //Debug.LogError("Exception Thrown");
                        }
                        delegateTimerList.RemoveAt(i);
                    }
                }
            }
            else
            {
                if (delegateTimerList[i].timeToInvoke <= Time.time)
                {
                    try
                    {
                        delegateTimerList[i].delegateToInvoke();
                    }
                    catch (System.Exception)
                    {

                        //Debug.LogError("Exception Thrown");
                    }
                    delegateTimerList.RemoveAt(i);
                }
            }
        }
    }
    public void EndGame()
    {
        delegateTimerList.Clear();
    }


    public void AddDelegate(MyDelegate del, float time, int repeat)
    {
        for (int i = 1; i <= repeat; i++)
        {
            DelegateTimer toADD = new DelegateTimer(Time.time + (time * i), del);
            delegateTimerList.Add(toADD);
        }
    }

    public void AddDelegateRelatedToTime(MyDelegate del, float time, int repeat, bool isRelatedToTime)
    {
        for (int i = 1; i <= repeat; i++)
        {
            DelegateTimer toADD = new DelegateTimer(Time.time + (time * i), del, isRelatedToTime);
            delegateTimerList.Add(toADD);
        }
    }

    public void AddTimeToDelegateMethods(float abilityTime)
    {
        for (int i = 0; i < delegateTimerList.Count; i++)
        {
            if (!delegateTimerList[i].isRelatedToTime)
            {
                delegateTimerList[i].timeToInvoke += abilityTime;
            }
        }
    }

    private class DelegateTimer
    {
        public float timeToInvoke;
        public MyDelegate delegateToInvoke;
        //public MyArgumentDelegate MyArgumentDelegate;
        public int repeat;
        public bool isRelatedToTime;

        public DelegateTimer(float timeOfInvo, MyDelegate del)
        {
            this.timeToInvoke = timeOfInvo;
            this.delegateToInvoke = del;

        }

        public DelegateTimer(float timeOfInvo, MyDelegate del, bool isRelatedToTime)
        {
            this.timeToInvoke = timeOfInvo;
            this.delegateToInvoke = del;
            this.isRelatedToTime = isRelatedToTime;
        }


        //public DelegateTimer(float timeOfInvo, MyArgumentDelegate myArgumentDelegate)
        //{
        //    this.timeToInvoke = timeOfInvo;
        //    this.MyArgumentDelegate = myArgumentDelegate;
        //}
        public DelegateTimer(float timeOfInvo, MyDelegate del, int repeat)
        {
            this.timeToInvoke = timeOfInvo;
            this.delegateToInvoke = del;
            this.repeat = repeat;
        }

    }
}
