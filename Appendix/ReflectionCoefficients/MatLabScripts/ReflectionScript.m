close all; clear; clc;
%% Setup
fs = 48000; % Hz

%% Audio Recorder Setup
recorder = audiorecorder(fs, 24, 1);


recDuration = 3;
t = (0:1/fs:recDuration)';
testSignal = 0.5*sin(440*2*pi*t);

sound(testSignal, fs);
recordblocking(recorder, recDuration);
a = recorder.getaudiodata;

%%
figure('Position', [10 10 900 300]);
plot(t, testSignal, t(2:end), a, 'LineWidth', 1);
grid on
% xlim([0 0.05])

%% MLS signal setup



%% Dependencies
[fList,pList] = matlab.codetools.requiredFilesAndProducts('ReflectionScript.m');

%% Functions
%function PlayAndRecord(playSignal, )