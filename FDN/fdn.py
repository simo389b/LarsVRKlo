import numpy as np
import soundfile as sf

def feedback_delay_network(input_file, output_file, num_delay_lines, delay_lengths, feedback_matrix, b, c, mix_ratio):
    # Read input audio file
    audio_data, samplerate = sf.read(input_file)
    num_channels = audio_data.shape[1]
    
    # Initialize delay lines
    delay_lines = [np.zeros((length, num_channels)) for length in delay_lengths]
    
    # Initialize output buffer
    output_buffer = np.zeros_like(audio_data)
    
    # Process each sample
    for n in range(len(audio_data)):
        input_sample = audio_data[n]
        output_sample = np.zeros(num_channels)
        
        # Update delay lines and accumulate output samples
        for i in range(num_delay_lines):
            output_sample += b[i] * delay_lines[i][-1]  # Add the last sample in the delay line with gain b
            delay_lines[i] = np.roll(delay_lines[i], 1, axis=0)  # Shift delay line samples
            delay_lines[i][0] = c[i] * input_sample  # Insert the input sample at the beginning with gain c
            
            # Apply feedback matrix
            for j in range(num_delay_lines):
                delay_lines[i][0] += feedback_matrix[i][j] * delay_lines[j][-1]
        
        # Mix dry and wet signal
        output_buffer[n] = (1 - mix_ratio) * input_sample + mix_ratio * output_sample
    
    # Write output audio file
    sf.write(output_file, output_buffer, samplerate)

# Example usage
input_file = 'dabbror.wav'
output_file = 'output.wav'
num_delay_lines = 4
delay_lengths = [1133, 1277, 1493, 1583]  # Prime numbers for better results
feedback_matrix = np.array([
    [0.5, 0.0, 0.0, 0.0],
    [0.0, 0.5, 0.0, 0.0],
    [0.0, 0.0, 0.5, 0.0],
    [0.0, 0.0, 0.0, 0.5]
])
b = [1.0, 1.0, 1.0, 1.0]  # Gain coefficients b for each delay line
c = [1.0, 1.0, 1.0, 1.0]  # Gain coefficients c for each delay line
mix_ratio = 0.3

feedback_delay_network(input_file, output_file, num_delay_lines, delay_lengths, feedback_matrix, b, c, mix_ratio)
