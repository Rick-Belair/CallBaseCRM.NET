Installation Instructions for CallBase Inbound 5

The following instructions are for the installation of version 5.1.3 of the
CallbaseCRM.net application.

It assumes that the application has the following installed:
- Web Catalog v5 or later
- Version 4 of the .net framework
- Oracle client v10 or above

The following changes have been made:
CRMNET-1	Developed By Nortak Software HTTP to HTTPS and Version Number Increase
CRMNET-3	The Campaign drop-down is appearing in other pop-up
CRMNET-4	The Order Status field is extending passed the border

-----------------------------------------------------------

Pre-deployment:

1a) Make a backup of existing files

   - Find the location where the CallbaseCRM .net application is installed.
     Copy all files and subdirectories to a secondary location. 

1b) Prepare the archive:

   - Download the file callbasecrm.net_5.1.2_patch_20240219.zip from the Nortak
     share site
   - From the downloaded zip file archive, copy the contents of the subdirectory 'app'
     to a temporary directory


Install Application

2a) Shut down IIS (optional)

   - Open up Internet Information Services Manager
   - Right-click on the web site containging webcatalog.net and select 'stop'

   Note that this step is optional. The application can usually be updated while the server
   is running.

2b) Copy application files

   - On the server, find the directory where the callbasecrm .net application is stored
     (CallBase Inbound 5.01 in TEST is http://ncwbina0097/CallBaseCRM/login.aspx
     (CallBase Inbound 5.01 in PROD is  http://ncwbina0100/callbasecrm5/login.aspx)

   - Go to the location where the application directory was extracted from the 
     install package (see step 1b above).
   - Take all subdirectories that were previously extracted in step 1b and copy 
     them to the application directory.

2c) Restart IIS

   - Open up Internet Information Services Manager
   - Right-click on the web site containing the web catalog and select 'start'

   Note that this only needs to be done if IIS was shut down (see step 2a)

Testing

4a) Verify Application(s)

   - Open up a web browser window
   - For the current web server, test the following application:

      http://(current web server)/(web directory)/login.aspx

     This will test whether the new application is installed correctly.
     You should see a login page with versioin 5.1.3 at the bottom of the screen



