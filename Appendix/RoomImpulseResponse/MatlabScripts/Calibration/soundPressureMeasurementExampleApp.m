function soundPressureMeasurementExampleApp(numTSteps)
%SOUNDPRESSUREMEASUREMENTEXAMPLEAPP splMeter demo application.
%   Demonstrates the use of splMeter in the octave-band modes.
%
%   soundPressureMeasurementExampleApp executes the SPL algorithm
%   in a loop for 'steps' iterations. The default is Inf (run until
%   the application window is closed).
%
%   See also splMeter.

% Copyright 2014-2018 The MathWorks, Inc.

%% Default value for input
if nargin < 1 || isempty(numTSteps)
    numTSteps = Inf;  % Run until the window is closed.
end

%% Setup Application Default Parameters

frameSize = 4096;   % Note: Increasing this may help on slow computers
defaultFS = 44100;  % The default sample rate used for pink noise and recording
inFs = defaultFS;   % Sample rate (default for the pink noise)

%% Setup System Objects

% Default source is pink noise
source = dsp.ColoredNoise('Color', 'pink', 'SamplesPerFrame', frameSize);

% Create the SPL Meter object
TimeInterval = 0.5;
TimeWeighting = 'Fast';
bandwidthText = {'1 octave', '2/3 octave', '1/3 octave'};
weightingText = {'Z-weighting', 'A-weighting', 'C-weighting'};
meter = splMeter('SampleRate',inFs,...
                 'Bandwidth',bandwidthText{1},...
                 'TimeWeighting',TimeWeighting,...
                 'TimeInterval',TimeInterval,...
                 'FrequencyWeighting',weightingText{1});

% Delay the SPL LT output slightly
delay = dsp.Delay(floor(TimeInterval*inFs/frameSize));

% Output to sound card
audioSink = audioDeviceWriter(inFs);
bufferAudio = true;

%% Create the Graphical User Interface (UI)

tuningUI = HelperCreateSoundPressureUI();
rmsPressureFiltText = findobj(tuningUI, 'Tag', 'StRmsPressureFilt');
fileNameText = findobj(tuningUI, 'Tag', 'StFullPathName');
c = onCleanup(@()clear('HelperSoundPressureUnpackUDP'));

%% Create the bar figure and position both windows

fh = figure('Toolbar','none','Menubar','none',...
            'Name','SPL Meter','NumberTitle','off');
colordef(fh, 'black');  % colordef this figure ONLY
p = tuningUI.Position;
p(1) = max(1, p(1)-p(3)/2-50);
tuningUI.Position = p;  % move the app left to make place for the figure
fh.Position = [p(1)+p(3)+3 p(2)-60 fh.Position(3)+120 fh.Position(4)+60];
bar(0);                 % create axes (ax)
ax = fh.Children;       % keep a handle to them
rebuildBars = true;     % we have to remake the bars
freq_text = [];         % hold handle to "Frequency (Hz)" label
oldFiltNum = 0;         % Old FiltNum and BW values (to check for changes)
oldBW = 0;

%% Streaming Loop

