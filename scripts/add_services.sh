#!/bin/sh

sudo systemctl stop banishedbot.service

sudo systemctl disable banishedbot.service

read -p "BanishedBot Discord Application Token: " discordToken

sudo cp ./services/banishedbot.service ./services/banishedbot.service.backup

sudo sed -i '/BanishedBothDiscordToken=/s/$/'"$discordToken"'/' ./services/banishedbot.service.backup

sudo mv ./services/banishedbot.service.backup /etc/systemd/system/banishedbot.service

sudo systemctl enable banishedbot.service

sudo systemctl start banishedbot.service
