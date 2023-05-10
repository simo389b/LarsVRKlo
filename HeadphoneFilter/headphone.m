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
