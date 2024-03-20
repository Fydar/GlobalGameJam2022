using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioStatsUpdater : MonoBehaviour
{
    [SerializeField]
    private Text _bloodlustText;

    [SerializeField]
    private Text _timeOfDayText;
    
    public float bloodlastValue = -1.0f;
    public enum TimeOfDayTypes { Day = 0, Night = 1 };
    public TimeOfDayTypes timeOfDay;

    
    void Start(){
    
    }


    public void updateBloodlustValue(float value){
        //update value
        bloodlastValue = value;

        //print value
        switch (bloodlastValue){
            case 2:
                 _bloodlustText.text  = "Bloodlust: Killing Time!! ("+bloodlastValue+")";
                break;
            case 1:
                 _bloodlustText.text  = "Bloodlust: Started! ("+bloodlastValue+")";
                break;
            case 0:
                 _bloodlustText.text  = "Bloodlust: Rising ("+bloodlastValue+")";
                break;
            default: //-1
                _bloodlustText.text  = "Bloodlust: Gone ("+bloodlastValue+")";
                break;
        }

        //send global paramter to FMOD
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Bloodlust", bloodlastValue);
    }


    public void updateTimeOfDay(int value){
        //update value
        timeOfDay = (TimeOfDayTypes) value;

        //print value
        _timeOfDayText.text  = "Time Of Day: "+timeOfDay;

        //send global parameter to FMOD
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("TimeOfDay", value);
    }




}
