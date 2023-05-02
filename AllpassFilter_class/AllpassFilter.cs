public class AllpassFilter
{
    private float g;
    private float g2;
    private float[] internalBuffer;
    private float[] inputBuffer;
    private int oldIndex;
    private int newIndex;
    private int delay;

    public AllpassFilter(int delay, float gain)
    {
        g = gain;                               // g
        this.delay = delay;                     // d
        g2 = (1.0f - g * g);                     // 1 - g^2
        internalBuffer = new float[delay+1];   // buffer of size N
        inputBuffer    = new float[delay+1];   // buffer of size N
        newIndex = 0;                           // buffer index for new samples
        oldIndex = 1;                           // buffer index for delayed samples
    }

    public float ProcessSample(float input)
    {
        // calculate output sample, allpass
        inputBuffer[newIndex] = input;                                                      // x(n)
        internalBuffer[newIndex] = inputBuffer[oldIndex] + g * internalBuffer[oldIndex];    // y(n) = x(n) + g * y(n - M)
        float output    = -g * inputBuffer[newIndex] + g2 * internalBuffer[newIndex];      // x(n) = -g * x(n) + y(n - M)

        // increment and wrap buffer indices
        newIndex = (newIndex + 1) % internalBuffer.Length;
        oldIndex = (oldIndex + 1) % internalBuffer.Length;

        // return the output sample
        return output;
    }

    public void Reset()
    {
        Array.Clear(internalBuffer, 0, internalBuffer.Length);
        Array.Clear(inputBuffer, 0, inputBuffer.Length);
        newIndex = 0;
        oldIndex = (internalBuffer.Length - delay) % internalBuffer.Length;
    }

    public static void WriteString(string s)
    {
        string path = "Data/out.txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(s);
        writer.Close();
    }
}
