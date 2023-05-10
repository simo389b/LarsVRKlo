function [filtNum,bandwidth,changeFile,audioSource,audioMute] = ...
    HelperSoundPressureUnpackUDP()
% Function to create a UI to tune the parameters of algorithm components.
% Output:
%         filtNum     - Variable to store the filter in use. Can take
%                       values 0 (noise), 1 (mic) or 2 (audio file).
%         bandwidth   - Variable to store the bandwith in use. Can be
%                       0 (1 octave), 1 (2/3 octave) or 2 (1/3 octave).
%         changeFile  - Flag to change the audio source. If true, the audio
%                       source configuration will change.
%         audioSource - Variable to store the audio source in use. Can take
%                       values from 0 to 2.
%         audioMute   - Audio mute checkbox. Value can be 0 or 1.
%     
% This function HelperSoundPressureUnpackUDP is only in support of
% SoundPressureMeasurementExample. It may change in a future release.
%

%   Copyright 2014-2018 The MathWorks, Inc.

persistent UDPReceive prevFiltNum prevBW prevAudioSource prevMute

if isempty(UDPReceive)
    UDPReceive = dsp.UDPReceiver('MessageDataType','double','ReceiveBufferSize',200);
    prevAudioSource = 0;
    prevFiltNum = 0;
    prevMute = 1;
    prevBW = 0;
end

packetUDP = UDPReceive();

if isempty(packetUDP)
    % The received UDP packet doesn't come from the UI.
    % The configuration should not change.
    changeFile = false;
    bandwidth = prevBW;
    filtNum = prevFiltNum;
    audioSource = prevAudioSource;
    audioMute = prevMute;
else
    % The received UDP packet comes from the UI.
    % There is a change in the configuration.
    filtNum = packetUDP(1);
    bandwidth = packetUDP(2);
    changeFile = packetUDP(3);
    audioSource = packetUDP(4);
    audioMute = packetUDP(5);
    prevFiltNum = filtNum;
    prevAudioSource = audioSource;
    prevMute = audioMute;
    prevBW = bandwidth;
end

end