while numTSteps > 0
    numTSteps = numTSteps - 1; 
    
    % Obtain new values for parameters through UDP Receive
    [filtNum, bandwidth, changeFile, ...
        sourceType, audioMute] = HelperSoundPressureUnpackUDP(); 
    
    % Set the frequency weighting
    if filtNum ~= oldFiltNum
        oldFiltNum = filtNum;
        release(meter);
        release(delay);
        meter.FrequencyWeighting = weightingText{filtNum+1};
        rebuildBars = true; % the legend needs updating
    end
    
    % Set the octave bandwidth
    if bandwidth ~= oldBW
        oldBW = bandwidth;
        release(meter);
        release(delay);
        meter.Bandwidth = bandwidthText{bandwidth+1};
        rebuildBars = true; % the number of bars has changed
    end
    
    % Set the audio source
    if changeFile
        if isvalid(source)
            release(source); % Release the audio source
        end
        
        if sourceType == 2
            % Create an audio file reader with the filename
            fileName = get(fileNameText, 'String');
            source = dsp.AudioFileReader(fileName, 'PlayCount', Inf, 'SamplesPerFrame', frameSize);
            inFsNew = source.SampleRate; % Get the new sample rate
        elseif sourceType == 1
            % Read data from an external source (mic)
            inFsNew = defaultFS; % Set the sample rate to default
            source = audioDeviceReader('SamplesPerFrame', frameSize, 'SampleRate', defaultFS);
        else 
            % Create pink noise data
            source = dsp.ColoredNoise('Color', 'pink', 'SamplesPerFrame', frameSize);
            inFsNew = defaultFS; % Set the sample rate to default
        end
        
        % If the sample rate has changed, we need to update the objects
        if inFsNew ~= inFs
            inFs = inFsNew;
            rebuildBars = true;  % the number of bars may have changed
            release(delay)
            delay.Length = floor(TimeInterval*inFs/frameSize);
            release(meter);
            release(audioSink);
            meter.SampleRate = inFs;
            audioSink.SampleRate = inFs;
            bufferAudio = true;
        end
    end

    audio = source(); % Retrieve audio samples from the source
    audioMono = sum(audio,2)/size(audio,2); % create mono version
    if sourceType == 0  % clip and lower power of pink noise
        audioMono = min(1,max(-1,0.25*audioMono));
    end
    
    % Compute the weighted and unweighted SPL.
    [LT,Leq,Lpeak,Lmax] = meter(audioMono);
       
    % Update UI with the RMS pressure value
    LT_total = 10*log10(sum(10.^(LT(end,:)/10)));
    EQ_total = 10*log10(sum(10.^(Leq(end,:)/10)));
    if isvalid(rmsPressureFiltText)
        leveldesc = [meter.FrequencyWeighting(1) meter.TimeWeighting(1)];
        set(rmsPressureFiltText, 'String', ...
            sprintf('L%s : %.1f dB        L%ceq : %.1f dB', ...
            leveldesc, LT_total, meter.FrequencyWeighting(1), EQ_total));
    else % UI is closed, so quit
        break;
    end
    
    % Display input and filtered data spectrum
    b4 = max(0, Leq(end,:));
    b3 = max(0, delay(LT(end,:)));
    b2 = min(120,max(b3, Lmax(end,:)));
    b1 = min(120,max(b2, Lpeak(end,:)));
    block = 1.0*ones(1,size(b1,2)); % the small bars for peak and max display
    if rebuildBars
        rebuildBars = false;
        p1 = bar(ax, [b1; block]', 'stacked');
        p1(1).Visible = 'off';
        p1(2).FaceColor = [192 192 192]/255; % peak
        p1(2).EdgeColor = p1(2).FaceColor;
        hold(ax,'on');
        p2 = bar(ax,[b2; min(b2,block)]', 'stacked');
        p2(1).Visible = 'off';
        %p2(2).FaceColor = [255 51 0]/255; % max with fixed colors
        p2(2).FaceColor = 'flat'; % max with colors that depend on values
        p2(2).EdgeColor = p2(2).FaceColor;
        p2(2).CData = bsxfun(@times,[.2,1,0],ones(size(b3,2),3));
        p3 = bar(ax, b3');
        p3.FaceColor = [0 255 0]/255;
        p3.EdgeColor = p3.FaceColor;
        p4 = bar(ax, b4', 0.2, 'stacked');
        p4.FaceColor = [51 153 255]/255;
        p4.EdgeColor = p4.FaceColor;
        hold(ax,'off');
        xt = get(ax, 'XTick'); 
        if length(xt) > 12 % reduce the number of ticks in 2/3 octave mode
            xt = xt(1:2:end);
        end
        xt(1) = 1;
        ax.XTick = xt;
        cf = round(meter.getCenterFrequencies,2,'significant');
        ax.XTickLabel  = compose('%1.0f',cf(xt));
        ax.XAxisLocation = 'origin';
        ax.YLim = [-30 120];
        subzeroticks = cellfun(@str2num,ax.YTickLabel)<0;
        ax.YTickLabel{subzeroticks} = ''; % remove negative ticks
        prefix = ['L_{' meter.FrequencyWeighting(1) meter.TimeWeighting(1)];
        legend(ax, [p3(1) p4 p2(2) p1(2)], ...
               [prefix,'}'],[prefix,'eq}'],[prefix,'max}'],[prefix,'peak}'],...
               'Orientation', 'horizontal', 'Location', 'south');
        ylabel(ax, 'Power (dB)')
        % Put the xlabel at the bottom of the figure:
        if isempty(freq_text)
            freq_text = uicontrol(fh, 'Style','text', 'String', 'Frequency (Hz)', ...
                          'Units', 'normalized', 'Position', [0 0 1 0.08], ...
                          'FontUnits', ax.FontUnits, 'FontSize', ax.FontSize, ...
                          'ForegroundColor', ax.YColor, 'BackgroundColor', fh.Color); 
        end
        title(ax, 'Sound Pressure Meter', 'FontSize', 14, 'Color', ax.YColor)
        ba = ax.Children;
        drawnow limitrate;
    elseif isvalid(fh)
        ba(6).YData = b1;
        %ba(5).YData = block; % this value never changes
        ba(4).YData = b2;
        ba(3).YData = min(b2,block);
        ba(2).YData = b3;
        ba(1).YData = b4;
        mb = max(0,min(100,100-b2)); % max display with colors that depend on values
        ba(3).CData = hsv2rgb([3e-3*mb; ones(2,size(mb,2))].');  % one color per bar
        drawnow limitrate;
    else
        break; % Figure has been closed so quit
    end 

    % Playback the audio or silence
    % Do this at the end of the loop to minimize startup underruns
    if bufferAudio
        bufferAudio = false;
        audioSink(zeros(frameSize,1));
        audioSink(zeros(frameSize,1));
    end
    % Playing silence (or the audio input) is what keeps
    % the display from running faster than "real time"
    if audioMute
        audioSink(zeros(frameSize,1));
    else 
        audioSink(audioMono);
    end

end

%% Cleanup
% Close the interface, the input file, the audio input device, and release resources.

if isvalid(fh)
    close(fh)
end
if isvalid(tuningUI)
    close(tuningUI,'force');
end
if isvalid(source)
    release(source);
end
