// Create a new allpass filter with a delay of 0.5 samples and a gain of 0.5
AllpassFilter filter1 = new AllpassFilter(1176, 0.7f);
AllpassFilter filter2 = new AllpassFilter(1001, 0.7f);
AllpassFilter filter3 = new AllpassFilter(529, 0.7f);
AllpassFilter filter4 = new AllpassFilter(358, 0.7f);
filter1.Reset();
filter2.Reset();
filter3.Reset();
filter4.Reset();

// Create an impulse
float[] inputSignal = new float[256];
inputSignal[0] = 1.0f;

// Create an output signal as an array of the same length
float[] outputSignal = new float[256];

// Artificial delay and gain
int predelay   = 1073;
float pregain = 2.7368f;
float[] predelayBuffer = new float[predelay + 1];
int predelayNewIndex = 0;
int predelayOldIndex = 1;

// Process each sample of the input signal through the filter
for (int j = 0; j < 69; j++)
{
    if(j != 0)
    {
        inputSignal = new float[256];
    }
    for(int i = 0; i < inputSignal.Length; i++)
    {
        predelayBuffer[predelayNewIndex] = pregain * inputSignal[i];
        outputSignal[i] = filter1.ProcessSample(predelayBuffer[predelayOldIndex]);
        outputSignal[i] = filter2.ProcessSample(outputSignal[i]);
        outputSignal[i] = filter3.ProcessSample(outputSignal[i]);
        outputSignal[i] = filter4.ProcessSample(outputSignal[i]);
        
        predelayNewIndex = (predelayNewIndex + 1) % predelayBuffer.Length;
        predelayOldIndex = (predelayOldIndex + 1) % predelayBuffer.Length;

        // Print the input and output values to the console
        string inputString = inputSignal[i].ToString().Replace(",", ".");
        string outputString = outputSignal[i].ToString().Replace(",", ".");
        AllpassFilter.WriteString(inputString.Replace(",", ".") + ", " + outputString.Replace(",", "."));
    }
}
