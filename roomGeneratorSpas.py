# -*- coding: utf-8 -*-
"""
Created on Wed Feb 15 12:10:36 2023

@author: jonas
"""

import numpy as np
import matplotlib.pyplot as plt
from numba import jit
from IPython import get_ipython
ipython = get_ipython()

def rectRoomGenerator(upperBounds, dataResolution):
    x = np.linspace(0,upperBounds[0],dataResolution)
    y = np.linspace(0,upperBounds[1],dataResolution)
    
    dim = np.transpose(np.array([x, y]))
    
    WX1 = np.array([1, 0])*dim
    WX2 = np.array([1, 0])*dim + [0, upperBounds[1]]
    WY1 = np.array([0, 1])*dim
    WY2 = np.array([0, 1])*dim + [upperBounds[0], 0]

    return WX1, WX2, WY1, WY2

def plotRoom(lines, SPoint, RPoint, MPoint):
    plt.plot(lines[0][:,0],lines[0][:,1])
    plt.plot(lines[1][:,0],lines[1][:,1])
    plt.plot(lines[2][:,0],lines[2][:,1])
    plt.plot(lines[3][:,0],lines[3][:,1]) 
    
    plt.plot(SPoint[0], SPoint[1], marker="o", markersize=10, markeredgecolor="green",
        markerfacecolor="green")
    plt.plot(RPoint[0], RPoint[1], marker="x", markersize=10, markeredgecolor="red",
        markerfacecolor="red")
    
    for i in range(len(MPoint[0])):
        plt.plot(MPoint[0][i], MPoint[1][i], marker="*", markersize=10)#, markeredgecolor="blue", markerfacecolor="blue")
    
    plt.show()

#@jit()
def findMirrorPoints(lines, SPoint, RPoint):
    mirrorX = [0, 0, 0, 0]
    mirrorY = [0, 0, 0, 0]
    for i in range(len(lines)):
        print('halløj')
        #while((mirrorY[i]-SPoint[1])/(mirrorX[i]-SPoint[0])!=(RPoint[1]-mirrorY[i])/(RPoint[0]-mirrorX[i])):
        while(((mirrorY[i]-SPoint[1])/(mirrorX[i]-SPoint[0]))-((RPoint[1]-mirrorY[i])/(RPoint[0]-mirrorX[i]))>0.0001):
            if (i == 0 and mirrorX[i]<np.max(lines[0][:,:])): #mangler at X ikke må gå over højeste dim
                mirrorY[i] = 0
                mirrorX[i] += 0.0001
                #print(mirrorX[i])
            elif (i == 1 and mirrorX[i]<np.max(lines[0][:,:])):
                mirrorY[i] = np.max(lines[2][:,:]) # Burde give max Y roomDim
                mirrorX[i] += 0.0001
            elif (i == 2 and mirrorY[i]<np.max(lines[2][:,:])):
                mirrorX[i] = 0
                mirrorY[i] += 0.0001
            elif (i == 3 and mirrorY[i]<np.max(lines[2][:,:])):
                mirrorX[i] = np.max(lines[0][:,:]) # Burde give max X roomDim
                mirrorY[i] += 0.0001
    return mirrorX, mirrorY
    
roomDim = [200, 200] # Corner points of rectangle
dataRes = 10000 # Number of points between points
S = [40, 50] # Source point
R = [90, 100] # Reciever point

roomLines = rectRoomGenerator(roomDim, dataRes) #Bottom X line, top X line, left Y line, right Y line

#ipython.magic("timeit findMirrorPoints(roomLines, S, R)")
mirrorPoints = findMirrorPoints(roomLines, S, R)

print(mirrorPoints)

plotRoom(roomLines, S, R, mirrorPoints)




