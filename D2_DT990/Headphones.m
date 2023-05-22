close all; clear; clc;
list_factory = fieldnames(get(groot,'factory'));
index_interpreter = find(contains(list_factory,'Interpreter'));
for i = 1:length(index_interpreter)
    default_name = strrep(list_factory{index_interpreter(i)},'factory','default');
    set(groot, default_name,'latex');
end
clear list_factory index_interpreter default_name i;

SETTINGS.titleFS    = 18;
SETTINGS.subtitleFS = 15;
SETTINGS.labelFS    = 12;

beta = 0.07;
A = [ones(10,1); zeros(1100,1); ones(938, 1)];
A2 = [ones(10,1); zeros(900,1); ones(1138, 1)];
%A = [ones(2048,1)];
N1 = 1;
N2 = 10000;

IR = audioread("D2_44K_16bit_1024tap_FIR_HpIR.wav");
EQ = audioread("D2_44K_16bit_1024tap_FIR_HpEQ.wav");
EQl = EQ(:,1);
IRl = [IR(:,1)];
IRr = IR(:,2);
[hIRL, wIRL] = freqz(IRl,1,2048);
%[hIRR, wIRR] = freqz(IRr,1,2048);
[hEQL, wEQL] = freqz(IRr,1,2048);
abso = abs(hEQL');
abso2 = abs(hIRL')
absoinv = abso.^(-1);
absoinv2 = abso2.^(-1);

%zplane(IRl)
%Minl = isminphase(IRl)
%Minr = isminphase(IRr)

frekvensVektor = wEQL*22050*pi;
FreqNormal = wEQL/pi;
FreqNormal(1,1) = 0;
FreqNormal(2048,1) = 1;
delay1 = zeros(2048,1);
delay2 = zeros(2048,1);

for i = 1:length(frekvensVektor)
    delay1(i,1) = exp(-(1i)*frekvensVektor(i,1)*N1);
    delay2(i,1) = exp(-(1i)*frekvensVektor(i,1)*N1);
end

FreqLog = logspace(-4, 0, 2047);
FreqLog = [0 FreqLog];


H1 = ((conj(hEQL)).*delay1).*(1./((abso.^2)+beta*(A.^2)')');
H2 = ((conj(hIRL)).*delay2).*(1./((abso2.^2)+beta*(A2.^2)')');
[br, ar] = yulewalk(20, FreqNormal, abs(H1));
[bl, al] = yulewalk(20, FreqNormal, abs(H2));
[hFiltr, wFiltr] = freqz(br,ar,22050);
[hFiltl, wFiltl] = freqz(bl,al,22050);
writematrix([br; ar], 'RightFilter');
writematrix([bl; al], 'Leftfilter');



Kris = hEQL'.*H1';

%figure();
%plot(EQl);
f = figure('Position', [10 10 900 300]);
tiledlayout(2,1);
nexttile;
%semilogx(wEQL*(22050/pi), 20*log10(abs(hEQL)), 'LineWidth', 2);
semilogx(wEQL*(22050/pi), 20*log10(abs(H1)), 'LineWidth',2);
grid;
hold on;

%semilogx(wEQL*(22050/pi), 20*log10(abs(A)), 'LineWidth',2);

%semilogx(wFiltr*(22050/pi), 20*log10(abs(hFiltr)), 'LineWidth',2);
%semilogx(wEQL*(22050/pi), 20*log10(abs(H2)), 'LineWidth',2);
semilogx(wFiltr*(22050/pi), 20*log10(hFiltr), 'LineWidth',2);
xlim([20 20000]);
ylabel('Magnitude [dB]')
xlabel('Frequency [Hz]')
title('Right Ear IIR-filter for $N = 21$','Interpreter','latex')
legend('Inverse with Regularisation', 'IIR-Filter' ,'Location', 'southwest')
nexttile;

semilogx(wFiltr*(22050/pi), 360/(2*pi)*(angle(hFiltr)), 'LineWidth', 2);
xlim([20 20000])
%semilogx(wEQL*(22050/pi), 360/(2*pi)*(angle(H1)), 'LineWidth', 2);
grid;
ylabel('Phase [$^\circ$]', 'Interpreter','latex')
xlabel('Frequency [Hz]')
legend('IIR-Filter Phase Response', 'Location', 'southwest')

hold off;
f2 = figure('Position', [10 10 900 300]);
tiledlayout(2,1);
nexttile;
semilogx(wFiltl*(22050/pi), 20*log10(hFiltl), 'LineWidth',2);
hold on;
semilogx(wEQL*(22050/pi), 20*log10(abs(H2)), 'LineWidth',2);
%ylim([-0.1 1.1]);
xlim([20 20000]);
xlabel('Frequency [Hz]')
ylabel('Magnitude [dB]')
title('Left Ear IIR-filter for $N=$','Interpreter','latex');
legend('Inverse with Regularisation', 'Direct Inverse' ,'Location', 'southwest')
grid
nexttile;
semilogx(wFiltr*(22050/pi), 360/(2*pi)*(angle(hFiltr)), 'LineWidth', 2);
xlim([20 20000]);
grid;
ylabel('Phase [$^\circ$]', 'Interpreter','latex')
xlabel('Frequency [Hz]')
legend('IIR-Filter Phase Response', 'Location', 'southwest')
exportgraphics(f, 'RightEarIIR20.pdf')
exportgraphics(f2, 'LeftEarIIR20.pdf')