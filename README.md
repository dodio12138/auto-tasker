# auto-tasker

## 📢 Introduction

This is an auto-click application based on the `WFP/.NET Framework 4.8`, currently running on Windows only.

## ✨ Features
- Support for multi-step tasks.
- Mouse coordinate display.
- A cute little bunny in the bottom right corner. Her name is Godzilla.

## 🐢 Tutorial
1. Enter the coordinates of the mouse and the delay time of the click in the text box. 
    - The current mouse position is shown in the lower left corner.
    - The command format for each step is: `mouseX.mouseY.delayTime(ms)`.
    - Each step is split by ending with `;`.
    - Example: `400.500.3000;600.700.5000;`
2. Set the loop time, when the value is -1 is infinite loop.
3. Click the left button to run the task and the right button to interrupt the run.
