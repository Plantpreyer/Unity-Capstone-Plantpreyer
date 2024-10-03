using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class Survey : MonoBehaviour
{
    public TMP_InputField userNameField;

    public TMP_InputField q1field;

    public TMP_InputField q2field;

    public TMP_InputField q3field;

    public TMP_InputField q4field;

    public TMP_InputField q5field;

    public TMP_Dropdown q1drop;

    public TMP_Dropdown q3drop;

    public TMP_Dropdown q4drop;

    public TMP_Dropdown q5drop;

    private string userName;

    private string q1;
    private string q12;

    private string q2;

    private string q3;
    private string q32;

    private string q4;
    private string q42;

    private string q5;
    private string q52;

    private string aName = "entry.314671095";

    private string aGroup = "entry.1332755613";
    private string acGroup = "entry.2075826188";

    private string a1 = "entry.988632581";
    private string a12 = "entry.1290659844";

    private string a2 = "entry.952113751";
    private string a3 = "entry.2060476939";
    private string a32 = "entry.1802098856";

    private string a4 = "entry.238660632";
    private string a42 = "entry.838083524";
    private string a5 = "entry.139449682";
    private string a52 = "entry.648336937";

    private string aTotalCount = "entry.1737687526";
    private string acTotalCount = "entry.523639930";
    private string aForestCount = "entry.1327873366";
    private string acForestCount = "entry.693232485";

    private string URL;

    private bool posted = false;

    public void Start()
    {
        if (GameObject.Find("UICanvas"))
            Destroy(GameObject.Find("UICanvas"));

        if (SolvedGames.Instance.IsExperimental())
        {
            URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSe_nauTruqdGJN8_di7ZdU-8-FcKldYr-8DU8J5rdxEiBMhAA/formResponse";
        }
        else
        {
            URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfkORShDNsGWL6dJ3hrEB5Kohyp6ZSD9bkUVzWgVkObfle-iQ/formResponse";
        }
    }


    public void Send()
    {
        if(posted){
            return;
        }
        posted = true;

        userName = userNameField.text;

        q1 = q1field.text;

        q12 = getDropText(q1drop);

        q2 = q2field.text;

        q3 = q3field.text;

        q32 = getDropText(q3drop);

        q4 = q4field.text;

        q42 = getDropText(q4drop);

        q5 = q5field.text;

        q52 = getDropText(q5drop);

        StartCoroutine(Post());


    }

    public void QuitGame(){
        if(!posted){
            return;
        }

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    IEnumerator Post()
    {
        
        WWWForm form = new WWWForm();
        form.AddField(aName, userName);

        form.AddField(a1, q1);

        form.AddField(a12, q12);

        form.AddField(a2, q2);

        form.AddField(a3, q3);

        form.AddField(a32, q32);

        form.AddField(a4, q4);

        form.AddField(a42, q42);

        form.AddField(a5, q5);

        form.AddField(a52, q52);

        if (SolvedGames.Instance.IsExperimental())
        {
            form.AddField(aTotalCount, SolvedGames.Instance.GetTotal());
            form.AddField(aForestCount, SolvedGames.Instance.GetForest());
            form.AddField(aGroup, "Experimental");
        }
        else
        {
            form.AddField(acTotalCount, SolvedGames.Instance.GetTotal());
            form.AddField(acForestCount, SolvedGames.Instance.GetForest());
            form.AddField(acGroup, "Control");
        }
        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        yield return www.SendWebRequest();
    }

    private string getDropText(TMP_Dropdown q)
    {
        return q.options[q.value].text;
    }


}