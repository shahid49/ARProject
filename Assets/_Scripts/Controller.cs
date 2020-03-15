using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
public class Controller : MonoBehaviour
{

    public GameObject[] UIScreens;

    public static Controller Instance;
    public string[] namesList;
     public List<string> runTimeNameList = new List<string>();
    public string sName;
    float startTime, waitTime;
    [HideInInspector] public bool found;
    TextTracker textTracker;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        GetStudentNamesFromDatabase();

    }


    void Start()
    {
        textTracker = (TextTracker)TrackerManager.Instance.GetTracker<TextTracker>();
    }
    public void GetStudentNamesFromDatabase()
    {
        string url = "http://localhost/GetStudentList.php";


        WWWForm form = new WWWForm();
        WWW www = new WWW(url);
        StartCoroutine(waitForDBReport(www));
    }

    IEnumerator waitForDBReport(WWW www)
    {
        yield return www;
        namesList = www.text.Split(char.Parse(","));
        // check for errors
        if (www.error == null)
        {
            // Debug.Log("WWW Ok!: " + www.text);
            var vuforia = VuforiaARController.Instance;
            //vuforia.RegisterVuforiaStartedCallback(OnStarted);
        }
        else
        {
            //Debug.Log("WWW Error: " + www.error);
        }
    }


    bool c;
    string[] subName;
    int nameCounter = 0;

    public void foundName()
    {
        if(c == false)
        {
            c = true;
        }else
        {
            return;
        }
        Debug.Log("found");
        for (nameCounter = 0; nameCounter < namesList.Length - 1; nameCounter++)
        {

             subName = namesList[nameCounter].Split(char.Parse(" "));
            for (int j = 0; j < subName.Length; j++)
            {
                if (subName.Length == runTimeNameList.Count)
                {
                    match(subName);
                    break;
                }

            }
            


        }
    }
    
    void match(string[] sub)
    {
        Debug.Log("Match");
        for (int i = 0; i < sub.Length; i++)
        {
            if (runTimeNameList.Contains(sub[i]))
            {
                sName = namesList[nameCounter];
            }
        }
    }
    public void onClickStartBtn()
    {
        UIScreens[0].SetActive(false);
        UIScreens[1].SetActive(true);
        startTime = Time.time;
        waitTime = startTime + 10;

        InvokeRepeating("timeCheck", 1.0f, 1.0f);
    }
    void timeCheck()
    {

        if (waitTime < Time.time)
        {
            textTracker.Stop();

            UIScreens[2].SetActive(true);
        }
        if (found)
        {
            CancelInvoke();
        }
    }
    public void onClickReset()
    {
        textTracker.Start();
        UIScreens[2].SetActive(false);
        startTime = Time.time;
        waitTime = startTime + 10;
    }


    void nameCallforInfo()
    {

    }

   
    
}
