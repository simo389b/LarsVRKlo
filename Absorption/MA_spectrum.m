clear

% Define the model order p
M=16;

% Define the number of samples in the High-Order AR model.
L=200;

% Load the data file
amplitudes = readmatrix("tabledata.txt")
% ...and move it to tha variable amp
amp = amplitudes;

% First we mirror the spectrum around pi -- it must exist from 0 to 2*pi
for i=0:1023
    amp(1025+i) = amplitudes(1024-i);
end


% Make a frequency scale from 0 to pi
delta_freq = pi/1024;
freq_axis(1)=0;
for i=2:1024
    freq_axis(i) = freq_axis(i-1) + delta_freq;
end

% Make a frequency scale from 0 to 2*pi
delta_freq = (2*pi)/2048;
freq_axis_2pi(1)=0;
for i=2:2048
    freq_axis_2pi(i) = freq_axis_2pi(i-1) + delta_freq;
end

%plot(freq_axis_2pi,amp)
%axis([0 6.5 0.7 1.3]);

% Calculate the corresponding impuls response sequence
h = real(ifft(amp));
%stem(h)

% Since the spectrum is "zero-phase" and symmetric around pi, then the
% time domain representation, i.e., the impulse response a_L is also
% symmetric, and it represents one period of the periodic time domain
% signal (remember that the spectrum is discrete in frequency).
% Now, due to the "zero-phase" characteristic of the spectrum, the impulse
% response is "non-causal", i.e., one period should be represented from
% -1024 to +1024. Therefore, we next mirrow the response such that it
% becomes "symmetric around zero".
h_sym(1:1024) = h(1025:2048);
h_sym(1025:2048) = h(1:1024);
for i=-24:24
    stem_axis_h(25+i) = i-1;
end
%stem(stem_axis_h,h_sym(1000:1048))


% Next, calculate the autocorrelation of the impulse response -- using all
% 2048 impulse response samples. Applying the xcorr M-function creates 4095
% autocorrelation coefficients.
r_h = xcorr(h_sym);
for i=-20:20
    stem_axis_r_h(21+i) = 2048+i;
end
%stem(stem_axis_r_h, r_h(2028:2068))

% Based on the autocorr.-coefficients, the autocorr.-matrix and vector are
% next generatd
R_h = toeplitz(r_h(2048:2048+L-1));
b_h = r_h(2049:2049+L-1);

% Solving the equation system provides L samples of the impulse response
% from the High-Order AR model of the desired filter. This impulse response
% is next considered as the observable signal.
a_h = -inv(R_h)*b_h';
%plot([1 a_h(1:50)'])

% Using now the inpulse response as the observable signal, we now calculate
% M MA filter coefficients using the usual autocorrelation method. Remember
% that the first sample in the impulse response equals 1 (the coefficient
% a_0).
r_a = xcorr([1 a_h']');

% As usual, the autocorr. matrix and vector are created based on the
% autocorrelation coefficients.
R_a = toeplitz(r_a(L+1:L+M));
b_a = r_a(L+2:L+M+1);

% Solving the equation system provides the L MA coefficients
a_a = -inv(R_a)*b_a;

% Now we need to calculate the gain factor for 0dB DC gain
G_a = 1;
for i=1:M
    G_a = G_a + a_a(i);
end
G_a = 1/G_a;

% ...and the finally, calculate and plot the approximated MA and the
% desired amplitude1Total spectrum
[H_a,w] = freqz([1 a_a'],1, 1024);
plot(freq_axis, amplitudes(1:1024), freq_axis, G_a*abs(H_a));
%axis([0 3.3 0.7 1.3]);
