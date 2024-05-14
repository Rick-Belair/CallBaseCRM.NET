Installation Instructions for CallBase Inbound 5

The following instructions are for the installation of version 5.1.2 of the
CallbaseCRM.net application.

It assumes that the application has the following installed:
- Web Catalog v5 or later
- Version 4 of the .net framework
- Oracle client v10 or above

The following changes have been made:
- The Answer field will display a warning if the text entered is over 4000 characters
  and will be truncated when saved.
- The Questions/Comments field will display a warning if the text entered is over 4000 characters
  and will be truncated when saved.
- A fix for saving / displaying KnowledgeBase entries correctly.
- A fix to populate the  Customer Shipping Details Prov/State dropdown
  when the main Prov/State dropdown is changed.

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

Database Modification Steps
---------------------------   

3a) Go to the files extracted from the install package (see step 1b above) and find the files under the directory database scripts

3b) Log on to the oracle server under the callbase01 schema and run the following script:  insert_fhtmllabels.sql

3c) Log on to the oracle server under the callbase01 schema and run the following script: insert_fhtmllanguage.sql

[Note if the database scripts were previously run an error result will occur, this is normal and no damage can result from running these scripts a second time]

Testing

4a) Verify Application(s)

   - Open up a web browser window
   - For the current web server, test the following application:

      http://(current web server)/(web directory)/login.aspx

     This will test whether the new application is installed correctly.
     You should see a login page with versioin 5.1.2 at the bottom of the screen



