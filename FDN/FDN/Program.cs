using System;
using System.IO;
using NAudio.Wave;
using System.Diagnostics;

class DelayLine
{
    private float[] buffer;
    private int index;

    public DelayLine(int length)
    {
        buffer = new float[length];
        index = 0;
    }

    public float Read()
    {
        return buffer[index];
    }

    public void Write(float input)
    {
        buffer[index] = input;
        index = (index + 1) % buffer.Length;
    }
}

class FeedbackDelayNetwork
{
    public static void Process(string inputFilePath, string outputFilePath, int[] delayLengths, float[,] feedbackMatrix, float[] b, float[] c, float mixRatio)
    {
        using (var inputFileReader = new AudioFileReader(inputFilePath))
        {
            int numChannels = inputFileReader.WaveFormat.Channels;

            var delayLines = new DelayLine[delayLengths.Length];
            for (int i = 0; i < delayLengths.Length; i++)
            {
                delayLines[i] = new DelayLine(delayLengths[i]);
            }

            using (var outputFileWriter = new WaveFileWriter(outputFilePath, inputFileReader.WaveFormat))
            {
                float[] inputBuffer = new float[numChannels];
                float[] outputBuffer = new float[numChannels];

                int numSamples = (int)(inputFileReader.Length / (numChannels * sizeof(float)));
                for (int n = 0; n < numSamples; n++)
                {
                    inputFileReader.Read(inputBuffer, 0, numChannels);

                    for (int ch = 0; ch < numChannels; ch++)
                    {
                        float inputSample = inputBuffer[ch];
                        float outputSample = 0.0f;
                        float[] feedback = new float[delayLines.Length];

                        for (int i = 0; i < delayLines.Length; i++)
                        {
                            float delayedSample = delayLines[i].Read();

                            for (int j = 0; j < delayLines.Length; j++)
                            {
                                feedback[j] += feedbackMatrix[j, i] * delayedSample;
                            }
                            outputSample += b[i] * delayedSample;
                        }

                        for (int i = 0; i < delayLines.Length; i++)
                        {
                            delayLines[i].Write(feedback[i] + inputSample * c[i]);
                        }

                        outputBuffer[ch] = (1 - mixRatio) * inputSample + mixRatio * outputSample;
                    }

                    outputFileWriter.WriteSamples(outputBuffer, 0, numChannels);
                }
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        string inputFilePath = "input.wav";
        string outputFilePath = "output.wav";

        int[] delayLengths = { 1133, 1277, 1493, 1583 };
        float[,] feedbackMatrix = {
            { 0.5f, 0.0f, 0.0f, 0.0f },
            { 0.0f, 0.5f, 0.0f, 0.0f },
            { 0.0f, 0.0f, 0.5f, 0.0f },
            { 0.0f, 0.0f, 0.0f, 0.5f }
        };
        float[] b = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] c = { 1.0f, 1.0f, 1.0f, 1.0f };
        float mixRatio = 0.3f;
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start(); // Start the stopwatch
        FeedbackDelayNetwork.Process(inputFilePath, outputFilePath, delayLengths, feedbackMatrix, b, c, mixRatio);
        stopwatch.Stop(); // Stop the stopwatch

        Console.WriteLine("Elapsed time: {0} ms", stopwatch.ElapsedMilliseconds);
    }
}


