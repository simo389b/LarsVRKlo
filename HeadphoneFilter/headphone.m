close all; clear; clc;
list_factory = fieldnames(get(groot,'factory'));
index_interpreter = find(contains(list_factory,'Interpreter'));
for i = 1:length(index_interpreter)
    default_name = strrep(list_factory{index_interpreter(i)},'factory','default');
    set(groot, default_name,'latex');
end
clear list_factory index_interpreter default_name i;


% Settings
SETTINGS.titleFS    = 18;
SETTINGS.subtitleFS = 15;
SETTINGS.labelFS    = 12;


fname = 'fixedheadphone.json'; 
fid = fopen(fname); 
raw = fread(fid,inf); 
str = char(raw'); 
fclose(fid); 
val = jsondecode(str);



frekvens = val.data(:,1)';
frekvensNorm = frekvens/val.data(end,1);
frekvensNorm(1,1) = 0;
ampRight1 = val.data(:,3)'-90;
ampRight = ampRight1/20;
ampRight = 10.^(ampRight);
ampLeft1 = val.data(:,2)'-90;
ampLeft = ampLeft1/20;
ampLeft = 10.^(ampLeft)

Fs = 44100;

[bRight,aRight] = yulewalk(21, frekvensNorm, ampRight);
[bLeft, aLeft] = yulewalk(21, frekvensNorm, ampLeft);

[hRIIR wRIIR] = freqz(bRight,aRight,Fs);
[hLIIR wLIIR] = freqz(bLeft,aLeft,Fs);
%freqz(bRight,aRight,Fs)
%zplane(bRight, aRight);


f = figure('Position', [10 10 900 600]);
%s = semilogx(wR*(22050/pi),20*log10(hR), 'LineWidth', 2);
%s.Color = 'red';
hold on;
s = semilogx(wRIIR*(22050/pi),20*log10(hRIIR), 'LineWidth', 2);
s.Color = 'blue';
s = semilogx(frekvens, ampRight1, 'LineWidth', 2)
s.Color = 'green'; 
xlim([100 20000])
grid;
hold off;

f = figure('Position', [10 10 900 600]);
s = semilogx(wR*(22050/pi),20*log10(hR), 'LineWidth', 2);
s.Color = 'red';
hold on;
s = semilogx(wRIIR*(22050/pi),20*log10(hRIIR), 'LineWidth', 2);
s.Color = 'blue';
s = semilogx(frekvens, ampRight1, 'LineWidth', 2)
s.Color = 'green'; 
xlim([100 20000])
grid;
hold off;

%plot(val.data(:,1), val.data(:,2))
%plot(val.data(:,1), val.data(:,2))

f = figure('Position', [10 10 450 200])
p = semilogx(val.data(:,1), val.data(:,2:4), 'LineWidth', 2);
grid("on")
xlim([val.data(1,1) val.data(end,1)])
p(3).LineStyle = '--';
legend('Left', 'Right', 'Target Response', 'Location', 'southwest')
title('Frequency Response (Averaged and Compensated)', FontSize=SETTINGS.subtitleFS);
xlabel('Frequency [Hz]', 'FontSize', SETTINGS.labelFS);
ylabel('Amplitude [dB SPL]', 'FontSize', SETTINGS.labelFS);


exportgraphics(f, 'HeadphoneFrekvensRespons.pdf', 'Resolution', 200);

filterMatrix = [bRight; aRight];
writematrix(filterMatrix, 'Filters.txt')
