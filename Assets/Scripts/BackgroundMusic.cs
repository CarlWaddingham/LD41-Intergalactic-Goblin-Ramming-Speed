using UnityEngine;
using System.Collections;
	/// </summary>
public class BackgroundMusic : MonoBehaviour
{
	/// the background music
	public AudioClip SoundClip ;

	protected AudioSource _source;

	/// <summary>
	/// Gets the AudioSource associated to that GameObject, and asks the GameManager to play it.
	/// </summary>
	protected virtual void Start () 
	{
		_source = gameObject.AddComponent<AudioSource>() as AudioSource;	
		_source.playOnAwake=false;
		_source.spatialBlend=0;
		_source.rolloffMode = AudioRolloffMode.Logarithmic;
		_source.loop=true;	
		
		_source.clip=SoundClip;
			
		SoundManager.instance.PlayBackgroundMusic(_source);
	}
}