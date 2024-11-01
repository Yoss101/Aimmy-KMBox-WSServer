## Aimmy-KMBox-WSServer

This application is a websocket server listening for commands from [Aimmy-KMBox](https://github.com/Yoss101/Aimmy-KMBox). It then translate these commands into serial commands to the kmbox/arduino.
Required hardware (only one is needed):
- Kmbox B
- Kmbox B+
- KMBox B+ pro
- Arduino with usb host shield 2.0 and [Neoware Firmware](https://neoware.dev/product/neomouse-arduino-firmware/)*

> Note: While it is possible to build an arduino; soldering is required. If you do not or cannot do it yourself; you can purchase one from [Phoenix Dma](https://www.amazon.com/dp/B0DLKRW4Z1). Please do your own research own product avaliability and compatibility.

Required Software:
- [CH341SER driver](http://www.wch.cn/downloads/CH341SER_EXE.html)
- [Windows C++ AIO](https://www.techpowerup.com/download/visual-c-redistributable-runtime-package-all-in-one/)
- [Windows .net run time](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-8.0.10-windows-x64-installer)
- Any other needed drivers for your kmbox or arduino

>Note: KMBox-WSServer requires a client application located [here](https://github.com/Yoss101/Aimmy-KMBox).
