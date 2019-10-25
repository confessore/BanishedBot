#!/bin/sh

sudo service banishedbot stop

cd /home/orfasanti/banishedbot
sudo git pull origin master

cd /home/orfasanti/banishedbot/src/BanishedBot
sudo dotnet publish -c Release -o /var/dotnetcore/BanishedBot

sudo service banishedbot start
