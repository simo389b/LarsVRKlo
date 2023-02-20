# -*- coding: utf-8 -*-
"""
Created on Wed Feb 15 12:10:36 2023

@author: Simon
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
    #plt.twinx()
    #plt.twiny()
    
    plt.plot(SPoint[0], SPoint[1], marker="o", markersize=10, markeredgecolor="green",
        markerfacecolor="green")
    plt.plot(RPoint[0], RPoint[1], marker="x", markersize=10, markeredgecolor="red",
        markerfacecolor="red")
    
    for i in range(len(MPoint[0])):
        plt.plot(MPoint[0][i,0], MPoint[0][i,1], marker="*", markersize=10)#, markeredgecolor="blue", markerfacecolor="blue")
    #plt.show()

#@jit()
def findMirrorPoints(lines, SPoint, RPoint):
    mirrorX = [0, 0, 0, 0]
    mirrorY = [0, 0, 0, 0]
    for i in range(len(lines)):
        print('halløj')
        while(((abs(mirrorY[i]-SPoint[1]))/(abs(mirrorX[i]-SPoint[0])))-((abs(RPoint[1]-mirrorY[i]))/(abs(RPoint[0]-mirrorX[i])))>0.1):
            if (i == 0 and mirrorX[i]<np.max(lines[0][:,:])): 
                mirrorY[i] = 0
                mirrorX[i] += 0.0001
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

def findMirrorPoints2(lines, SPoint, RPoint, dim):
    point1 = [0,0]
    point1[0] = ((SPoint[0]*RPoint[1])+(RPoint[0]*SPoint[1]))/(SPoint[1]+RPoint[1])
    
    point2 = [0,0]
    point2[1] = ((SPoint[0]*RPoint[1])+(RPoint[0]*SPoint[1]))/(SPoint[0]+RPoint[0])
    
    point3 = [0, dim[1]]
    point3[0] =  -((SPoint[0]*(dim[1]-RPoint[1]))+(RPoint[0]*(dim[1]-SPoint[1])))/(-2*dim[1]+(SPoint[1]+RPoint[1]))
    
    point4 = [dim[0], 0]
    point4[1] = -((dim[0]*(RPoint[1]+SPoint[1])-SPoint[0]*RPoint[1]-RPoint[0]*SPoint[1])/(-2*dim[0]+SPoint[0]+RPoint[0]))
    return point1, point2, point3, point4

def findMirrorPoints3(lines, SPoint, RPoint, dim, order):
    mirrorPointsXY = [0] * order # Tredje dimension er ordensindex 
    for i in range(order):
        if(i == 0):
            mirrorPointsXY[i] = np.zeros((len(lines)**(i+1),2)) # Antal vægge opløftet i reflektionsorden
            
            #Bundvæg
            mirrorPointsXY[i][0,0] = ((SPoint[0]*RPoint[1])+(RPoint[0]*SPoint[1]))/(SPoint[1]+RPoint[1])
            
            #Venstre væg
            mirrorPointsXY[i][1,1] = ((SPoint[0]*RPoint[1])+(RPoint[0]*SPoint[1]))/(SPoint[0]+RPoint[0])
            
            #Topvæg
            mirrorPointsXY[i][2,1] = dim[1]
            mirrorPointsXY[i][2,0] = -((SPoint[0]*(dim[1]-RPoint[1]))+(RPoint[0]*(dim[1]-SPoint[1])))/(-2*dim[1]+(SPoint[1]+RPoint[1]))
            
            #Højre væg
            mirrorPointsXY[i][3,0] = dim[0]
            mirrorPointsXY[i][3,1] = -((dim[0]*(RPoint[1]+SPoint[1])-SPoint[0]*RPoint[1]-RPoint[0]*SPoint[1])/(-2*dim[0]+SPoint[0]+RPoint[0]))
            
        if(i == 1):
            for j in range(len(lines)**order):
                mirrorPointsXY[i][j,0] = 100 + 50*np.random.rand([-j,j+1])
                mirrorPointsXY[i][j,1] = 100 + 50*np.random.rand([-j,j+1])
    return mirrorPointsXY

def findMirrorPoints4(lines, SPoint, RPoint, dim, order):
    mirrorPointsXY = [0] * order # Tredje dimension er ordensindex 
    for i in range(order):
        mirrorPointsXY[i] = np.zeros((len(lines)**(i+1),2))
        for j in range (len(lines)**(i+1)):
           
            #HØJRE VÆG!!
            if(j==0):
                #mirrorPointsXY[i][j,0] = ((i+1)*dim[0]+(i+1)%2*dim[0]+((-1)**(i+1)*SPoint[0]))
                mirrorPointsXY[i][j,0] = dim[0]*(i+1+(1+(-1)**i)/(2))+SPoint[0]*(-1)**(i+1)                    
                mirrorPointsXY[i][j,1] = SPoint[1]
                
            #ØVERSTE VÆG!!
            if(j==1):
                mirrorPointsXY[i][j,0] = SPoint[0]
                #mirrorPointsXY[i][j,1] = ((i+1)*dim[1]+(i+1)%2*dim[1]+((-1)**(i+1)*SPoint[1]))
                mirrorPointsXY[i][j,1] = dim[1]*(i+1+(1+(-1)**i)/(2))+SPoint[1]*(-1)**(i+1)
                
            #VENSTRE VÆG!!
            if(j==2):
                mirrorPointsXY[i][j,0] = -((i)*dim[0]+(i)%2*dim[0]+((-1)**(i)*SPoint[0]))
                mirrorPointsXY[i][j,1] = SPoint[1]
                
            #NEDERSTE VÆG!!
            if(j==3):
                mirrorPointsXY[i][j,0] = SPoint[0]
                mirrorPointsXY[i][j,1] = -((i)*dim[1]+(i)%2*dim[1]+((-1)**(i)*SPoint[1]))
                
            
    return mirrorPointsXY

roomDim = [200, 150] # Corner points of rectangle
dataRes = 10000 # Number of points between points
S = [70, 50] # Source point
R = [140, 110] # Reciever point
reflectionOrder = 1 # Number of reflections to be considered

roomLines = rectRoomGenerator(roomDim, dataRes) #Bottom X line, top X line, left Y line, right Y line

#ipython.magic("timeit findMirrorPoints(roomLines, S, R)")
mirrorPoints2 = findMirrorPoints3(roomLines, S, R, roomDim, reflectionOrder)
mirrorPoints = findMirrorPoints4(roomLines, S, R, roomDim, reflectionOrder)

print(mirrorPoints)

plotRoom(roomLines, S, R, mirrorPoints)
plotRoom(roomLines, S, R, mirrorPoints2)
plt.show()




