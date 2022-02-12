using UnityEngine;

public class UMainPanel : MonoBehaviour
{
	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void OnClicked(string game)
    {
        AudioMgr.Instance.PlaySound("/Prefabs/Audio/ui_button_clicked.prefab");

        if (game != "tps")
        {
            Message msg = new Message("CHANGE_SCENE");
            msg.AddArg("Scene", game);

            MessageMgr.Instance.SendMessage(msg);
        }
        else
        {
            Debug.Log("implement at later.");
        }
    }
}
