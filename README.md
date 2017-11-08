# salesforce_rip_connector

Open Source Community: Starter kit for Salesforce Relativity Integration Point

verson .2 compatible with Relativity 9.5

While this is an open source project on the Relativity GitHub account, support is only available through through the Relativity developer community. You are welcome to use the code and solution as you see fit within the confines of the license it is released under. However, if you are looking for support or modifications to the solution, we suggest reaching out to a [Relativity Development Partner](https://www.relativity.com/ediscovery-software/app-hub/).

If the GenericPartner.wsdl file is refreshed there is a possiblitiy that a known issue with .NET WSDLE.exe will occur where a dataype is created incorrectly.  [https://support.microsoft.com/en-us/help/2486643/sco-unable-to-generate-a-temporary-class-result-1-error-when-you-execu]

This will result in error CS0030, the proxy class is generated incorrectly where the datatype has been inappropriately created as a two-dimensional array. The issue can be resolved by modifing the incorrect two-dimensional array to a single-dimensional array (ListViewRecordColumn[][] modified to ListViewRecordColumn[]
and ListViewColumn[][] to ListViewColumn[] in the \sForceService\Reference.cs).

Enhancements for the future : 
* Add Object Type next to a field that could  potentially have the same name as another field (In another object). Right now, we only pull the cases object from salesforce. If we were to add functionality to pull data in from other Object Types, this would be something we implement.
