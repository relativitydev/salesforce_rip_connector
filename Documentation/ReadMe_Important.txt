Salesforce RIP Provider Notes
-----------------------------

Logging into Salesforce:  In order to login to your Salesforce account through an application (Salesforce Provider) you need both your user password as well as the security token.  The security token is obtained in Salesforce by navigating under your username in the top right corner to Setup->My Personal Information->Reset My Security Token.  An email will be sent to you with the sercurity token which needs to be appended to your regular password.  

For example:

Salesforce web Login from browser: 
User id: myname@company.com
pwd: Test1234!

with emailed Security token:5AB0RSy3tr123JBeKS8f7RRu 

Salesforce Provider login credentials in Integration Point custom page would be:
User id: myname@company.com
pwd: Test1234!5AB0RSy3tr123JBeKS8f7RRu

Known issues:  The first release of this solution was to demonstrate basic functionality and is preliminary in nature.  The deadline to complete this project in time for Fest17 was tight and it resulted in items left uncompleted in order to leave enough time for first past QA. Basic error handling and exception trapping is minimal and should be expanded to provide the user important information when problems occur.  There are calls to output error messages to the Console which was used during initial prototyping that should be logged instead.  From a user perspective, the password input would be clearer if it was broken into two input fields, one for the password and another for the security token.  The code would take these two inputs and concatenate them together when logging into Salesforce.  General code cleanup and formatting is needed as well.