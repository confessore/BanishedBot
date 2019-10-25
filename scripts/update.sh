#!/bin/sh

sudo service banishedbot stop

cd /home/$USER/banishedbot
sudo git pull origin master

cd /home/$USER/banishedbot/src/BanishedBot
sudo dotnet publish -c Release -o /var/dotnetcore/BanishedBot

sudo service banishedbot start
