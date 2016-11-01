FROM microsoft/iis
MAINTAINER csolar@syndeonetwork.com
ARG CONT_IMG_VER
ENV CONT_IMG_VER ${CONT_IMG_VER:-v1.0.0}
LABEL Description="Pulse IIS" Vendor="Syndeo" Version=${CONT_IMG_VER}

RUN powershell -Command \
	Add-WindowsFeature Web-Server; \
	Add-WindowsFeature Web-Net-Ext45; \
	Add-WindowsFeature Web-AppInit; \
	Add-WindowsFeature Web-Asp-Net45;

	
RUN powershell -Command Stop-WebAppPool -name DefaultAppPool
RUN powershell -Command Stop-WebSite -name 'Default Web Site'

COPY ./* C:/inetpub/wwwroot/bin/
COPY *.config C:/inetpub/wwwroot/
COPY *.asax C:/inetpub/wwwroot/

RUN powershell -Command \
	Import-Module WebAdministration; \
	Set-ItemProperty 'IIS:/Sites/Default Web Site' -name applicationDefaults.preloadEnabled -value True; \
	Set-ItemProperty 'IIS:/Sites/Default Web Site' -name serverAutoStart -value True; \
	Set-ItemProperty 'IIS:/AppPools/DefaultAppPool' -name autoStart -value True; \
	Set-ItemProperty 'IIS:/AppPools/DefaultAppPool' -name startMode -value 'alwaysrunning'; \
	Set-ItemProperty 'IIS:/AppPools/DefaultAppPool' -name managedRuntimeVersion -value 'v4.0'; 

#RUN powershell -Command Start-WebAppPool -name DefaultAppPool
#RUN powershell -Command Start-WebSite -name 'Default Web Site'

EXPOSE 80

CMD sc stop wuauserv
CMD netsh advfirewall set allprofiles state off
