# Want a free dynamic DNS?
Well, so did I and since the most popular service which is built in most of the routers sold today (dyndns.com) only offers paid plans I have found another solution. The company called zerigo.com offers a free plan for one domain and 5 DNS records and also provides a nice API so the only thing remaining is to automate the DNS updates...well, that's what this project is about.

## What you're going to need
- An account at zerigo.com
- A domain which is either hosted or at least its DNS records are hosted at zerigo.com

## How to get it running?
- [Download the installation package](https://github.com/lholota/LH.ZerigoDynDns/releases/download/v1.0.2/LH.ZerigoDynDns.Setup.msi)
- Install it on your (home) server
- Update the config file (as described below)
- Start the service "LH Dynamic Dns for zerigo.com" and you are done

## Configuration
When you install the application, you will find the configuration file LH.ZerigoDynDns.Service.exe.config in the directory you installed the application into. In the config you will find the followin section:


     <zerigoDynDns checkIntervalInSeconds="1800">
         <credentials userName="" apiKey="" />
         <domains>
           <domain name="www1.mydomain.com" />
           <domain name="www2.mydomain.com" />
         </domains>
       </zerigoDynDns>

| Attribute | Description |
|:-------- |:----------- |
| checkIntervalInSeconds | This sets how often the service will check if the IP has changed. The default value is 30 minutes (1800 seconds) |
| userName | This is the user name you use to log into zerigo.com account |
| apiKey | This is **not** the password! <br />The API Key can be generated at zerigo.com account administration section. Simply go to DNS -> Preferences -> API Keys -> Click the "Generate a API Key" button. The key will appear in the table below. |
| domain name | This is the name of the (sub)domain you want to point to your current public IP. You can specify multiple domains. |

### Proxy
In case you would like to use a proxy server for all HTTP(S) calls made to zerigo, uncomment the following section in the configuration file:

    <system.net>
        <defaultProxy>
          <proxy autoDetect="False" 
                 bypassonlocal="True"
                 proxyaddress="http://localhost:8888"/>
        </defaultProxy>
      </system.net>
      
The values entered into the section is a sample of the simplest setup. For more advanced alternatives, please check the following MSDN article: https://msdn.microsoft.com/en-us/library/kd3cf2ex(v=VS.110).aspx

## Troubleshooting
The service does not start? The DNS records are not updated? There could be a number of things wrong, but there are some tools built in to help you find out what the problem is.

### Logging
By default the application logs all warnings and errors into the Windows Event Log. Check the log under Windows Logs - Application. The source of the events raised by the application is "LH.ZerigoDynDns".

### Console mode
The application can be run in console mode. In this case it logs all diagnostic messages into the console (level debug and above, therefore these messages are more detailed). Simply run the application with the parameters as shown below:

    LH.ZerigoDynDns.Service.exe --console
    
### Configuration check
There is another useful parameter you can execute the application with - check-config. In this case the application will run in console mode automatically and it will only load the configuration and verify that it is correct. If there is an error in the configuration, you will see it in the console output.

    LH.ZerigoDynDns.Service.exe --check-config
    
## License
This application is released under the [MIT license](https://github.com/lholota/LH.ZerigoDynDns/blob/master/LICENSE).
    
## Found a bug?
Please create an [issue](https://github.com/lholota/LH.ZerigoDynDns/issues/new) or fork the repo and submit a pull request :)
