**************************************************************************************
AirScout Linux compatibility 
Last modified: 2018-04-14
**************************************************************************************

+++ INSTALLATION

This software is running on 32bit/64bit - Linux with the help of the Mono runtime environment without any modifications.
There is no special installation package or archive to download.

Nevertheless some precautions are necessary depending on the your Linux system.
Linux support remains experimental, I simply cannot verify all existing configurations.
Latest tests with AirScout were made on a 64bit Ubuntu Linux (21.10) with KDE-Desktop and Mono 6.8.0.105 in a VMWare virtual machine.

To get ist running on your system, please follow the steps below

1. Check if your version of Linux is up to date 
2. Get the latest Mono version information and install the full Mono package by running (see https://www.mono-project.com/):

	sudo apt install gnupg ca-certificates
	sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
	echo "deb https://download.mono-project.com/repo/ubuntu stable-focal main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list
	sudo apt update

	sudo apt-get install mono-complete	(on most systems)

	or 

	sudo zypper install mono-complete	(on SUSE Linux)

3. Unzip archive or simple copy all unpacked files in a directory of your choice, e.g. /home/[Linux Username]/AirScout
4. Open a terminal window there
5. Run AirScout by typing "mono AirScout.exe", if you have problems with language settings "LANG=en-EN mono AirScout.exe"
6. Have fun with AirScout running on Linux with 99% functionality


+++ KNOWN BUGS AND LIMITATIONS

- Web browser window not working
- Main window's last location, size and state cannot be saved properly --> AirScout is always starting maximized
- Splitter position cannot be saved properly --> use default splitter positions
- Frameless splash window on startup does not work --> show it with frame
- Startup Wizard cannot be shown with Aero design --> show it in standard form design (causes some errors in layout)
- some minor graphics issues, sometimes text does not fit in bounds
- some minor textbox select issues (text is not selected when clicking once in the MyCall or DXCall textbox)
- Settings upgrade from previous versions do not work, always use default settings when upgrading
- database view for maintenance does not work --> SQLite-DataAdapter issues
- Program sometimes does not close properly, stops working while saving settings to disk
- dropdowns for callsigns and locators disappear on startup on some configurations (seems to be a graphics issue) --> try to resize window to get them back
- filling the watch list ith calsigns is taking ages. 



VERY IMPORTANT!!! ON SOME OLDER/SMALLER SYSTEMS THE SSL ENCRYPTED CONNECTIONS ("https://......") DO NOT WORK
IN THIS CASE YOU HAVE TO IMPORT SSL CERTIFICATES TO THE MONO CERTIFICATE STORE MANUALLY!!!

Mono is using its own certificate store different from system or Mozilla's web browser.
The synchronisation should be done automatically but I found it not working sometimes after downloading the Mono package. 
In this case you will see a lot of Error messages in the Terminal window like "Error while reading chunked content: Cannot read application data on failed TLS connection"

1. Install the "ca-certificates-mono" package to get SSL certificates
2. Import the certificates into the Mono certificate store by typing "cert-sync /etc/ssl/certs/ca-certificates.crt"	(can be a different location on other versions!)




