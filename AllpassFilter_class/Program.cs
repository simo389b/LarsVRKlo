using NAudio.Wave;

// Open the WAV file
var audioFile = new AudioFileReader("soundclips/dabbror.wav");

// Create an array to hold the sample values
float[] samples = new float[audioFile.Length / 4]; // assuming 16-bit audio

// Read the sample values into the array
int offset = 0;
while (audioFile.Position < audioFile.Length)
{
    int samplesRead = audioFile.Read(samples, offset, samples.Length - offset);
    if (samplesRead == 0) break;
    offset += samplesRead;
}

// Close the WAV file
audioFile.Close();


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
/*
float[] inputSignal = new float[256];
inputSignal[0] = 1.0f;

// Create an output signal as an array of the same length
float[] outputSignal = new float[256];
*/

// Create an array for output signal
float[] outputSignal = new float[samples.Length+20000];

// Artificial delay and gain
int predelay   = 1073;
float pregain = 2.7368f;
float[] predelayBuffer = new float[predelay + 1];
int predelayNewIndex = 0;
int predelayOldIndex = 1;

// Process each sample of the input signal through the filter    
for(int i = 0; i < outputSignal.Length; i += 2)
{
    if(i < samples.Length)
    {
        predelayBuffer[predelayNewIndex] = pregain * samples[i];
    }
    else{
        predelayBuffer[predelayNewIndex] = 0;
    }
    // predelayBuffer[predelayNewIndex] = pregain * samples[i];
    outputSignal[i] = filter1.ProcessSample(predelayBuffer[predelayOldIndex]);
    outputSignal[i] = filter2.ProcessSample(outputSignal[i]);
    outputSignal[i] = filter3.ProcessSample(outputSignal[i]);
    outputSignal[i]   = filter4.ProcessSample(outputSignal[i]);
    // outputSignal[i+1] = filter4.ProcessSample(outputSignal[i]);
    
    predelayNewIndex = (predelayNewIndex + 1) % predelayBuffer.Length;
    predelayOldIndex = (predelayOldIndex + 1) % predelayBuffer.Length;

    // Print the input and output values to the console
    string inputString = default!;
    if(i < samples.Length)
    {
        inputString = samples[i].ToString().Replace(",", ".");
    }
    else
    {
        inputString = "0";
    }
     
    string outputString = outputSignal[i].ToString().Replace(",", ".");
    AllpassFilter.WriteString(inputString.Replace(",", ".") + ", " + outputString.Replace(",", "."));
}