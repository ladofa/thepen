# ThePen
ThePen is a software for drawing on screen in Windows. It started just for my online lectures. I don't have any specific plan to push this work. But I really hope it used widely and be helpful for you.

## Download the Latest Version
 * Alpha12 : https://github.com/ladofa/thepen/releases/download/%23thepenalpha/ThePen-alpha12.zip
 * It is not allowed by Windows SmartScreen. It is **not officially authorized** for Windows yet. You have to check the advanced option.
  - Virus warning can be shown
    
<img src="images/w1.jpg" width="40%" height="40%">
  - For installation, you need to select advanced option
<img src="images/w2.jpg" width="40%" height="40%">
<img src="images/w3.jpg" width="40%" height="40%">



## Important Features
 * One Key Sourtcut : Push only one keyboard button to change your pen type and color.
   
 ![alt text](images/onekey.jpg "Title")
 * Easy Switch : Just by lifting the stylus pen, it switches to drawing mode. And just move your mouse to escape drawing mode.
   - It doesn't work on some applications(Chrome, Edge, VSCode, etc.). In this case, move your pen to edges of the monitor to switch mode.
     
   ![alt text](images/edgearea.jpg "Title")
 * Shaking gesture to clear all drawing.
 * Shapes : Line, Circle, Rectangle, Grid (One Key Shortcut Only)
 * Stamp : Just push a key to put your stamps. (One Key Shortcut Only)
 * Arrow : Add arrow on free drawings or any shapes.
 * Overlay - Display current time, text messages and images. (It is not in maintenance yet)
 * Background and overlay files (`back1.png`, `overlay1.txt`, ...) are read from `Documents\ThePen`, or from the program folder.
 * Right click and move to erase.
 * Color palette swap (shortcut : number key 7(&) )
 * Multi Board (shortcut : F1, F2, F3)

## Reference
 * Epic Pen : https://epic-pen.com/
 * Panser Pen : https://kinfolksoft.com/%EB%B8%8C%EB%A6%AC%ED%95%91%ED%8E%9Cbriefing-pen-2/

## Acknowledgements
These articles helped me to learn the techniques. Their code is not included in this project.
 * Low level mouse hook : https://poorman.tistory.com/297
 * Global hot keys in WPF : https://social.technet.microsoft.com/wiki/contents/articles/30568.wpf-implementing-global-hot-keys.aspx
 * Moving a window without title bar : https://gigong.tistory.com/99
 * Undo/redo for InkCanvas : https://stackoverflow.com/questions/6368517
 * Raising a mouse event : https://stackoverflow.com/questions/2421304

## Development Environment
 * WPF C# on .NET 10
 * PixiEditor.ColorPicker (3.4.1) - MIT License

## Release Notes
 * 2024-05-26 (Alpha 12)
   - Fix Undo/redo bug
   - Shaking gesture was readopted.
   - Main contron window is always on drawing.
 * 2024-03-23 (Alpha 11)
   - Fix Eraser Bug (from Alpah10)
   - Fix Arrow Bug (arrow tip will not split with body)
   - Fix Background Bug (back1.png, back2.png will be possible)
   - New gesture (in eraser mode, touch left screen edge and right -> clear all)
 * 2024-03-15 (Alpha 10)
   - add Multi board, each board has own pen style, (shortcut F1, F2, F3) (pen swap will be deprecated.)
   - add temporary hide drawing (shortcut F1, F2, F3 - press again)
   - add palette swap, in setting you can choose twin color.
   - Right click works as eraser.
   - Quick quit - clear all and quit drawing at once (shortcut q)
 * 2024-03-02 (Alpha 8)
   - The taskbar is always visible when you press alt+tab between fullscreen applications.
 * 2024-01-18 (Alpha 7)
   - The eraser got bigger, you can change the size.
   - Draw arrow on the tip of your line with the shotcut (default : Left Shift)
   - Bug Fixed : mouse effects are available from now.
   - Changed to .Net 6.0
 * 2023-03-14 (Alpha 6)
   - Fix the dpi problem
   - Changed to .Net 7.0 (VS 2022)

## Future Works
 * More settings for the mouse effect.
 * Offering Background To Draw
 * Temporary hide

## License
Copyright (c) 2021-2026 ladofa

ThePen is free and will stay free. It is licensed under the [PolyForm Noncommercial License 1.0.0](LICENSE.md).
 * You may use, modify and redistribute it for any noncommercial purpose.
 * You must pass on the license and the `Required Notice` lines in [NOTICE](NOTICE) when you redistribute it.
 * Additional permission : you may run and use ThePen for any purpose, including at work, in paid lectures and in other commercial activities.
 * You may not sell ThePen, charge a fee for copies of it, or include it in a product or service that is sold or offered for a fee.

Third-party components and their licenses are listed in [THIRD-PARTY-NOTICES.md](THIRD-PARTY-NOTICES.md).

My plan is to upload this project to Microsoft Store for free.
