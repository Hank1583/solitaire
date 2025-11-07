using UnityEngine;
public class Sound : MonoBehaviour
{
//	private static AudioSource music;
	private static AudioSource sound;
	private static Sound _instance = null;
	public static Sound Instance
	{
		get 
		{
			if (_instance == null)
			{
				GameObject soundInstance = (GameObject) Instantiate(Resources.Load("SoundSystem"));
				soundInstance.name = "SoundSystem";
//				music = GameObject.Find ("Music").GetComponent<AudioSource> ();
				sound = GameObject.Find ("SoundFX").GetComponent<AudioSource> ();
				_instance = soundInstance.GetComponent<Sound>();
			}
			return _instance;
		}
	}
	#region toPlay




	public void Shift()
	{/*
		int index = Random.Range (0, SoundSettings.Instance.shift.Length);
		sound.clip = SoundSettings.Instance.shift[index];
		sound.Play ();
        */
	}
	public void Foley()
	{
        /*
		int index = Random.Range (0, SoundSettings.Instance.foley.Length);
		sound.clip = SoundSettings.Instance.foley[index];
		sound.Play ();
        */
	}
	public void Creack()
	{
        /*
		int index = Random.Range (0, SoundSettings.Instance.creack.Length);
		sound.clip = SoundSettings.Instance.creack[index];
		sound.Play ();
        */
	}
	public void Up()
	{
        if (!GameSettings.Instance.isSoundSet) return;
        int index = Random.Range (0, SoundSettings.Instance.up.Length);
		sound.clip = SoundSettings.Instance.up[index];
		sound.Play ();
        
	}
	public void Down()
    {
        if (!GameSettings.Instance.isSoundSet) return;

        int index = Random.Range (0, SoundSettings.Instance.down.Length);
		sound.clip = SoundSettings.Instance.down[index];
		sound.Play ();
       
	}
	public void Error()
	{
        /*
		int index = Random.Range (0, SoundSettings.Instance.error.Length);
		sound.clip = SoundSettings.Instance.error[index];
		sound.Play ();
        */
	}
	public void Win()
    {
        if (!GameSettings.Instance.isSoundSet) return;
        int index = Random.Range (0, SoundSettings.Instance.win.Length);
		sound.clip = SoundSettings.Instance.win[index];
		sound.Play ();
	}
	public void Claps()
    {
        if (!GameSettings.Instance.isSoundSet) return;
        int index = Random.Range (0, SoundSettings.Instance.claps.Length);
		sound.clip = SoundSettings.Instance.claps[index];
		sound.Play ();
	}

    public void TouchCard()
    {
        if (!GameSettings.Instance.isSoundSet) return;
        sound.clip = SoundSettings.Instance.touchCard;
        sound.Play();
    }

    public void MissCard()
    {
        if (!GameSettings.Instance.isSoundSet) return;
        sound.clip = SoundSettings.Instance.missCard;
        sound.Play();
    }
    public void FoundRowSpider()
    {
        if (!GameSettings.Instance.isSoundSet) return;
    
        sound.clip = SoundSettings.Instance.destroyKCard;
        sound.Play();
    }




    public void HintCard()
    {
        if (!GameSettings.Instance.isSoundSet) return;
        sound.clip = SoundSettings.Instance.hintCard;
        sound.Play();
    }

    public void UndoCard()
    {
        if (!GameSettings.Instance.isSoundSet) return;
        sound.clip = SoundSettings.Instance.undoCard;
        sound.Play();
    }

    

  

    public void StartNew()
    {
        if (!GameSettings.Instance.isSoundSet) return;
        sound.clip = SoundSettings.Instance.startNew;
        sound.Play();
    }

    public void ButtonClick()
    {
        if (!GameSettings.Instance.isSoundSet) return;
        sound.clip = SoundSettings.Instance.buttonClick;
        sound.Play();
    }
    #endregion
}