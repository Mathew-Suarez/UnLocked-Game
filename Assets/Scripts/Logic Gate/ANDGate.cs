using UnityEngine;
using UnityEngine.Events;

public class ANDGate : MonoBehaviour, ILogicInput
{
    public MonoBehaviour[] inputSources;   // Drag GameObjects with ILogicInput
    public UnityEvent onOutputTrue;
    public UnityEvent onOutputFalse;

    private bool previousOutput = false;

    // ILogicInput – let other gates treat this gate as an input
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
                if (input == null || !input.IsOn)
                    return false;
            }
            return inputSources.Length > 0;
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