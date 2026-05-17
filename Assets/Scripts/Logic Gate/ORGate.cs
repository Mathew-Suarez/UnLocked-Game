    using UnityEngine;
using UnityEngine.Events;

public class ORGate : MonoBehaviour, ILogicInput
{
    public MonoBehaviour[] inputSources;
    public UnityEvent onOutputTrue;
    public UnityEvent onOutputFalse;

    private bool previousOutput = false;
    void Start()
    {
        bool realOutput = IsOn;
        previousOutput = !realOutput;
    }
    public bool IsOn
    {
        get
        {
            foreach (var src in inputSources)
            {
                var input = src as ILogicInput;
                if (input != null && input.IsOn)
                    return true;
            }
            return false;
        }
    }

    void Update()
    {
        bool current = IsOn;
        if (current != previousOutput)
        {
            previousOutput = current;
            if (current) onOutputTrue.Invoke();
            else onOutputFalse.Invoke();
        }
    }
}