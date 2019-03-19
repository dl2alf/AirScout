**************************************************************************************
Linux compatibility 
Last modified: 2018-02-09
**************************************************************************************

This software is running on Linux with the help of the Mono runtime environment without any modifications.
Some precautions are necessary depending on the your Linux system.
I cannot verify all existing variants but see some samples below.
After you have finished all preparations:

 - simple copy all the files in a directory of your choice
 - open a terminal window there
 - run AirScout Plane Server by typing "mono AirScoutPlaneServer.exe"

VERY IMPORTANT TO IMPORT SSL CERTIFICATES TO THE MONO CERTIFICATE STORE MANUALLY!!!
This guarantees the download from websites with SSL encryption (https://).
Mono is using its own certificate store different from system or Mozilla's web browser.
The synchronisation should be done automatically but I found it not working after downloading the Mono package. 	

**************************************************************************************
Installation samples
**************************************************************************************


*** Suse Linux 13.2 KDE 4 (64bit) ***

1. Install Mono 4.x complete package "mono-complete.ymp"
2. Install the "ca-certificates-mozilla" package to get SSL certificates
3. Import the certificates into the Mono certificate store by typing "sudo cert-sync /etc/ssl/ca-bundle.pem"	(can be a different location on other versions!)


*** Raspberry Pi V1 with jessie ***

The latest Mono package in the Raspian repository is V3.2.8. You will need to upgrade manually to V4.xxx.
1. Open a Terminal window.
2. Type this in or paste:

		sudo su -
		echo "deb http://plugwash.raspbian.org/mono4 jessie-mono4 main" >> /etc/apt/sources.list
		apt-get update

3. Please follow the instructions on https://www.raspberrypi.org/forums/viewtopic.php?f=34&t=99595 and https://www.raspberrypi.org/forums/viewtopic.php?f=38&t=105708&p=791212#p791212.
4. SSL IMPORT PROCEDURE NOT PROVEN!!!



