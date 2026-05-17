using UnityEngine;
using UnityEngine.Events;

public class NOTGate : MonoBehaviour, ILogicInput
{
    public MonoBehaviour inputSource;
    public UnityEvent onOutputTrue;
    public UnityEvent onOutputFalse;

    private bool previousOutput;

    void Start()
    {
        // Force a mismatch so Update() fires the correct event on the first frame
        bool realOutput = IsOn;
        previousOutput = !realOutput;
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

    public bool IsOn
    {
        get
        {
            var input = inputSource as ILogicInput;
            return input == null ? false : !input.IsOn;
        }
    }
}